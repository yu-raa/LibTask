import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AppComponent } from "./app.component";

@Injectable(
  {
    providedIn: 'root'
  }
) export class LogoutService {
  private url: string = "http://localhost:5029"

  constructor(private httpCl: HttpClient) { }

  logout() {
    this.httpCl.get(`${this.url}/api/Logout`)
    AppComponent.loggedIn = false
  }
}
