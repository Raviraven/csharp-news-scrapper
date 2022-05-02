import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Article } from 'src/app/shared/article.model';
import { ArticlesService } from 'src/app/shared/articles.service';

@Component({
  selector: 'app-articles-new',
  templateUrl: './articles-new.component.html',
  styleUrls: ['./articles-new.component.scss'],
})
export class ArticlesNewComponent implements OnInit {
  constructor(private service: ArticlesService) {}

  ngOnInit(): void {
    this.getNewArticles();
  }

  articles: Article[] = [];
  articlesPerPage: Article[] = [];
  errors: string[] = [];
  public LastScrapDate: string | undefined = undefined;
  public dataLoaded: boolean = false;

  onChangePage(articlesPerPage: Article[]) {
    this.articlesPerPage = articlesPerPage;
  }

  getNewArticles() {
    this.articles = [];

    this.service
      .getNewArticles()
      .toPromise()
      .then(
        (res) => {
          this.articles = res;
        },
        (err) => {
          this.errors.push((err as HttpErrorResponse).error);
        }
      )
      .finally(() => {
        this.dataLoaded = true;
        this.setDate();
      });
  }

  setDate() {
    if (this.articles.length > 0) {
      const date: Date = new Date(this.articles[0].dateScrapped);
      this.LastScrapDate = date.toLocaleDateString();
    }
  }
}
