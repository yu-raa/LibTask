import { NgForOf, NgIf, NgOptimizedImage } from "@angular/common";
import { AfterContentInit, ChangeDetectorRef, Component } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router, RouterOutlet } from "@angular/router";
import { AuthorService } from "../authors";
import { BookService } from "../books";
import { Author, Book, BookToShow, DataService, PageQuery, RespClass } from "../data";
import { AppComponent } from "../app.component";

@Component({
  selector: 'app-mine',
  standalone: true,
  imports: [ReactiveFormsModule, NgForOf, NgIf, RouterOutlet, NgOptimizedImage],
  templateUrl: './mybooks.component.html',
  styleUrl: './mybooks.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class OwnedBooksComponent implements AfterContentInit {
  books!: BookToShow[]
  authors!: Author[]
  page: number = 1
  pageSize: number = 20
  amount!: number
  role!: string
  isLoaded: boolean = false
  isLoggedIn: boolean
  notifiedToReturn: boolean = false

  constructor(private ds: DataService, private bs: BookService, private as: AuthorService, private rout: Router, private cd: ChangeDetectorRef, private route?: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngAfterContentInit(): Promise<void> {
    if (this.ds.getData() !== null) {
      this.isLoaded = true
      await this.onIn()
    }
  }

  async onIn() {
     if (this.isLoaded == true) {
      let authRes = await this.as.getAuthors(this.ds.getData())
      this.authors = authRes.key
      let res = await this.bs.getBooks(this.ds.getData(), 20, 1, null, null, null)
      this.books = res.key as BookToShow[]
      this.books = this.books.filter((val) => val.userId == this.ds.getData().id as string)
       for (let book of this.books) {
         book.notif = await this.bs.checkForNotif(this.ds.getData(), book)
        book.author = this.authors.find(obj => obj.id == book.authorId) as Author

      }
      DataService.amountOfBooks = this.books.length
      let logindata = this.ds.getData() as RespClass
      this.role = logindata != null ? logindata.roles as string : 'user'
      let data: any
      await this.route?.queryParams.forEach(res => {
        if (res instanceof PageQuery) {
          let res2 = res as PageQuery
          this.page = res2.page;
          this.pageSize = res2.pageSize;
        }
      }).catch(reas => data = reas)
    }
  }

  setToNoSelect(ev: Event) {
    let toggle = document.getElementById('pageS') as HTMLSelectElement
    toggle.selectedIndex = -1;
  }

  async decreasePage(ev: Event) {
    this.page--;

    await this.onIn()
  }

  async increasePage(ev: Event) {
    this.page++;
    await this.onIn()
  }

  async reloadAmount(ev: Event) {
    let toggle = document.getElementById('pageS') as HTMLSelectElement
    this.pageSize = Number.parseInt(toggle.options[toggle.selectedIndex].id)
    let res = await this.bs.getBooks(this.ds.getData(), this.pageSize, this.page, null, null, null)
    this.books = res.key as BookToShow[];
    this.books = this.books.filter((val, ind, arr) => val.userId == this.ds.getData().id as string)
    for (let book of this.books) {
      book.author = this.authors.find(obj => obj.id == book.authorId) as Author
    }
    DataService.amountOfBooks = this.books.length
    this.amount = DataService.amountOfBooks
  }

  goToBook(ev: Event, id: string) {
    this.rout.navigate([`bookDetails`], { queryParams: { id } })
  }
}
