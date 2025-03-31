import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignUpComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { MainComponent } from './main/main.component';
import { AddBookComponent } from './addbook/addbook.component';
import { AddAuthorComponent } from './addauthor/addauthor.component';
import { BookDetailComponent } from './bookdetail/bookdetail.component';
import { BookEditComponent } from './editbook/editbook.component';
import { OwnedBooksComponent } from './mybooks/mybooks.component';

export const routes: Routes = [
  { path: "register", component: SignUpComponent },
  { path: "login", component: LoginComponent },
  { path: "main", component: MainComponent },
  { path: "addBook", component: AddBookComponent },
  { path: "addAuthor", component: AddAuthorComponent },
  { path: "bookDetails", component: BookDetailComponent },
  { path: "editBook", component: BookEditComponent },
  { path: "myBooks", component: OwnedBooksComponent }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)]
})
export class AppRoutingModule { }
