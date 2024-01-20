import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service'; // may need to move it later.
import { ResponseApi } from '../Interfaces/response-api';
import { IChangePassword } from '../Interfaces/Auth/change-password';
import { ILogIn } from '../Interfaces/Auth/log-in';
import { IRegisterUser } from '../Interfaces/Auth/register-user';
import { IUpdateUser } from '../Interfaces/Auth/update-user';

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

  logOut():boolean {
    /*IMPLEMENT LATER*/
    return false
  }

  updateUser(request:IUpdateUser):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}UpdateUser`, request)
  }

  changePassword(request:IChangePassword):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}ChangePassword`, request)
  }

  deleteUser(password:string):Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.apiUrl}DeleteUser`, {body: ["password", password]}) 
  }
}
