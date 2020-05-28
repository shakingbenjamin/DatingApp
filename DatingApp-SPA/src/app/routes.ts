import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/memeber-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'messages', component: MessagesComponent },
      { path: 'members', component: MemberListComponent, resolve: { users: MemberListResolver } },
      // when navigating to members, the resolver is called to make sure
      { path: 'members/:id', component: MemberDetailComponent, resolve: { user: MemberDetailResolver } },
      { path: 'member/edit', component: MemberEditComponent, resolve: { user: MemberEditResolver }, canDeactivate: [PreventUnsavedChanges] },
      { path: 'lists', component: ListsComponent , resolve: {users: ListsResolver}},
    ],
  },
  { path: '**', redirectTo: '', pathMatch: 'full' },
];
