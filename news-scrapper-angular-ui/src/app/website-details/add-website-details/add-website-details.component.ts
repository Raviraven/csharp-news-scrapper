import { Component, OnInit } from '@angular/core';
import { WebsiteDetails } from 'src/app/shared/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';

@Component({
  selector: 'app-add-website-details',
  templateUrl: './add-website-details.component.html',
  styleUrls: ['./add-website-details.component.scss'],
})
export class AddWebsiteDetailsComponent implements OnInit {
  constructor(private service: WebsiteDetailsService) {}

  ngOnInit(): void {}

  model: WebsiteDetails = new WebsiteDetails();
  errors: string[] = [];

  onSubmit() {
    this.errors = [];

    this.service.addWebsiteDetails(this.model).subscribe(
      (res) => {},
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }
}
