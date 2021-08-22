import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AddWebsiteDetailsComponent } from './website-details/add-website-details/add-website-details.component';
import { WebsiteDetailsListComponent } from './website-details/website-details-list/website-details-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/website-details', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },  
  { path: 'website-details', component: WebsiteDetailsListComponent,  },
  { path: 'website-details/add', component: AddWebsiteDetailsComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
