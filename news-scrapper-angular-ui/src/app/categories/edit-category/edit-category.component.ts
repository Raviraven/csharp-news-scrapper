import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriesService } from 'src/app/shared/categories.service';
import { CategoryEdit } from 'src/app/shared/categories/category-edit.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.scss'],
})
export class EditCategoryComponent implements OnInit {
  constructor(
    private service: CategoriesService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    if (this.id != null && this.id != 0) {
      this.getWebsiteDetailsById(this.id);
    }
  }

  model: CategoryEdit = new CategoryEdit();
  errors: string[] = [];
  id: number = 0;

  onSubmit() {
    this.errors = [];
    this.service.save(this.model).subscribe(
      (res) => {
        this.router.navigate(['/categories']);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }

  getWebsiteDetailsById(id: number) {
    this.service
      .get(id)
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
