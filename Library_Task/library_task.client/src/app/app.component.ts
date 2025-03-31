import { Component, OnChanges, OnInit } from '@angular/core';
import { AppRoutingModule} from './app-routing.module';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { SignUpComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { DataService } from './data';
import { NgIf } from '@angular/common';
import { LogoutService } from './lo';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, SignUpComponent, LoginComponent, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})

export class AppComponent implements OnInit {
  public static loggedIn: boolean
  protected classReference = AppComponent

  constructor(private ds: DataService, private rout: Router, private ls: LogoutService) {
    AppComponent.loggedIn = (this.ds.getData() != null && this.ds.getData() != undefined)
  }

  ngOnInit() {
  }

  logout(ev: Event) {
    this.ds.deleteLoginInfo()
    this.ls.logout()
    this.rout.navigate([''])
  }
}
