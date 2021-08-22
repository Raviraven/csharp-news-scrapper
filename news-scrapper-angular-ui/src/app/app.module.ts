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

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AddWebsiteDetailsComponent,
    WebsiteDetailsListComponent
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
