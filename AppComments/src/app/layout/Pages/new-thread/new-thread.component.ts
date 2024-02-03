import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../Services/auth.service';
import { UtilitiesService } from '../../../Services/utilities.service';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ThreadsService } from '../../../Services/threads.service';
import { IPostThread } from '../../../Interfaces/Threads/post-thread'

@Component({
  selector: 'app-new-thread',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './new-thread.component.html',
  styleUrl: './new-thread.component.css'
})
export class NewThreadComponent implements OnInit{

  NewThreadForm:FormGroup;
  ShowLoading:boolean = false;

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Threads:ThreadsService,
    private Utilities:UtilitiesService
  ) 
  {
    this.NewThreadForm = this.FormBuilder.group({
      titulo:["", Validators.required],
      contenido:["", Validators.required]
    });
  }

  ngOnInit(): void 
  {

  }

  createThread():void {

    this.ShowLoading = true;

    const user = this.Auth.getSession()

    if (user)
    {

      const request:IPostThread = {
        creatorId: Number(user.sid),
        title: this.NewThreadForm.value.titulo,
        content: this.NewThreadForm.value.contenido
      };
  
      this.Threads.postThread(request).subscribe(
        {
          next: response => {
            if (response.status) {
              setTimeout(() => {
                this.Router.navigate(["threads"]);
              }, 2000)
              this.Utilities.Alert(response.message, "Ok");
              this.ShowLoading = false;
            }
          },
          error: err => {
            this.ShowLoading = false;
            this.Utilities.Alert(err.error.message,"Opps!")
          },
          complete: () => { 
          }
        }
      );

    }
    else
    {
      this.ShowLoading = false;
      this.Utilities.Alert('Debe iniciar sesion para crear un hilo.',"Opps!")
    }

  }

}
