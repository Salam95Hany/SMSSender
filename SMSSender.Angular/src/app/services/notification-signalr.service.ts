import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalrService {
  Url = environment.signalRUrl;
  private hubConnection!: signalR.HubConnection;
  private authService = inject(AuthService);

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.Url, {
        accessTokenFactory: () => this.authService.UserModel?.token ?? null
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Error)
      .build();

    this.hubConnection.start().then(() => { }).catch(err => console.log(err));
  }

  onMessageAdded(callback: (operationType: number) => void) {
    this.hubConnection.on('Message_Added', (operationType: number) => { callback(operationType); });
  }

  onMessageCalculated(callback: (messageTransactionId: number) => void) {
    this.hubConnection.on('Message_Calculated', (messageTransactionId: number) => { callback(messageTransactionId); });
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().then(() => { }).catch(err => console.log(err));
    }
  }
}
