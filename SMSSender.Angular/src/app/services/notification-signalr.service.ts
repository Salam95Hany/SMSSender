import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationSignalrService {
  Url = environment.signalRUrl;
  private hubConnection!: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.Url)
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Error)
      .build();

    this.hubConnection.start().then(() => { }).catch(err => console.log(err));
  }

  onMessageAdded(callback: () => void) {
    this.hubConnection.on(
      'Message_Added', () => {
        callback();
      }
    );
  }
}
