import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MaterialsModule } from '../MaterialsModule/materials/materials.module';
import { LogInComponent } from "./Pages/log-in/log-in.component";
import { CookieService } from 'ngx-cookie-service';

@Component({
    selector: 'app-layout',
    standalone: true,
    templateUrl: './layout.component.html',
    styleUrl: './layout.component.css',
    imports: [CommonModule, MaterialsModule, RouterOutlet, RouterLink, RouterLinkActive, FormsModule, LogInComponent]
})
export class LayoutComponent implements OnInit {
  
  loggedIn:boolean = this.checkLogInStatus();

  constructor(
    private cookies:CookieService,
    private router:Router
    ) 
    { 

    }

  ngOnInit() 
  {

  }

  checkLogInStatus():boolean
  {
    return (this.cookies.check("token") && this.cookies.check("session")) ? true : false;
  }

  logOut():void {
    if (this.checkLogInStatus())
    {
      this.loggedIn = false;
      this.cookies.delete("token");
      this.cookies.delete("session");
      this.router.navigate([this.router.url, this.checkLogInStatus]);
    }
  }

}
