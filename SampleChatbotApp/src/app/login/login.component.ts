import {Component, inject} from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatDialog} from '@angular/material/dialog';
import {NicknameComponent} from '../nickname/nickname.component';
import {AuthService} from '../services/auth.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [MatButtonModule, MatIconModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private readonly dialog = inject(MatDialog);
  private readonly authService = inject(AuthService);
  protected currentUser$ = this.authService.currentUser();

  openDialog(): void {
    this.dialog.open(NicknameComponent);
  }

  logout(): void {
    this.authService.logout();
  }
}
