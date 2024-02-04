import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ISession } from '../../../../Interfaces/Auth/session';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-delete-account-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './delete-account-partial.component.html',
  styleUrl: './delete-account-partial.component.css'
})
export class DeleteAccountPartialComponent implements OnInit{

  deleteForm:FormGroup;
  HidePassword = true;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    this.deleteForm = this.FormBuilder.group({
      password:["", Validators.required]
    });
  }

  ngOnInit(): void 
  {

  }

  deleteAccount():void 
  {
    const user: ISession | null = this.Auth.getSession();
    if (user)
    {

      Swal.fire({
        title: "Esta seguro que desea borrar su cuenta?",
        text: "Una vez borrado no podra ser recuperada.",
        icon: "warning",
        showCancelButton: true,
        cancelButtonAriaLabel: 'Cancelar',
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, Borrala!"
      }).then((result) => {
        if (result.isConfirmed) {
          this.delete(this.deleteForm.value.password);
        }
      });

    }
    else
    {

    }

  }

  delete(password:string):void 
  {
    const user: ISession | null = this.Auth.getSession();
    if (user)
    {

      this.Auth.deleteUser(password).subscribe(
        {
          next: response => {
            console.log(response);
            if (response.status) {
              setTimeout(() => {
                this.Utilities.Alert(response.message, "Ok");
              }, 1000)
            }
          },
          error: err => {
            console.log(err);
            this.Utilities.Alert(err.error.message,"Opps!")
          },
          complete: () => {
            this.Auth.logOut();
            this.Router.navigate(['threads'])
          }
        }
      );

    }
    else
    {
      this.Utilities.Alert('Inicie sesion para borrar su cuenta.',"Opps!")
    }

  }

}