import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase } from '@angular/common/http'
import { BookService } from './books';
import { Params } from '@angular/router';

export class PageQuery implements Params {
  constructor(public page: number,
    public pageSize: number) {

    }
}

export class RespClass {
  constructor(tokenIn?: string,
    idIn?: string,
    isAdmIn?: string) {
    this.token = tokenIn
    this.id = idIn
    this.roles = isAdmIn
  }

  token?: string;
  id?: string;
  roles?: string;
}

export class Author {
  id: string;
  name: string;
  surname: string;
  dateOfBirth: Date;
  countryOfOrigin: string;

  constructor(id: string, name: string, surname: string, dob: Date, country: string) {
    this.id = id
    this.name = name
    this.surname = surname
    this.dateOfBirth = dob
    this.countryOfOrigin = country
  }

  toString() {
    return this.surname + ' ' + this.name + ' born ' + this.getDob() + ' in ' + this.countryOfOrigin
  }

  getId() {
    return this.id as unknown as string
  }

  getName() {
    return this.name
  }

  getSurname() {
    return this.surname
  }

  getCountry() {
    return this.countryOfOrigin
  }

  getDob() {
    return this.dateOfBirth.toDateString()
  }
}

export class User {
  constructor(idd: string, isadm: boolean, email: string, password: string) {
    this.id = idd;
    this.email = email;
    this.isAdmin = isadm;
    this.password = password;
  }

  id!: string;
  isAdmin!: boolean;
  email!: string;
  password!: string;


  getId() {
    return this.id as unknown as string
  }
}

export class Book {
  constructor(id: string, isbn: string, genre: string, title: string, descr: string, image: string = 'https://media.istockphoto.com/id/1335708681/photo/stacks-of-books-for-teaching-knowledge-college-library-green-background.jpg?s=612x612&w=0&k=20&c=xsqB09d-hvAbJrnVASDyEd27fn11jSxpgRBDy-eaET0=', lastTaken: Date, lastToReturn: Date, authi: string, usi: string) {
    this.authorId = authi
    this.userId = usi
    this.description = descr
    this.genre = genre
    this.bookImage = image
    this.title = title
    this.id = id
    this.isbn = isbn
    this.lastTaken = lastTaken
    this.lastToReturn = lastToReturn
  }

  isbn!: string;
  genre!: string;
  title!: string;
  id!: string;
  description!: string;
  bookImage?: string;
  lastTaken: Date;
  lastToReturn?: Date;
  userId!: string;
  authorId!: string;
}

export class BookToShow {
  isbn!: string;
  genre!: string;
  title!: string;
  id!: string;
  description!: string;
  bookImage?: string;
  lastTaken!: Date;
  lastToReturn?: Date;
  userId!: string;
  authorId!: string;
  author!: Author;
  user!: User;
  notif!: boolean;
}

@Injectable(
  {
    providedIn: 'root'
  }
) export class DataService {
  static amountOfBooks: number

  constructor() { }

  getData() {
    return JSON.parse(localStorage.getItem('loginInfo') as string) as RespClass
  }

  saveLoginInfo(info: RespClass) {
    localStorage.setItem('loginInfo', JSON.stringify(info))
  }

  getCache(books: Book[], ids: string[]) {
    for (let id of ids) {
        books[books.findIndex(book => book.id === id)].bookImage = localStorage.getItem('bookInfo' + id) as string
      }
     return books
  }

  savePicCache(info: Book[]) {
    for (let book of info) {
      localStorage.setItem('bookInfo' + book.id, JSON.stringify(book.bookImage))
    }
  }

  deleteLoginInfo() {
    localStorage.removeItem('loginInfo')
  }
}

export type bookInfoTuple = { key: Book[], value: number }

export type authorInfoTuple = { key: Author[], value: number }

export class BookInfo {
  bookInfo: BookToShow

  constructor(info: BookToShow) {
    this.bookInfo = info
  }
}
