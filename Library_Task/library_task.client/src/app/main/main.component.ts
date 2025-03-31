import { NgForOf, NgIf, NgOptimizedImage } from "@angular/common";
import { AfterViewInit, Component, ElementRef, ViewChild } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Router, RouterOutlet } from "@angular/router";
import { AuthorService } from "../authors";
import { BookService } from "../books";
import { Author, Book, BookToShow, DataService, PageQuery, RespClass, bookInfoTuple } from "../data";
import { AppComponent } from "../app.component";

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [ReactiveFormsModule, NgForOf, NgIf, RouterOutlet, NgOptimizedImage],
  templateUrl: './main.component.html',
  styleUrl: './main.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class MainComponent implements AfterViewInit {
  @ViewChild('mysel1') mysel1!: ElementRef | undefined;
  @ViewChild('inputt') inp!: ElementRef | undefined;
  @ViewChild('mysel2') mysel2!: ElementRef | undefined;
  genres!: string[]
  books!: BookToShow[]
  authors!: Author[]
  page: number = 1
  pageSize: number = 20
  amount!: number
  role!: string
  isLoggedIn: boolean
  isOnInCalled = false
  searchInput!: string
  sel1!: number
  sel2!: number

  constructor(private ds: DataService, private bs: BookService, private as: AuthorService, private rout: Router, private route?: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngAfterViewInit(): Promise<void> {
    if (this.ds.getData() !== null) {
      await this.onIn(true)
    }
  }

  async onIn(firstUse: boolean) {
    if (firstUse) {
      let authRes = await this.as.getAuthors(this.ds.getData())
      this.authors = authRes.key
      let res: bookInfoTuple
      let sel = this.mysel1?.nativeElement as HTMLSelectElement
      let filterAuth = sel.selectedOptions[0].id
      this.sel1 = sel.selectedIndex
      sel = this.mysel2?.nativeElement as HTMLSelectElement
      let filterGenre = sel.selectedOptions[0].id
      this.sel2 = sel.selectedIndex
      res = await this.bs.getBooks(this.ds.getData(), this.pageSize, this.page, (this.inp?.nativeElement as HTMLInputElement).value as string | null, filterAuth, filterGenre)
      this.books = res.key as BookToShow[]
      this.genres = this.books.map(book => book.genre)
      DataService.amountOfBooks = res.value
    }
    else {
      let res: bookInfoTuple
      let sel = this.mysel1?.nativeElement as HTMLSelectElement
      sel.selectedIndex = this.sel1
      let filterAuth = sel.selectedOptions[0].id
      sel = this.mysel2?.nativeElement as HTMLSelectElement
      sel.selectedIndex = this.sel2
      let filterGenre = sel.selectedOptions[0].id
      res = await this.bs.getBooks(this.ds.getData(), this.pageSize, this.page, (this.inp?.nativeElement as HTMLInputElement).value as string | null, filterAuth, filterGenre)
      this.books = res.key as BookToShow[]
      DataService.amountOfBooks = res.value
    }
    this.genres = [...new Set(this.genres)];
      for (let book of this.books) {
        book.author = this.authors.find(obj => obj.id == book.authorId) as Author
      }
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

  async search(ev: Event) {
    let sel = this.mysel1?.nativeElement as HTMLSelectElement
    let filterAuth = sel.selectedOptions[0].id
    this.sel1 = sel.selectedIndex
    sel = this.mysel2?.nativeElement as HTMLSelectElement
    let filterGenre = sel.selectedOptions[0].id
    this.sel2 = sel.selectedIndex
    let res = await this.bs.getBooks(this.ds.getData(), this.pageSize, this.page, (this.inp?.nativeElement as HTMLInputElement).value as string | null, filterAuth, filterGenre)
    this.books = res.key as BookToShow[];
    for (let book of this.books) {
      book.author = this.authors.find(obj => obj.id == book.authorId) as Author
    }
    DataService.amountOfBooks = res.value
    this.amount = DataService.amountOfBooks
  }

  async seeMine(ev: Event) {
    this.rout.navigate(['myBooks'], { queryParams: new PageQuery(this.page, this.pageSize) })
  }

  async takeTheBook(ev: Event, id: string) {
    await this.bs.giveBook(this.ds.getData(), id)
    this.rout.navigate(['myBooks'], { queryParams: new PageQuery(this.page, this.pageSize) })
  }

  createNewBook(ev: Event) {
    this.rout.navigate([`addBook`], { queryParams: new PageQuery(this.page, this.pageSize) })
  }

  async decreasePage(ev: Event) {
    this.page--;
    await this.onIn(false)
  }

  setToNoSelect(ev: Event) {
    let toggle = ev.target as HTMLSelectElement
    toggle.selectedIndex = -1;
  }

  async increasePage(ev: Event) {
    this.page++;
    await this.onIn(false)
  }

  async reloadAmount(ev: Event) {
    let toggle = document.getElementById('pageS') as HTMLSelectElement
    this.pageSize = Number.parseInt(toggle.options[toggle.selectedIndex].id)
    let sel = document.getElementById('filterAuthSel') as HTMLSelectElement
    let filterAuth = sel.selectedOptions[0].id
    this.sel1 = sel.selectedIndex
    sel = document.getElementById('filterGenreSel') as HTMLSelectElement
    let filterGenre = sel.selectedOptions[0].id
    this.sel2 = sel.selectedIndex
    let res = await this.bs.getBooks(this.ds.getData(), this.pageSize, this.page, document.getElementById('searchTerm')?.textContent as string | null, filterAuth, filterGenre)

    this.books = res.key as BookToShow[];
    for (let book of this.books) {
      book.author = this.authors.find(obj => obj.id == book.authorId) as Author
    }
    DataService.amountOfBooks = res.value
    this.amount = DataService.amountOfBooks
  }

  goToBook(ev: Event, id: string) {
    this.rout.navigate([`bookDetails`], { queryParams: { id } })
  }
}
