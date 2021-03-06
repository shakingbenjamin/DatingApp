import { Injectable } from '@angular/core';
import {
  Resolve,
  Router,
  ActivatedRouteSnapshot,
} from '@angular/router';

import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<User[]> {
  // a class to make sure that the data is loaded before getting to the page so the ? operator isn't needed on components
  pageNumber = 1;
  pageSize = 4;
  likesParams = 'Likers';
  
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}
  // when the route is hit, the resolver gets the data required for the component and catches any error returned.
  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParams).pipe(
      catchError((error) => {
        this.alertify.error('problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
