import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable()
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  getUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(this.baseUrl + 'users');
  }

  getUser(id: number): Observable<User> {
    return this.httpClient.get<User>(this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User) {
    return this.httpClient.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(userId: number, photoId: number) {
    return this.httpClient.post(this.baseUrl + 'users/' + userId + '/photos/' + photoId + '/setMain', {})
  }

  deletePhoto(userId: number, photoId: number){
    return this.httpClient.delete(this.baseUrl + 'users/' + userId + '/photos/' + photoId)
  }
}
