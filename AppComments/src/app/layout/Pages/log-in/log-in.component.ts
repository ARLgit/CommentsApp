import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ILogIn } from '../../../Interfaces/Auth/log-in'; 
import { AuthService } from '../../../Services/auth.service';
import { UtilitiesService } from '../../../Services/utilities.service';
import { response } from 'express';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';

@Component({
  selector: 'app-log-in',
  standalone: true,
  imports: [FormsModule, MaterialsModule],
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
    let response = this.Auth.logIn(this.LogInForm.value.userName, this.LogInForm.value.password);
    this.Utilities.Alert(response, "LogIn");
  }

}
