import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ISession } from '../../../../Interfaces/Auth/session';
import { IChangePassword } from '../../../../Interfaces/Auth/change-password';

@Component({
  selector: 'app-change-password-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './change-password-partial.component.html',
  styleUrl: './change-password-partial.component.css'
})
export class ChangePasswordPartialComponent implements OnInit{

  passwordChangeForm:FormGroup;
  HidePassword1 = true;
  HidePassword2 = true;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    this.passwordChangeForm = this.FormBuilder.group({
      oldPassword:["", Validators.required],
      newPassword:["", Validators.required]
    });
  }

  ngOnInit(): void 
  {
    
  }

  changePassword(): void
  {

    this.ShowLoading = true;

    const user: ISession | null = this.Auth.getSession();
    if (user)
    {

      const request: IChangePassword = {
        oldPassword: this.passwordChangeForm.value.oldPassword,
        newPassword: this.passwordChangeForm.value.newPassword
      }
      
      this.Auth.changePassword(request).subscribe(
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