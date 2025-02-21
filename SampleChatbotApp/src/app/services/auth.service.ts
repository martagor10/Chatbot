import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userLoggedIn$ = new BehaviorSubject<string | null>(null);
  private readonly nicknameKey = 'nickname';

  constructor() {}

  public login(nickname: string): void {
    this.userLoggedIn$.next(nickname);
    localStorage.setItem(this.nicknameKey, nickname);
  }

  public logout(): void {
    this.userLoggedIn$.next(null);
    localStorage.removeItem(this.nicknameKey);
  }

  public currentUser(): Observable<string | null> {
    return this.userLoggedIn$.asObservable();
  }

  public currentUserValue(): string | null {
    return this.userLoggedIn$.getValue();
  }

  public initUserFromLocalStorage(): void {
    const user = this.getUserFromLocalStorage();
    if (user) {
      this.userLoggedIn$.next(user);
    }
  }

  private getUserFromLocalStorage(): string | null {
    return localStorage?.getItem(this.nicknameKey);
  }
}
