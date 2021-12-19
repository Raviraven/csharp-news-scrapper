import { Component, OnInit } from '@angular/core';
import { WebsiteDetails } from 'src/app/shared/website-details/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';

@Component({
  selector: 'app-website-details-list',
  templateUrl: './website-details-list.component.html',
  styleUrls: ['./website-details-list.component.scss'],
})
export class WebsiteDetailsListComponent implements OnInit {
  constructor(private service: WebsiteDetailsService) {}

  websitesDetails: WebsiteDetails[] = [];
  errors: string[] = [];

  ngOnInit(): void {
    this.service.getAllWebsiteDetails().subscribe((res) => {
      this.websitesDetails = res;
    });
  }

  onRemove(id: number){
    this.service.deleteWebsiteDetails(id).subscribe(
      res => {
        var index = this.websitesDetails.findIndex(n=>n.id == id);
        this.websitesDetails.splice(index, 1);
      },
      err => {
        this.errors.push(err.error.message);
      }
    );
  }
}
