import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar'
import { ILogIn } from '../Interfaces/Auth/log-in';
import { AuthService } from './auth.service';
import { CookieService } from 'ngx-cookie-service';
import { response } from 'express';

@Injectable({
  providedIn: 'root'
})
export class UtilitiesService {

  constructor(private snackBar:MatSnackBar, private cookies:CookieService, private auth:AuthService) { }

  Alert(message:string, type:string){
    this.snackBar.open(message, type, {
      horizontalPosition:"center",
      verticalPosition:"top",
      duration:3000
    });
  }
  
  logIn(username:string, password:string) {
    const user: ILogIn = {
      userName: username,
      password: password
    };
    return this.auth.logIn(user).subscribe(
      (response) => {
        if (!this.cookies.check("token") && response.Status == true)
        {
          this.cookies.set("token", String(response.Value));
          return true;
        }
        return false;
      }
    );
  }

  logOut() {
    if (this.cookies.check("token"))
    {
      this.cookies.delete("token");
      return true;
    }
    return false;
  }
}
