import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { RegisterService } from '../signup';
import { NgIf } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { HttpResponse, HttpResponseBase } from '@angular/common/http';
import { DataService, RespClass } from '../data';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterOutlet],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  providers: [RegisterService, DataService, RouterOutlet]
})

export class SignUpComponent implements OnInit {
  signupForm: FormGroup = new FormGroup({
    email: new FormControl(),
    password: new FormControl(),
    isAdmin: new FormControl()
  })
;

  constructor(private fb: FormBuilder, private rs: RegisterService, private ds: DataService, private rout: Router) { }

  ngOnInit(): void {
  }

  async onSubmit() {
    if (this.signupForm.valid) {
      let mod = { isAdmin: this.signupForm.value.isAdmin == true, email: this.signupForm.value.email, password: this.signupForm.value.password }
      let res = await this.rs.signUp(mod) as HttpResponse<RespClass>
      if ((res as HttpResponseBase).statusText == "Bad Request") {
        let el = document.getElementById('alreadyReg')
        if (el != null) {
          el.innerHTML = "You have already signed up!"
          console.log('already signed up')
          return
        }
      }
      else if ((res as RespClass) !== null) {
        console.log('success')
        this.ds.saveLoginInfo(res as RespClass)
        AppComponent.loggedIn = true
        this.rout.navigate(['main'])
      }
      else {
        console.log("unknown error")
      }
      return
    }
    else {
      console.log('register error')
    }
  }
}
