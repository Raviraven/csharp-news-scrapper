import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Article } from './article.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ArticlesService {
  constructor(private http: HttpClient) {}

  readonly baseUrl = environment.apiUrl + 'articles/';

  getAllArticles() {
    return this.http.get<Article[]>(this.baseUrl);
  }

  getArticleById(id: number) {
    return this.http.get<Article>(this.baseUrl + id);
  }

  getNewArticles() {
    return this.http.get<Article[]>(this.baseUrl + 'new');
  }

  scrapArticles() {
    return this.http.get<string[]>(this.baseUrl + 'scrap');
  }
}
