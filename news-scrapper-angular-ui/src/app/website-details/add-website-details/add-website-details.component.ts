import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { WebsiteDetails } from 'src/app/shared/website-details/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';
import { Category } from 'src/app/shared/categories/category.model';
import { CategoriesService } from 'src/app/shared/categories.service';
import { CategoryWebsiteDetails } from 'src/app/shared/categories/category-website-details.model';
import { CategoryDropdown } from 'src/app/shared/categories/category-dropdown.model';

@Component({
  selector: 'app-add-website-details',
  templateUrl: './add-website-details.component.html',
  styleUrls: ['./add-website-details.component.scss'],
})
export class AddWebsiteDetailsComponent implements OnInit {
  constructor(
    private service: WebsiteDetailsService,
    private router: Router,
    private categoriesService: CategoriesService
  ) {}

  ngOnInit(): void {
    this.categoriesService.getAll().subscribe(
      (res) => {
        this.fulfillAvailableCategories(res);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }

  model: WebsiteDetails = new WebsiteDetails();
  errors: string[] = [];
  availableCategories: CategoryDropdown[] = [];
  chosenCategories: CategoryWebsiteDetails[] = [];
  selectedCategory: CategoryWebsiteDetails = new CategoryWebsiteDetails(0, '');

  fulfillAvailableCategories(res: Category[]) {
    for (let index = 0; index < res.length; index++) {
      const element = res[index];
      let categoryTemp = new CategoryDropdown();
      categoryTemp.id = element.id;
      categoryTemp.name = element.name;
      //categoryTemp.visible = true;

      this.availableCategories.push(categoryTemp);
    }
  }

  onCategoryAdd() {
    console.log(this.selectedCategory);
    this.chosenCategories.push(this.selectedCategory);
  }

  onCategoryRemove(id: number) {
    var index = this.chosenCategories.findIndex((n) => n.id == id);
    var deleted = this.chosenCategories.splice(index, 1);
  }

  onSubmit() {
    this.errors = [];
    this.model.categories = this.chosenCategories;
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
