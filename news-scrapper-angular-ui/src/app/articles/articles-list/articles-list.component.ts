import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Article } from 'src/app/shared/article.model';
import { ArticlesService } from 'src/app/shared/articles.service';

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss'],
})
export class ArticlesListComponent implements OnInit {
  constructor(private service: ArticlesService) {}

  ngOnInit(): void {
    this.getAllArticles();
  }

  articles: Article[] = [];
  articlesOnPage: Article[] = [];

  dataLoaded: boolean = false;
  errors: string[] = [];

  onChangePage(articlesOnPage: Article[]) {
    this.articlesOnPage = articlesOnPage;
  }

  getAllArticles() {
    this.articles = [];

    this.service
      .getAllArticles()
      .toPromise()
      .then(
        (res) => {
          this.articles = res;
        },
        (err) => {
          console.log(err);
          //this.errors.push(err.error.message);
          this.errors.push((err as HttpErrorResponse).error);
        }
      )
      .finally(() => {
        this.dataLoaded = true;
      });
  }

  scrapArticles() {
    this.service
      .scrapArticles()
      .toPromise()
      .then(
        (res) => {
          if (res.length > 1) {
            this.errors = res;
          } else {
            console.log(res);
          }
        },
        (err) => {
          this.errors.push(err.error.message);
        }
      )
      .finally(() => {
        this.getAllArticles();
      });
  }
}
