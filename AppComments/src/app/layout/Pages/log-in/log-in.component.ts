import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../Services/auth.service';
import { UtilitiesService } from '../../../Services/utilities.service';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ILogIn } from '../../../Interfaces/Auth/log-in';
import { CookieService } from 'ngx-cookie-service';
import { HttpErrorResponse } from '@angular/common/http';
import { ResponseApi } from '../../../Interfaces/response-api';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MaterialsModule],
  templateUrl: './log-in.component.html',
  styleUrl: './log-in.component.css'
})
export class LogInComponent implements OnInit {

  LogInForm:FormGroup;
  HidePassword:boolean = true;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private cookies:CookieService,
    private Utilities:UtilitiesService
  ) 
  {
    this.LogInForm = this.FormBuilder.group({
      userName:["", Validators.required],
      password:["", Validators.required]
    });
  }

  ngOnInit() 
  {
    
  }

  logIn()
  {
    const user: ILogIn = {
      userName: this.LogInForm.value.userName,
      password: this.LogInForm.value.password
    };

    let result:string;

    this.ShowLoading = true;

    this.Auth.logInRequest(user).subscribe(
      {
        next: (response:ResponseApi) => {
          if (!this.cookies.check("token") && response.status == true)
          {
            this.cookies.set("token", String(response.value));
          }
          result = response.message;
        },
        error: (err:HttpErrorResponse) => {
          result = err.error.message
          this.Utilities.Alert(result, "Ok");
        },
        complete: () => {
          this.Utilities.Alert(result, "Ok");
          this.ShowLoading = false;
        }
      }
    );

  }

}
