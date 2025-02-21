import {inject, Injectable} from '@angular/core';
import {LocalMessage} from '../models/api-message';
import {BehaviorSubject, Observable, Subject, Subscription} from 'rxjs';
import {HttpDownloadProgressEvent, HttpEvent, HttpEventType} from '@angular/common/http';
import {MessageKind} from '../models/message-kind';
import {ChatApiService} from './chat-api.service';
import {MessageRating} from '../models/message-rating';

const MessageHeaders = {
  UserMessageId: 'X-User-Message-Id',
  UserMessageDate: 'X-User-Message-Date',
  ChatbotMessageId: 'X-Chatbot-Message-Id',
  ChatbotMessageDate: 'X-Chatbot-Message-Date',
}

@Injectable({
  providedIn: 'root'
})
export class ConversationService {
  private readonly chatApiService = inject(ChatApiService);

  private conversationHistory = new BehaviorSubject<LocalMessage[]>([]);
  private messageInProgress$ = new BehaviorSubject<boolean>(false);
  private conversationChanged$ = new Subject<void>();
  private messageInProgressSubscription: Subscription | undefined = undefined;
  private currentResponse: LocalMessage | undefined = undefined;

  public get inProgress(): Observable<boolean> {
    return this.messageInProgress$.asObservable();
  }

  public get onConversationChanged(): Observable<void> {
    return this.conversationChanged$.asObservable();
  }

  public upvoteMessage(message: LocalMessage): void {
    this.chatApiService.upvoteMessage(message.id)
      .subscribe({
        next: () => {
          message.rating = MessageRating.Positive
        }
      });
  }

  public downvoteMessage(message: LocalMessage): void {
    this.chatApiService.downvoteMessage(message.id)
      .subscribe({
        next: () => {
          message.rating = MessageRating.Negative
        }
      });
  }

  public getMessages(): Observable<LocalMessage[]> {
    this.chatApiService.getMessages()
      .subscribe({
        next: (result) => {
          this.conversationHistory.next(result);
          this.conversationChanged$.next();
        }
      });

    return this.conversationHistory.asObservable();
  }

  public createMessage(text: string): void {
    this.messageInProgress$.next(true);

    this.messageInProgressSubscription = this.chatApiService.createMessage(text)
      .subscribe({
        next: (event: HttpEvent<string>) => {
          this.handleCreateMessageResponse(event, text);
        },
        complete: () => {
          this.messageInProgress$.next(false);
          this.messageInProgressSubscription = undefined;

          if (this.currentResponse) {
            this.currentResponse.inProgress = false;
            this.currentResponse = undefined;
          }
        }
      });
  }

  public cancelMessage(message: LocalMessage): void {
    if (!message.inProgress || !this.messageInProgressSubscription) return;

    this.messageInProgressSubscription.unsubscribe();
    this.messageInProgress$.next(false);
    this.messageInProgressSubscription = undefined;

    if (this.currentResponse) {
      this.currentResponse.inProgress = false;
      this.currentResponse = undefined;
    }
  }

  private handleCreateMessageResponse(event: HttpEvent<string>, text: string): void {
    if (event.type === HttpEventType.ResponseHeader) {
      const userMessage = {
        id: event.headers.get(MessageHeaders.UserMessageId)!,
        kind: MessageKind.User,
        text: text,
        createdAt: new Date(Date.parse(event.headers.get(MessageHeaders.UserMessageDate)!)),
      };

      const chatbotMessage = {
        id: event.headers.get(MessageHeaders.ChatbotMessageId)!,
        kind: MessageKind.Chatbot,
        text: '',
        createdAt: new Date(Date.parse(event.headers.get(MessageHeaders.ChatbotMessageDate)!)),
        inProgress: true
      };

      this.conversationHistory.next([...this.conversationHistory.getValue(), userMessage, chatbotMessage]);
      this.currentResponse = chatbotMessage;
    } else if (event.type === HttpEventType.DownloadProgress && this.currentResponse) {
      this.currentResponse.text = (event as HttpDownloadProgressEvent).partialText || '';
    } else if (event.type === HttpEventType.Response && this.currentResponse) {
      this.currentResponse.text = event.body || '';
    }

    this.conversationChanged$.next();
  }
}
