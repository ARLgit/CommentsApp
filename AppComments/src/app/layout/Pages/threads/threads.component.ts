import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { ActivatedRoute, NavigationEnd, Router, RouterModule } from '@angular/router';
import { IThread } from '../../../Interfaces/Threads/thread';
import { ThreadsService } from '../../../Services/threads.service';
import { HttpResponse } from '@angular/common/http';
import { ResponseApi } from '../../../Interfaces/response-api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-threads',
  standalone: true,
  imports: [CommonModule, MaterialsModule, RouterModule, FormsModule],
  templateUrl: './threads.component.html',
  styleUrl: './threads.component.css'
})
export class ThreadsComponent implements OnInit{

  searchQuery:string | null = null;
  page:number = 0;
  size:number = 0;
  threadList:IThread[] | null = null;
  totalItemCount: number = 0;
  TotalPageCount: number = 0;

  constructor(
    private Router:Router,
    private Route:ActivatedRoute,
    private Threads:ThreadsService
  ) 
  {
    this.Router.routeReuseStrategy.shouldReuseRoute = function(){
      return false;
    }

    this.Router.events.subscribe((evt) => {
      if (evt instanceof NavigationEnd) {
         // trick the Router into believing it's last link wasn't previously loaded
         this.Router.navigated = false;
         // if you need to scroll back to top, here is the right place
         window.scrollTo(0, 0);
      }
    });
  }

  ngOnInit(): void 
  {

    this.Route.queryParamMap.subscribe(
      {
        next: (params) => {
          this.searchQuery = params.get('searchQuery') || null;
          this.page = +Number(params.get('page')) || 1;
          this.size = +Number(params.get('size')) || 10;
        }
      }
    );

    this.Threads.getThreads(this.searchQuery, this.page, this.size).subscribe(
      {
        next: (response:HttpResponse<ResponseApi>) => {
          if (response.ok) 
          {
            let pagination = JSON.parse(String(response.headers?.get("x-pagination")));
            this.threadList = response.body?.value;
            this.totalItemCount = pagination.TotalItemCount; 
            this.TotalPageCount = pagination.TotalPageCount;
            //console.log(this.threadList)
          }
        },
        error: (err) => {
          //console.log(err);
        },
        complete: () => {
          //console.log('completed');
        } 
      }
    )

  }

}
