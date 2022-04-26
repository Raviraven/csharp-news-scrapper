import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogWindowContentComponent } from './dialog-window-content/dialog-window-content.component';

export interface DialogContentData {
  id: number;
  name: string;
}

@Component({
  selector: 'app-dialog-window',
  templateUrl: './dialog-window.component.html',
  styleUrls: ['./dialog-window.component.scss'],
})
export class DialogWindowComponent implements OnInit {
  @Input() onSuccessAction: (id: number) => void = (id) => {};
  @Input() id: number = 0;
  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {}

  openDialog() {
    const dialogRef = this.dialog.open(DialogWindowContentComponent, {
      data: {
        id: this.id,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === true && this.onSuccessAction) {
        this.onSuccessAction(this.id);
      }
    });
  }
}
