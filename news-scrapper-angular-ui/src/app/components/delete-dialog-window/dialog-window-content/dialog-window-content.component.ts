import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogContentData } from '../dialog-window.component';

@Component({
  selector: 'app-dialog-window-content',
  templateUrl: './dialog-window-content.component.html',
  styleUrls: ['./dialog-window-content.component.scss'],
})
export class DialogWindowContentComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public dialogContentData: DialogContentData
  ) {
    console.log(dialogContentData);
  }

  ngOnInit(): void {}
}
