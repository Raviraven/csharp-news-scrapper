import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CategoriesService } from 'src/app/shared/categories.service';
import { CategoryAdd } from 'src/app/shared/categories/category-add.model';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent implements OnInit {

  constructor(private service: CategoriesService,
    private router: Router) { }

  ngOnInit(): void {
  }
  
  model: CategoryAdd = new CategoryAdd();
  errors: string[] = [];
  //id: number = 0;

  onSubmit() {
    this.errors = [];

    this.service.add(this.model).subscribe(
      (res) => {
        this.router.navigate(['/categories']);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }
}
