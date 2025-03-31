import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { SignUpComponent } from './register/register.component';
import { RouterModule } from '@angular/router';
import { AppRoutingModule, routes } from './app-routing.module';
import { provideHttpClient } from '@angular/common/http';

@NgModule({
  providers: [
    provideHttpClient(),
  ],
  declarations: [
  ],
  imports: [
    BrowserModule,
    LoginComponent,
    SignUpComponent,
    AppRoutingModule,
    AppComponent,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
