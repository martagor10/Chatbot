import {Component} from '@angular/core';
import {FormBuilder, FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import {inject} from '@angular/core';
import {AuthService} from '../services/auth.service';
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';

@Component({
  selector: 'app-nickname',
  imports: [ReactiveFormsModule, MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule],
  templateUrl: './nickname.component.html',
  styleUrl: './nickname.component.scss'
})
export class NicknameComponent {
  private formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);

  nicknameForm = this.formBuilder.group({
    nickname: new FormControl('', Validators.required),
  });

  public submitForm(): void {
    if (this.nicknameForm.valid) {
      const user = this.nicknameForm.controls.nickname.value as string;
      this.authService.login(user);
    }
  }
}
