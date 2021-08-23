import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Article } from './article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  constructor(private http: HttpClient) { }

  readonly baseUrl ='http://localhost/news-scrapper.api/articles/';

  getAllArticles(){
    return this.http.get<Article[]>(this.baseUrl);
  }

  getArticleById(id: number){
    return this.http.get<Article>(this.baseUrl + id);
  }

  getNewArticles(){
    return this.http.get<Article[]>(this.baseUrl + 'new');
  }

  scrapArticles(){
    return this.http.get<string[]>(this.baseUrl + 'scrap');
  }
}
