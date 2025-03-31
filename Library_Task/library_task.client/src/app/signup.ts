import { ApplicationConfig, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http'
import { ReplaySubject, bufferTime, map } from 'rxjs';

@Injectable(
  {
    providedIn: 'root'
  }
) export class RegisterService {
  private url: string = "http://localhost:5029"
  private rs$: ReplaySubject<HttpResponseBase[]> = new ReplaySubject < HttpResponseBase[]>(1)

  constructor(private httpCl: HttpClient) { }

  async signUp(model: any) {
    let dat = new HttpResponse
    await this.httpCl.post<HttpResponse<string>>(`${this.url}/api/Register/`, model, { headers: (new HttpHeaders().set('Content-Type', 'application/json')) }).forEach(res => {
      dat = res;
    }).catch(reas => dat = reas)
    console.log(dat)
    if (dat.status == 200) {
      let data = new HttpResponse
      this.httpCl.post<HttpResponse<Object>>(`${this.url}/api/Login`, model, { headers: (new HttpHeaders().set('Content-Type', 'application/json')) }).subscribe(res => {
        data = res;
      })
      console.log(data)
      return data
    }
    return dat
  }
}
