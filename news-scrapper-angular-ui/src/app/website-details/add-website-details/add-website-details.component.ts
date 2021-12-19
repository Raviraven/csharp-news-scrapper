import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebsiteDetails } from 'src/app/shared/website-details/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';

@Component({
  selector: 'app-add-website-details',
  templateUrl: './add-website-details.component.html',
  styleUrls: ['./add-website-details.component.scss'],
})
export class AddWebsiteDetailsComponent implements OnInit {
  constructor(private service: WebsiteDetailsService, private router: Router) {}

  ngOnInit(): void {}

  model: WebsiteDetails = new WebsiteDetails();
  errors: string[] = [];
  id: number = 0;

  onSubmit() {
    this.errors = [];

    this.service.addWebsiteDetails(this.model).subscribe(
      (res) => {
        this.router.navigate(['/website-details']);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }
}
