import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient, private presenceService: PresenceService) { }
  //save the account in the local storage when reloading
  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user){
          this.setCurrentUser(user);
          // console.log(user)
        }
      })
    )
  }
  //regiser
  regiter(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user)
        {
          this.setCurrentUser(user);
        }
      })
    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);

    // start connection
    this.presenceService.createHubConnection(user);
  }
  //remove the account in the local storage
  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    // stop connection
    this.presenceService.stopHubConnection();

  }
  //decoded token
  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]));
  }
}
