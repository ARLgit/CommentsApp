import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service'; // may need to move it later.
import { ResponseApi } from '../Interfaces/response-api';
import { IPostComment } from '../Interfaces/Comments/post-comment';
import { IUpdateComment } from '../Interfaces/Comments/update-comment';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {

  private apiUrl:string = environment.apiUrl + "Comments/"

  constructor(private http:HttpClient, private cookies:CookieService) { }

  getComments(threadId:Number):Observable<ResponseApi> {
    return this.http.get<ResponseApi>(`${this.apiUrl}GetComments/${threadId}`)
  }

  postComment(request:IPostComment):Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiUrl}PostComment`, request)
  }

  updateComment(commentId:number , request:IUpdateComment):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}UpdateComment/${commentId}`, request)
  }

  deleteComment(commentId:number):Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.apiUrl}DeleteComment/${commentId}`)
  }

}
