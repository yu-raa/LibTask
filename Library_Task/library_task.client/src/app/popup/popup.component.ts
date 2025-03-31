import { Component, Inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from "@angular/material/dialog";
import { BookService } from "../books";

@Component({
  selector: 'app-popup',
  standalone: true,
  templateUrl: './popup.component.html'
})

export class PopupComponent {
  constructor(
    private dialogRef: MatDialogRef<PopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
  }

  confirm(ev: Event) {
    this.data.answerId = (ev.target as HTMLButtonElement).id
    this.dialogRef.close(this.data)
  }
}
