import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

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
import { AddCategoryComponent } from './categories/add-category/add-category.component';
import { CategoriesComponent } from './categories/categories/categories.component';
import { EditCategoryComponent } from './categories/edit-category/edit-category.component';
import { LoadingComponent } from './loading/loading.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DialogWindowComponent } from './components/delete-dialog-window/dialog-window.component';
import { DialogWindowContentComponent } from './components/delete-dialog-window/dialog-window-content/dialog-window-content.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MaterialModule } from './components/material.module';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';

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
    UserManagementComponent,
    AddCategoryComponent,
    CategoriesComponent,
    EditCategoryComponent,
    LoadingComponent,
    DialogWindowComponent,
    DialogWindowContentComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MaterialModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
