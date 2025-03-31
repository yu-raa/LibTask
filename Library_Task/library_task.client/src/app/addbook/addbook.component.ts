import { NgFor, NgForOf, NgIf } from "@angular/common";
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ActivatedRoute, Params, Router, RouterOutlet } from "@angular/router";
import { Author, Book, DataService, PageQuery, RespClass, User } from "../data";
import { Component, OnInit } from "@angular/core";
import { BookService } from "../books";
import { AuthorService } from "../authors";
import { AppComponent } from "../app.component";

@Component({
  selector: 'app-addbook',
  standalone: true,
  imports: [ReactiveFormsModule, NgForOf, NgIf, RouterOutlet],
  templateUrl: './addbook.component.html',
  styleUrl: './addbook.component.css',
  providers: [DataService, RouterOutlet, BookService, AuthorService]
})

export class AddBookComponent implements OnInit {
  book!: Book
  bookUrl!: string
  page!: number
  pageSize!: number
  authors?: Author[]
  isLoggedIn: boolean

  constructor(private as: AuthorService, private ds: DataService, private bs: BookService, private rout: Router, private route: ActivatedRoute) {
    this.isLoggedIn = AppComponent.loggedIn
  }

  async ngOnInit(): Promise<void> {
    await this.onIn()
  }

  async onIn() {
    this.authors = (await this.as.getAuthors(this.ds.getData())).key
    let data: any
    await this.route.queryParams.forEach(res => {
      if (res instanceof PageQuery) {
        let res2 = res as PageQuery
        this.page = res2.page;
        this.pageSize = res2.pageSize;
      }
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
      reader.onload = () => this.bookUrl = reader.result as string
    }
  }

  async create(ev: Event) {
    let authorSelect = document.getElementById('auth') as HTMLSelectElement
    if ((ev.target as HTMLButtonElement).innerText == 'Add New') {
      this.rout.navigate(['addAuthor'], { queryParams: new PageQuery(this.page, this.pageSize) })
    }
    else if ((ev.target as HTMLButtonElement).innerText == 'Create') {
      let count: number = 0
      let res = await this.bs.getBooks(this.ds.getData(), 2000000000, 1, null, null, null)
      count = Number.parseInt(res.key[res.key.length - 1].id) + 1
      let logindata = this.ds.getData() as RespClass
      this.book = new Book((count).toString(), (document.getElementById('isbn') as HTMLInputElement).value as string, (document.getElementById('genre') as HTMLInputElement).value as string, (document.getElementById('title') as HTMLInputElement).value as string, (document.getElementById('descr') as HTMLInputElement).value as string, this.bookUrl, new Date(), new Date(), this.authors?.at(authorSelect.selectedIndex)?.id as string, '1')
      this.bs.addBook(this.book, this.ds.getData() as RespClass, this.pageSize, this.page)
      this.ds.savePicCache([this.book])
      this.rout.navigate([`main`], { queryParams: new PageQuery(1, 20) })
    }
  }
}
