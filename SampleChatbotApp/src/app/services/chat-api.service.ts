import {inject, Injectable} from '@angular/core';
import {AuthService} from './auth.service';
import {ApiMessage} from '../models/api-message';
import {Observable} from 'rxjs';
import {HttpClient, HttpEvent} from '@angular/common/http';
import {environment} from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChatApiService {
  private readonly authService = inject(AuthService);
  private readonly httpClient = inject(HttpClient);

  private get defaultOptions() {
    return {
      headers: {
        "X-User-Name": this.authService.currentUserValue() as string
      },
    };
  }

  public upvoteMessage(messageId: string): Observable<string> {
    return this.httpClient.put<string>(`${environment.baseUrl}/${messageId}/upvote`, null, this.defaultOptions);
  }

  public downvoteMessage(messageId: string): Observable<string> {
    return this.httpClient.put<string>(`${environment.baseUrl}/${messageId}/downvote`, null, this.defaultOptions);
  }

  public getMessages(): Observable<ApiMessage[]> {
    return this.httpClient.get<ApiMessage[]>(`${environment.baseUrl}/history`, this.defaultOptions);
  }

  public createMessage(messageContent: string): Observable<HttpEvent<string>> {
    const options = {
      ...this.defaultOptions,
      observe: "events" as const,
      responseType: "text" as const,
      reportProgress: true,
    };

    const payload = {
      text: messageContent,
    };

    return this.httpClient.post(`${environment.baseUrl}/new`, payload, options);
  }
}
