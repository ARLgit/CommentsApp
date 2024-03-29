import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IRegisterUser } from '../../../Interfaces/Auth/register-user';
import { AuthService } from '../../../Services/auth.service';
import { UtilitiesService } from '../../../Services/utilities.service';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sing-up',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './sing-up.component.html',
  styleUrl: './sing-up.component.css'
})
export class SingUpComponent implements OnInit {
  
  RegistrationForm:FormGroup;
  HidePassword:boolean = true;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    this.RegistrationForm = this.FormBuilder.group({
      userName:["", Validators.required],
      password:["", Validators.required],
      email:["", Validators.required]
    });
  }

  ngOnInit() 
  {
    
  }

  registerUser()
  {
    this.ShowLoading = true;
    
    const request:IRegisterUser = {
      userName: this.RegistrationForm.value.userName,
      password: this.RegistrationForm.value.password,
      email: this.RegistrationForm.value.email
    };

    this.Auth.register(request).subscribe(
      {
        next: response => {
          if (response.status) {
            setTimeout(() => {
              this.Utilities.Alert(response.message, "Ok");
            }, 1000)
          }
        },
        error: err => {
          this.ShowLoading = false;
          this.Utilities.Alert(err.error.message,"Opps!")
        },
        complete: () => { 
          this.ShowLoading = false;
          this.Router.navigate(["threads"]);
        }
      }
    );
  }

}
