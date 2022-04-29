import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DialogWindowContentComponent } from './dialog-window-content/dialog-window-content.component';

export interface DialogContentData {
  id: number;
  objectName: string;
}

@Component({
  selector: 'app-dialog-window',
  templateUrl: './dialog-window.component.html',
  styleUrls: ['./dialog-window.component.scss'],
})
export class DialogWindowComponent implements OnInit {
  @Input() dialogContentData: DialogContentData = {
    id: 0,
    objectName: '',
  };
  @Output() removeItemEvent = new EventEmitter<number>();

  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {}

  openDialog() {
    const dialogRef = this.dialog.open(DialogWindowContentComponent, {
      data: {
        ...this.dialogContentData,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result === true) {
        this.removeItemEvent.emit(this.dialogContentData.id);
      }
    });
  }
}
