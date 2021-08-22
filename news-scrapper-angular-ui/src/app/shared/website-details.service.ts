import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { WebsiteDetails } from './website-details.model';

@Injectable({
  providedIn: 'root'
})
export class WebsiteDetailsService {

  constructor(private http: HttpClient) { }

  readonly baseUrl = 'http://localhost/news-scrapper.api/websitedetails/';

  
  getAllWebsiteDetails(){
    return this.http.get<WebsiteDetails[]>(this.baseUrl);
  }

  addWebsiteDetails(model: WebsiteDetails){
    return this.http.post<WebsiteDetails>(this.baseUrl, model);
  }

  saveWebsiteDetails(model: WebsiteDetails){
    return this.http.put(this.baseUrl, model);
  }

  getWebsiteDetails(id: number){
    return this.http.get<WebsiteDetails>(this.baseUrl + id);
  }

  deleteWebsiteDetails(id: number){
    return this.http.delete(this.baseUrl + id);
  }

}
