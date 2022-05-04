import { Component, Input, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ProgressSpinnerMode } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss'],
})
export class LoadingComponent implements OnInit {
  @Input() dataLoaded: boolean = false;
  public color: ThemePalette = 'primary';
  public mode: ProgressSpinnerMode = 'indeterminate';

  constructor() {
    console.log(this.dataLoaded);
  }

  ngOnInit(): void {}
}
