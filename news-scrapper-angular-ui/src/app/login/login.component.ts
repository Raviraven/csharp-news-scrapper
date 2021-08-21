import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { UsersService } from '../shared/users.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(public service: UsersService) { }

  ngOnInit(): void {
  }

  errors: string[] =[];
  
  private clearErrors(){
    this.errors = [];
  }

  onTestClick(){
    this.clearErrors();
    this.service.getUsers().toPromise().then(
        res => { console.log(res); },
        err => { this.errors.push(err.error.message); }
    );
  }

  onSubmit(form: NgForm){
    this.clearErrors();
    this.service.postLoginDetails().subscribe(
      res => { this.service.token = res.jwtToken; },
      err => { this.errors.push(err.error.message); }
    );
  }
}
