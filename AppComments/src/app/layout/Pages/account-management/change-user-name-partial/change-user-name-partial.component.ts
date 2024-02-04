import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ISession } from '../../../../Interfaces/Auth/session';
import { IUpdateUser } from '../../../../Interfaces/Auth/update-user';

@Component({
  selector: 'app-change-user-name-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './change-user-name-partial.component.html',
  styleUrl: './change-user-name-partial.component.css'
})
export class ChangeUserNamePartialComponent implements OnInit{

  UserNameUpdateForm:FormGroup;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    this.UserNameUpdateForm = this.FormBuilder.group({
      newUserName:["", Validators.required]
    });
  }

  ngOnInit(): void 
  {

  }

  changeUserName():void 
  {

    this.ShowLoading = true;

    const user: ISession | null = this.Auth.getSession();
    if (user)
    {

      const request: IUpdateUser = {
        userName: this.UserNameUpdateForm.value.newUserName,
        email: user.email
      }
      
      this.Auth.updateUser(request).subscribe(
        {
          next: response => {
            if (response.status) {
              setTimeout(() => {
                this.Utilities.Alert(response.message,"Ok")
              }, 1000)
            }
          },
          error: err => {
            this.ShowLoading = false;
            this.Utilities.Alert(err.error.message,"Opps!")
          },
          complete: () => { 
            this.Router.navigate(["account"]);
          }
        }
      );

    }
    else
    {
      this.Utilities.Alert('Debe iniciar sesion para cambiar su usuario',"Opps!")
    }

  }

}
