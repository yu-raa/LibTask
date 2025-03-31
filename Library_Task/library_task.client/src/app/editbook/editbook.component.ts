import { NgForOf, NgIf } from "@angular/common"
import { ChangeDetectorRef, Component, OnInit } from "@angular/core"
import { ReactiveFormsModule } from "@angular/forms"
import { ActivatedRoute, Router, RouterOutlet, UrlCreationOptions } from "@angular/router"
import { AuthorService } from "../authors"
import { BookService } from "../books"
import { DataService, Book, Author, BookToShow, PageQuery, RespClass, BookInfo } from "../data"
import { MatDialog } from "@angular/material/dialog"
import { AppComponent } from "../app.component"

@Component({
  selector: 'app-editbook',
  standalone: true,
  imports: [ReactiveFormsModule, NgForOf, NgIf, RouterOutlet],
  templateUrl: './editbook.component.html',
  styleUrl: './editbook.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class BookEditComponent implements OnInit {
  books!: BookToShow[]
  authors!: Author[]
  book!: Book
  role!: string
  isLoaded: boolean = false
  answerId!: string
  isLoggedIn: boolean

  constructor(private ds: DataService, private bs: BookService, private as: AuthorService, private rout: Router, private cd: ChangeDetectorRef, private route: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngOnInit(): Promise<void> {
    await this.onIn()
  }

  async onIn() {
    this.authors = (await this.as.getAuthors(this.ds.getData())).key
    let data: any
    await this.route.queryParams.forEach(res => {
      let res2 = JSON.parse(res['info'] as unknown as string) as BookInfo
        this.book = res2.bookInfo as BookToShow as Book
    }).catch(reas => data = reas)
  }

  async blobToDataUri(blob: Blob) {
    let arrayBuffer = await blob.arrayBuffer()
    let int8arr = new Uint8Array(arrayBuffer)
    let uri = ''
    for (let i = 0; i < int8arr.length; i++) {
      uri += int8arr[i] as unknown as symbol as unknown as string
    }
    return window.btoa(uri)
  }

  onFileSelected(ev: Event) {
    let inp = ev.target as HTMLInputElement
    if (inp != null && inp.files != null) {
      let file = inp.files[0]
      let pattern = /image-*/;
      if (!file.type.match(pattern)) {
        alert('invalid format');
        return;
      }
      let reader = new FileReader()
      reader.readAsDataURL(file)
      reader.onload = () => this.book.bookImage = reader.result as string
    }
  }

  async save(ev: Event) {
    let authorSelect = document.getElementById('auth') as HTMLSelectElement
    if ((ev.target as HTMLButtonElement).innerText == 'Add New') {
      this.rout.navigate(['addAuthor'], { queryParams: new PageQuery(1, 20) })
    }
    else if ((ev.target as HTMLButtonElement).innerText == 'Save') {
      let count: string
      let res = await this.bs.getBooks(this.ds.getData(), 2000000000, 1, null, null, null)
      count = this.book.id
      let logindata = this.ds.getData() as RespClass
      this.book = new Book(count, (document.getElementById('isbn') as HTMLInputElement).value.length == 0 ? (document.getElementById('isbn') as HTMLInputElement).placeholder : (document.getElementById('isbn') as HTMLInputElement).value, (document.getElementById('genre') as HTMLInputElement).value.length == 0 ? (document.getElementById('genre') as HTMLInputElement).placeholder : (document.getElementById('genre') as HTMLInputElement).value, (document.getElementById('title') as HTMLInputElement).value.length == 0 ? (document.getElementById('title') as HTMLInputElement).placeholder : (document.getElementById('title') as HTMLInputElement).value, (document.getElementById('descr') as HTMLInputElement).value.length == 0 ? (document.getElementById('descr') as HTMLInputElement).placeholder : (document.getElementById('descr') as HTMLInputElement).value, this.book.bookImage, new Date(), new Date(), this.authors?.at(authorSelect.selectedIndex)?.id as string, this.book.userId)
      this.bs.editBook(this.book, this.ds.getData() as RespClass)
      this.rout.navigate([`main`], { queryParams: new PageQuery(1, 20) })
    }
  }
}
