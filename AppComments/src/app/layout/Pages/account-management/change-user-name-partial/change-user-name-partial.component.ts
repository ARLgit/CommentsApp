import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule , Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-change-user-name-partial',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule , MaterialsModule],
  templateUrl: './change-user-name-partial.component.html',
  styleUrl: './change-user-name-partial.component.css'
})
export class ChangeUserNamePartialComponent implements OnInit{

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
