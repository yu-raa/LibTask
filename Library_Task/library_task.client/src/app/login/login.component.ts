import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { LoginService } from '../signin';
import { NgIf } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { RespClass } from '../data';
import { DataService } from '../data';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterOutlet],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  providers: [LoginService, DataService, RouterOutlet]
})

export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({
    email: new FormControl(),
    password: new FormControl()
  })
    ;

  constructor(private fb: FormBuilder, private rs: LoginService, private ds: DataService, private rout: Router) { }

  ngOnInit(): void {
  }

  async onSubmit() {
    if (this.loginForm.valid) {
      let signinRes = await this.rs.logIn(this.loginForm.value)
      this.ds.saveLoginInfo(signinRes as RespClass)
      AppComponent.loggedIn = true
      this.rout.navigate(['main'])
    }
  }
}
