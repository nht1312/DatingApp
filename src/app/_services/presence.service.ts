import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection } from '@microsoft/signalr';
import { HubConnectionBuilder } from '@microsoft/signalr/dist/esm/HubConnectionBuilder';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();

  constructor(private toastr: ToastrService, private router: Router) { }

  //Connection
  createHubConnection(user: User){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();
    
    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', username => {
      this.onlineUsers$.pipe(take(1)).subscribe({
        next: usernames => this.onlineUsersSource.next([...usernames, username])
      })
    })

    this.hubConnection.on('UserIsOffline', username => {
      this.onlineUsers$.pipe(take(1)).subscribe({
        next: usernames => this.onlineUsersSource.next(usernames.filter(x => x !== username ))
      })
    //status
    this.hubConnection?.on('GetOnlineUsers', usernames => {
      this.onlineUsersSource.next(usernames);
    });
    //notifications
    this.hubConnection?.on('NewMessageReceived', ({username, knowAs}) => {
      this.toastr.info(knowAs + ' has sent you a new message! Click me to see it')
        .onTap
        .pipe(take(1))
        .subscribe({
          next:  () => this.router.navigateByUrl('/members/' + username + '?tab=Messages')
        })
    } )
    })
  }
  //Stop Connection
  stopHubConnection(){
    this.hubConnection?.stop().catch(error => console.log(error));
  }
}
