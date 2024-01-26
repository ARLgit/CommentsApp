import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { CookieService } from 'ngx-cookie-service'; // may need to move it later.
import { ResponseApi } from '../Interfaces/response-api';
import { IPostThread } from '../Interfaces/Threads/post-thread';
import { IUpdateThread } from '../Interfaces/Threads/update-thread';

@Injectable({
  providedIn: 'root'
})
export class ThreadsService {

  private apiUrl:string = environment.apiUrl + "Threads/"

  constructor(private http:HttpClient, private cookies:CookieService) { }

  getThreads(searchQuery:string|null =  null, page: number|null =  null, size: number|null =  null):Observable<ResponseApi> {
    let url:string = `${this.apiUrl}GetThreads?`
    if (searchQuery !== null) {
      url += `searchQuery=${searchQuery}`
    }
    if (page !== null) {
      url += `&page=${page}`
    }
    if (size !== null) {
      url += `&size=${size}`
    }
    return this.http.get<ResponseApi>(url)
  }

  getThread(threadId:Number):Observable<ResponseApi> {
    return this.http.get<ResponseApi>(`${this.apiUrl}GetThread/${threadId}`)
  }

  postThread(request:IPostThread):Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.apiUrl}PostThread`, request)
  }

  updateThread(commentId:number , request:IUpdateThread):Observable<ResponseApi> {
    return this.http.put<ResponseApi>(`${this.apiUrl}UpdateThread/${commentId}`, request)
  }

  deleteThread(commentId:number):Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.apiUrl}DeleteThread/${commentId}`)
  }
}
