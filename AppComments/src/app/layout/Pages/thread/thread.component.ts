import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MaterialsModule } from '../../../MaterialsModule/materials/materials.module';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IThread } from '../../../Interfaces/Threads/thread';
import { ThreadsService } from '../../../Services/threads.service';
import { ResponseApi } from '../../../Interfaces/response-api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IComment } from '../../../Interfaces/Comments/comment';
import { CommentPartialComponent } from './comment-partial/comment-partial.component';

@Component({
  selector: 'app-thread',
  standalone: true,
  imports: [CommonModule, MaterialsModule, RouterModule, FormsModule, CommentPartialComponent],
  templateUrl: './thread.component.html',
  styleUrl: './thread.component.css'
})
export class ThreadComponent implements OnInit{

  id:number = 0;
  thread:IThread | null = null;
  sortedComments:IComment[] | null | undefined = null;
  sortedReplies:{[key: string]: IComment[]} = {};

  constructor(
    private Router:Router,
    private Route:ActivatedRoute,
    private Threads:ThreadsService
  ) 
  {
  }

  ngOnInit(): void 
  {

    this.Route.paramMap.subscribe(
      {
        next: (params) => {
          this.id = +Number(params.get('id')) || 1;
        }
      }
    );

    this.Threads.getThread(this.id).subscribe(
      {
        next: (response:ResponseApi) => {
          if (response.status) 
          {
            this.thread = response.value;
            this.sortedComments = this.thread?.comments?.sort(function(a,b)// may be unnecesary
            {
              let dateA = new Date(a.creationDate)
              let dateB = new Date(b.creationDate) 
              return dateA.getTime() - dateB.getTime();
            });
            const group:{[key: string]: IComment[]} = {};
            this.sortedComments?.forEach(comment => {
              group[Number(comment.parentId)] ||= [];
              group[Number(comment.parentId)].push(comment)
            });
            this.sortedReplies = group;
            
            /*Object.entries(this.sortedReplies).forEach(c => {
              console.log(c[1]);
            })*/

            /*console.log(this.thread);
            console.log(this.sortedComments);
            console.log(this.sortedReplies);*/
          }
        },
        error: (err) => {
          //console.log(err);
        },
        complete: () => {
          console.log('completed');
        } 
      }
    )

  }

  getReplies(id:number):IComment[] 
  {
    return id >= 0 ? this.sortedReplies[id] : []
  }

}
