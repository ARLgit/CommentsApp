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
import { CommentsService } from '../../../Services/comments.service';
import { IPostComment } from '../../../Interfaces/Comments/post-comment';
import { AuthService } from '../../../Services/auth.service';
import { ISession } from '../../../Interfaces/Auth/session';
import { UtilitiesService } from '../../../Services/utilities.service';

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
  threadReply:string = '';
  showReplyBox:boolean = false;

  constructor(
    private Router:Router,
    private Route:ActivatedRoute,
    private Threads:ThreadsService,
    private Comments:CommentsService,
    private Auth:AuthService,
    private Utilities:UtilitiesService
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
            const group:{[key: string]: IComment[]} = {};
            this.thread?.comments?.forEach(comment => {
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
          //console.log('completed');
        } 
      }
    )

  }

  getReplies(id:number):IComment[] 
  {
    return id >= 0 ? this.sortedReplies[id] : []
  }
  
  reply():void {
    const user: ISession | null = this.Auth.getSession();
    if (user) 
    {
      const request: IPostComment = {
        threadId: Number(this.thread?.threadId),
        creatorId: Number(user.sid),
        content: this.threadReply
      }
      this.Comments.postComment(request);
      //armar el .suscbribe aca
      this.Router.navigate(['/thread', this.id])
    }
  }



}
