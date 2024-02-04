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
  selector: 'app-change-email-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './change-email-partial.component.html',
  styleUrl: './change-email-partial.component.css'
})
export class ChangeEmailPartialComponent  implements OnInit{

  EmailUpdateForm:FormGroup;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    this.EmailUpdateForm = this.FormBuilder.group({
      newEmail:["", Validators.required]
    });
  }

  ngOnInit(): void 
  {

  }

  changeEmail():void 
  {

    this.ShowLoading = true;

    const user: ISession | null = this.Auth.getSession();
    if (user)
    {

      const request: IUpdateUser = {
        userName: user.name,
        email: this.EmailUpdateForm.value.newEmail
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
      this.Utilities.Alert('Debe iniciar sesion para cambiar su email',"Opps!")
    }

  }

}