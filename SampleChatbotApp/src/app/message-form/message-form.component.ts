import {Component, inject} from '@angular/core';
import {FormBuilder, FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../services/auth.service';
import {ConversationService} from '../services/conversation.service';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatCardModule} from '@angular/material/card';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {CommonModule} from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-message-form',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatCardModule, MatProgressBarModule, MatButtonModule, MatIconModule, CommonModule, MatProgressSpinnerModule],
  templateUrl: './message-form.component.html',
  styleUrl: './message-form.component.scss'
})
export class MessageFormComponent {
  private formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly conversationService = inject(ConversationService);
  protected currentUser$ = this.authService.currentUser();
  protected messageCreationInProgress$ = this.conversationService.inProgress;

  messageForm = this.formBuilder.group({
    message: new FormControl('', Validators.required),
  });

  submitMessage(): void {
    if (this.messageForm.valid) {
      const messageContent = this.messageForm.controls.message.value as string
      this.conversationService.createMessage(messageContent);
      this.messageForm.reset();
    }
  }
}
