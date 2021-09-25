import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ArticlesListComponent } from './articles/articles-list/articles-list.component';
import { ArticlesNewComponent } from './articles/articles-new/articles-new.component';
import { LoginComponent } from './login/login.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { AddWebsiteDetailsComponent } from './website-details/add-website-details/add-website-details.component';
import { EditWebsiteDetailsComponent } from './website-details/edit-website-details/edit-website-details.component';
import { WebsiteDetailsListComponent } from './website-details/website-details-list/website-details-list.component';

const routes: Routes = [
  // { path: '', redirectTo: '/articles', pathMatch: 'full' },
  { path: '', component: ArticlesListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'website-details', component: WebsiteDetailsListComponent },
  { path: 'website-details/add', component: AddWebsiteDetailsComponent },
  { path: 'website-details/edit/:id', component: EditWebsiteDetailsComponent },
  { path: 'articles', component: ArticlesListComponent },
  { path: 'articles-new', component: ArticlesNewComponent },
  { path: 'account', component: UserManagementComponent },
  { path: '**', component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
