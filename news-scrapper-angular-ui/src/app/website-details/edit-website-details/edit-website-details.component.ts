import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { WebsiteDetails } from 'src/app/shared/website-details/website-details.model';
import { WebsiteDetailsService } from 'src/app/shared/website-details.service';
import { CategoryDropdown } from 'src/app/shared/categories/category-dropdown.model';
import { CategoryWebsiteDetails } from 'src/app/shared/categories/category-website-details.model';
import { CategoriesService } from 'src/app/shared/categories.service';
import { Category } from 'src/app/shared/categories/category.model';

@Component({
  selector: 'app-edit-website-details',
  templateUrl: './edit-website-details.component.html',
  styleUrls: ['./edit-website-details.component.scss'],
})
export class EditWebsiteDetailsComponent implements OnInit {
  constructor(
    private service: WebsiteDetailsService,
    private route: ActivatedRoute,
    private router: Router,
    private categoriesService: CategoriesService
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    if (this.id != null && this.id != 0) {
      this.getWebsiteDetailsById(this.id);
    }

    this.categoriesService.getAll().subscribe(
      (res) => {
        this.fulfillAvailableCategories(res);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }

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

  fulfillChosenCategories(categories: CategoryWebsiteDetails[]) {
    categories.forEach(cat => {
      this.chosenCategories.push()
    });
  }

  model: WebsiteDetails = new WebsiteDetails();
  errors: string[] = [];
  id: number = 0;
  availableCategories: CategoryDropdown[] = [];
  chosenCategories: CategoryWebsiteDetails[] = [];
  selectedCategory: CategoryWebsiteDetails = new CategoryWebsiteDetails(0, '');

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
          this.chosenCategories.push(...res.categories);
        },
        (err) => {
          this.errors.push(err.error.message);
        }
      );
  }
}
