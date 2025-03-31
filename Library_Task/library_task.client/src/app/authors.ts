import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Author, RespClass, authorInfoTuple } from "./data";

@Injectable(
  {
    providedIn: 'root'
  }
) export class AuthorService {
  private url: string = "http://localhost:5029"

  constructor(private httpCl: HttpClient) { }

  async addAuthor(newAuthor: any, dat: RespClass) {
    let data: any, body = JSON.stringify(newAuthor)
    await this.httpCl.post<HttpResponseBase>(`${this.url}/api/Authors/admin/add`, body, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res; console.log(res)
    }).catch(reas => {
      data = reas; console.log(reas)
    })
    console.log(data)
    return await this.getAuthors(dat)
  }

  async getAuthors(data: any) {
    let dataAbtPages = `page${1}?pageSize=${20}`
    if (data[0] !== undefined) data = data[0]
    if (JSON.stringify(data).includes("admin")) {
      let dat = <RespClass>data
      await this.httpCl.get<HttpResponse<string>>(`${this.url}/api/Authors/admin/` + dataAbtPages, {
        headers: (new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + dat.token
        }))
      }).forEach(res => {
        data = res;
      })
      console.log(data)
      return data as authorInfoTuple
    }
    else {
      let dat = <RespClass>data
      await this.httpCl.get<HttpResponse<string>>(`${this.url}/api/authors/user/` + dataAbtPages, {
        headers: (new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + dat.token
        }))
      }).forEach(res => {
        data = res;
      }).catch(reas => data = reas)
      console.log(data)
      return data as authorInfoTuple
    }
  }
}
