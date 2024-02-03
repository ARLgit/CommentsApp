import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { CommonModule } from '@angular/common';
import { ChangeUserNamePartialComponent } from "./change-user-name-partial/change-user-name-partial.component";
import { ChangeEmailPartialComponent } from "./change-email-partial/change-email-partial.component";
import { ChangePasswordPartialComponent } from "./change-password-partial/change-password-partial.component";

@Component({
    selector: 'app-account-management',
    standalone: true,
    templateUrl: './account-management.component.html',
    styleUrl: './account-management.component.css',
    imports: [CommonModule, MaterialsModule, ChangeUserNamePartialComponent, ChangeEmailPartialComponent, ChangePasswordPartialComponent]
})
export class AccountManagementComponent implements OnInit{

  panelOpenState: boolean = false;

  constructor(
    private Router:Router
  ) 
  {
    
  }

  ngOnInit(): void 
  {

  }

}