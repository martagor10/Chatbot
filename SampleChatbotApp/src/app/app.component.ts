import {Component, inject, OnInit} from '@angular/core';
import { LoginComponent } from './login/login.component';
import { ConversationComponent } from './conversation/conversation.component';
import {MessageFormComponent} from './message-form/message-form.component';
import {AuthService} from './services/auth.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [LoginComponent, ConversationComponent, MessageFormComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  private readonly authService = inject(AuthService);
  protected readonly currentUser = this.authService.currentUser();

  public ngOnInit(): void {
    this.authService.initUserFromLocalStorage();
  }
}
