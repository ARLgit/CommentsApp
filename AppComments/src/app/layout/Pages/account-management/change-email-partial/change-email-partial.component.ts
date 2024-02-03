import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-email-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './change-email-partial.component.html',
  styleUrl: './change-email-partial.component.css'
})
export class ChangeEmailPartialComponent  implements OnInit{

  constructor(
    private FormBuilder:FormBuilder,
    private Router:Router,
    private Auth:AuthService,
    private Utilities:UtilitiesService
  ) 
  {
    
  }

  ngOnInit(): void 
  {

  }

}