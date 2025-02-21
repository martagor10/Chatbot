import {AfterViewInit, Component, ElementRef, inject, ViewChild} from '@angular/core';
import {CommonModule, DatePipe} from '@angular/common';
import {ConversationService} from '../services/conversation.service';
import {MatCardModule} from '@angular/material/card';
import {MessageKind} from '../models/message-kind';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {RatingPipe} from '../shared/rating.pipe';
import {MessageRating} from '../models/message-rating';
import {AuthService} from '../services/auth.service';
import {LocalMessage} from '../models/api-message';

@Component({
  selector: 'app-conversation',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, RatingPipe],
  templateUrl: './conversation.component.html',
  styleUrl: './conversation.component.scss',
  providers: [DatePipe]
})
export class ConversationComponent implements AfterViewInit {
  @ViewChild('scrollTarget') private scrollTarget: ElementRef | undefined;

  protected readonly conversationService = inject(ConversationService);
  private readonly authService = inject(AuthService);
  public messages$ = this.conversationService.getMessages();
  public MessageKind = MessageKind;
  public MessageRating = MessageRating;
  protected currentUser = this.authService.currentUserValue();

  ngAfterViewInit() {
    this.conversationService.onConversationChanged.subscribe({
      next: () => {
        if (this.scrollTarget) {
          const newestElement = this.scrollTarget.nativeElement.lastElementChild;
          if (newestElement) {
            newestElement.scrollIntoView({ behavior: 'smooth' });
          }
        }
      }
    });
  }

  public upvoteMessage(message: LocalMessage): void {
    this.conversationService.upvoteMessage(message);
  }

  public downvoteMessage(message: LocalMessage): void {
    this.conversationService.downvoteMessage(message);
  }

  public cancelMessage(message: LocalMessage): void {
    this.conversationService.cancelMessage(message);
  }
}
