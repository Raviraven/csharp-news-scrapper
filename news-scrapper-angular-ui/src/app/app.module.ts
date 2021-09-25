import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AuthInterceptor } from './interceptors/auth-interceptor';

import { AddWebsiteDetailsComponent } from './website-details/add-website-details/add-website-details.component';
import { WebsiteDetailsListComponent } from './website-details/website-details-list/website-details-list.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { EditWebsiteDetailsComponent } from './website-details/edit-website-details/edit-website-details.component';
import { ArticlesListComponent } from './articles/articles-list/articles-list.component';
import { AuthErrorsComponent } from './auth-errors/auth-errors.component';
import { ArticlesNewComponent } from './articles/articles-new/articles-new.component';
import { JwPaginationComponent } from './jw-pagination/jw-pagination.component';
import { HamburgerToggleDirective } from './directives/hamburger-toggle.directive';
import { UserManagementComponent } from './user-management/user-management.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AddWebsiteDetailsComponent,
    WebsiteDetailsListComponent,
    SidenavComponent,
    PageNotFoundComponent,
    EditWebsiteDetailsComponent,
    ArticlesListComponent,
    AuthErrorsComponent,
    ArticlesNewComponent,
    JwPaginationComponent,
    HamburgerToggleDirective,
    UserManagementComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
