import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http'
import { RespClass } from './data';

@Injectable(
  {
    providedIn: 'root'
  }
) export class LoginService {
  private url: string = "http://localhost:5029"

  constructor(private httpCl: HttpClient) { }

  async logIn(model: any) {
    let data = new HttpResponse<Object>
    await this.httpCl.post<HttpResponse<Object>>(`${this.url}/api/Login`, model, { headers: (new HttpHeaders().set('Content-Type', 'application/json')) }).forEach(res => {
      data = res;
    })
    console.log(data)
    return data
  }
}
