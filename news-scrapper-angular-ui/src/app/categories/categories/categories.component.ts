import { Component, OnInit } from '@angular/core';
import { CategoriesService } from 'src/app/shared/categories.service';
import { Category } from 'src/app/shared/categories/category.model';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
})
export class CategoriesComponent implements OnInit {
  constructor(private service: CategoriesService) {}

  ngOnInit(): void {
    this.service.getAll().subscribe(
      (res) => {
        this.categories = res;
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }

  categories: Category[] = [];
  errors: string[] = [];

  onRemove(id: number) {
    this.service.delete(id).subscribe(
      (res) => {
        var index = this.categories.findIndex((n) => n.id == id);
        this.categories.splice(index, 1);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }
}
