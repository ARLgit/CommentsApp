import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service';
import { ResponseApi } from '../Interfaces/response-api';
import { IChangePassword } from '../Interfaces/Auth/change-password';
import { ILogIn } from '../Interfaces/Auth/log-in';
import { IRegisterUser } from '../Interfaces/Auth/register-user';
import { IUpdateUser } from '../Interfaces/Auth/update-user';
import { stringify } from 'node:querystring';
import { response } from 'express';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl:string = environment.apiUrl + "Auth/"

  constructor(private http:HttpClient, private cookies:CookieService) { }

  register(request:IRegisterUser):Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiUrl}Register`, request)
  }

  logInRequest(request:ILogIn):Observable<ResponseApi> {
    let headers = new HttpHeaders();
    return this.http.post<ResponseApi>(`${this.apiUrl}LogIn`, request, {headers: headers})
  }

  logOut(): boolean {
    if (this.cookies.check("token"))
    {
      this.cookies.delete("token");
      return true;
    }
    return false;
  }

  updateUser(request:IUpdateUser):Observable<ResponseApi> {
    let headers = new HttpHeaders();
    if (this.cookies.check("token"))
    {
      headers.append("Authorization", `bearer ${this.cookies.get("token")}`)
    }
    return this.http.put<ResponseApi>(`${this.apiUrl}UpdateUser`, request, {headers: headers})
  }

  changePassword(request:IChangePassword):Observable<ResponseApi> {
    let headers = new HttpHeaders();
    if (this.cookies.check("token"))
    {
      headers.append("Authorization", `bearer ${this.cookies.get("token")}`)
    }
    return this.http.put<ResponseApi>(`${this.apiUrl}ChangePassword`, request, {headers: headers})
  }

  deleteUser(password:string):Observable<ResponseApi> {
    let headers = new HttpHeaders();
    if (this.cookies.check("token"))
    {
      headers.append("Authorization", `bearer ${this.cookies.get("token")}`)
    }
    return this.http.delete<ResponseApi>(`${this.apiUrl}DeleteUser`, {body: ["password", password], headers: headers}) 
  }
}
