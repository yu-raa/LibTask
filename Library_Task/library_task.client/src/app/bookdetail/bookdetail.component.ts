import { NgForOf, NgIf, NgOptimizedImage } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router, RouterOutlet } from "@angular/router";
import { Author, Book, BookInfo, BookToShow, DataService, PageQuery, RespClass, User } from "../data";
import { AfterContentChecked, ChangeDetectorRef, Component, DoCheck, OnInit } from "@angular/core";
import { BookService } from "../books";
import { AuthorService } from "../authors";
import { MatDialog } from "@angular/material/dialog";
import { PopupComponent } from "../popup/popup.component";
import { AppComponent } from "../app.component";


@Component({
  selector: 'app-details',
  standalone: true,
  imports: [ReactiveFormsModule, NgForOf, NgIf, RouterOutlet, NgOptimizedImage],
  templateUrl: './bookdetail.component.html',
  styleUrl: './bookdetail.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class BookDetailComponent implements OnInit, DoCheck {
  books!: BookToShow[]
  authors!: Author[]
  book!: BookToShow
  role!: string
  isLoaded: boolean = false
  answerId!: string
  isLoggedIn: boolean

  constructor(private dial: MatDialog, private ds: DataService, private bs: BookService, private as: AuthorService, private rout: Router, private cd: ChangeDetectorRef, private route?: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngOnInit(): Promise<void> {
    await this.onIn()
  }

  ngDoCheck(): void {
    this.isLoaded = true
    this.cd.detectChanges()
  }

  async onIn() {
    let authRes = await this.as.getAuthors(this.ds.getData())
    this.authors = authRes.key
    let res = await this.bs.getBooks(this.ds.getData(), 20, 1, null, null, null)
    this.books = res.key as BookToShow[]
    for (let book of this.books) {
      book.author = this.authors.find(obj => obj.id == book.authorId) as Author
    }
    DataService.amountOfBooks = res.value
    let logindata = this.ds.getData() as RespClass
    this.role = logindata != null ? logindata.roles as string : 'user'
    let data: any
    await this.route?.queryParams.forEach(res => {
      this.book = this.books.find(book => book.id == res['id']) as BookToShow
    }).catch(reas => data = reas)
  }

  editBook(ev: Event) {
    let inf = new BookInfo(this.book)
    this.rout.navigate([`editBook`], { queryParams: { info: JSON.stringify(inf) } })
  }

  async deleteBook(ev: Event) {
    let ref = this.dial.open(PopupComponent, { data: { answerId: this.answerId } })
    await ref.afterClosed().forEach(res => { this.answerId = res.answerId }).catch(res => { this.answerId = res.answerId })
    ref.close()
    if (this.answerId == 'yesButton') {
      await this.bs.deleteBook(this.book, this.ds.getData() as RespClass)
      this.rout.navigate([`main`])
    }
  }

  async takeBook(ev: Event) {
    await this.bs.giveBook(this.ds.getData(), this.book.id)
    this.rout.navigate(['myBooks'], { queryParams: new PageQuery(1, 20) })
  }
}
