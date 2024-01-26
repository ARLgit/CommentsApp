import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';
import { ResponseApi } from '../Interfaces/response-api';
import { IChangePassword } from '../Interfaces/Auth/change-password';
import { ILogIn } from '../Interfaces/Auth/log-in';
import { IRegisterUser } from '../Interfaces/Auth/register-user';
import { IUpdateUser } from '../Interfaces/Auth/update-user';
import { jwtDecode } from "jwt-decode";
import { ISession } from '../Interfaces/Auth/session';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl:string = environment.apiUrl + "Auth/"

  constructor(private http:HttpClient, private cookies:CookieService) { }

  register(request:IRegisterUser):Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiUrl}Register`, request)
  }

  logIn(request:ILogIn):Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiUrl}LogIn`, request)
  }

  createSession(token:string):boolean {
    if (this.cookies.check("token") || this.cookies.check("session"))
    {
      return false;
    }
    let claims:any = JSON.stringify(jwtDecode(token));
    this.cookies.set("session", claims)
    this.cookies.set("token", token);
    return true;
  }

  getSession():ISession | null {
    if (this.cookies.check("session"))
    {
      let claims = JSON.parse(this.cookies.get("session"))
      let session:ISession = {
        aud: claims["aud"],
        exp: String(new Date(claims["exp"] * 1000)),
        role: claims["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
        name: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
        sid: claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid"],
        iss: claims["iss"]
      }
      return session;
    }
    return null;
  }

  logOut(): boolean {
    if (this.cookies.check("token"))
    {
      this.cookies.delete("token");
      this.cookies.delete("session")
      return true;
    }
    return false;
  }

  updateUser(request:IUpdateUser):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}UpdateUser`, request)
  }

  changePassword(request:IChangePassword):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}ChangePassword`, request)
  }

  deleteUser(password:string):Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.apiUrl}DeleteUser`, {body: {"password": password}}) 
  }
}
