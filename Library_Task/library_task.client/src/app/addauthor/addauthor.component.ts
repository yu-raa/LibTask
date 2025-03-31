import { NgIf } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Params, Router, RouterOutlet } from "@angular/router";
import { Author, Book, DataService, PageQuery, RespClass } from "../data";
import { Component, OnInit } from "@angular/core";
import { BookService } from "../books";
import { HttpResponse } from "@angular/common/http";
import { AuthorService } from "../authors";
import { AppComponent } from "../app.component";

@Component({
  selector: 'app-addauthor',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, RouterOutlet],
  templateUrl: './addauthor.component.html',
  styleUrl: './addauthor.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class AddAuthorComponent implements OnInit {
  author!: Author
  page!: number
  pageSize!: number
  isLoggedIn: boolean

  constructor(private as: AuthorService, private ds: DataService, private bs: BookService, private rout: Router, private route: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngOnInit(): Promise<void> {
    await this.onIn()
  }

  async onIn() {
    let data: any
    await this.route.queryParams.forEach(res => {
      if (res instanceof PageQuery) {
        let res2 = res as PageQuery
        this.page = res2.page;
        this.pageSize = res2.pageSize;
      }
    }).catch(reas => data = reas)
  }

  async create(ev: Event) {
    let count: number = 1
    let authors = (await this.as.getAuthors(this.ds.getData())).key
    count = Number.parseInt(authors[authors.length - 1].id) + 1
    this.author = new Author(count.toString(), (document.getElementById('name') as HTMLInputElement).value as string, (document.getElementById('surname') as HTMLInputElement).value as string, new Date(Date.parse((document.getElementById('dob') as HTMLInputElement).value as string)), (document.getElementById('country') as HTMLInputElement).value as string)
    await this.as.addAuthor(this.author, this.ds.getData() as RespClass)
      this.rout.navigate([`addBook`], { queryParams: new PageQuery(this.page, this.pageSize) })
  }
}
