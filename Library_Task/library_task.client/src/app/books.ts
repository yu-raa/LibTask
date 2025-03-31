import { HttpClient, HttpHeaders, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Book, RespClass, bookInfoTuple } from "./data";

@Injectable(
  {
    providedIn: 'root'
  }
) export class BookService {
  private url: string = "http://localhost:5029"

  constructor(private httpCl: HttpClient) { }

  async addBook(newBook: Book, dat: RespClass, pageSize: number, page: number) {
    let data: any
    await this.httpCl.post<HttpResponse<string>>(`${this.url}/api/Books/add`, newBook, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res;
    }).catch(reas => data = reas)
    console.log(data)
  }

  async checkForNotif(dat: RespClass, book: Book) {
    let data: any
    await this.httpCl.post<HttpResponse<boolean>>(`${this.url}/api/Books/notif?id1=${book.id}`, dat, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res;
    }).catch(reas => data = reas)
    console.log(data)
    return data
  }

  async editBook(newBook: Book, dat: RespClass) {
    let data: any
    await this.httpCl.post<HttpResponse<string>>(`${this.url}/api/Books/edit`, newBook, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res;
    }).catch(reas => data = reas)
    console.log(data)
  }

  async giveBook(dat: RespClass, bookId: string) {
    let data: any
    await this.httpCl.post<HttpResponse<string>>(`${this.url}/api/Books/get?id1=${bookId}`, dat, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res;
    }).catch(reas => data = reas)
    console.log(data)
  }

  async deleteBook(book: Book, dat: RespClass) {
    let data: any
    await this.httpCl.post<HttpResponse<string>>(`${this.url}/api/Books/delete`, book, {
      headers: (new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + dat.token
      }))
    }).forEach(res => {
      data = res;
    }).catch(reas => data = reas)
    console.log(data)
  }

  async getBooks(data: any, pageSize: number, page: number, search: string | null, filterAuth: string | null, filterGenre: string | null) {
    let data2: any
    let dataAbtPages = ""
    dataAbtPages += `page${page}?pageSize=${pageSize}`
    if (search !== null && search.length > 0)
      dataAbtPages += `&search=${search}`
    if (filterAuth !== null && filterAuth.length > 0 && filterAuth !== "no1")
      dataAbtPages += `&authToFilter=${filterAuth}`
    if (filterGenre !== null && filterGenre.length > 0 && filterGenre !== "no2")
      dataAbtPages += `&genreToFilter=${filterGenre}`
    if (data[0] !== undefined) data = data[0]
    let dat = <RespClass>data
      await this.httpCl.get<HttpResponse<string>>(`${this.url}/api/Books/` + dataAbtPages, {
        headers: (new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ' + dat.token
        }))
      }).forEach(res => {
        data2 = res;
      })
      console.log(data2)
      return data2 as bookInfoTuple
  }
}
