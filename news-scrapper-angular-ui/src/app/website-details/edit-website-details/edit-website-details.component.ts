import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WebsiteDetails } from 'src/app/shared/website-details/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';

@Component({
  selector: 'app-edit-website-details',
  templateUrl: './edit-website-details.component.html',
  styleUrls: ['./edit-website-details.component.scss'],
})
export class EditWebsiteDetailsComponent implements OnInit {
  constructor(
    private service: WebsiteDetailsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    if (this.id != null && this.id != 0) {
      this.getWebsiteDetailsById(this.id);
    }
  }

  model: WebsiteDetails = new WebsiteDetails();
  errors: string[] = [];
  id: number = 0;

  onSubmit() {
    this.errors = [];
    this.service.saveWebsiteDetails(this.model).subscribe(
      (res) => {
        this.router.navigate(['/website-details']);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }

  getWebsiteDetailsById(id: number) {
    this.service
      .getWebsiteDetails(id)
      .toPromise()
      .then(
        (res) => {
          this.model = res;
        },
        (err) => {
          this.errors.push(err.error.message);
        }
      );
  }
}
