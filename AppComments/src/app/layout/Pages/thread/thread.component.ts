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
import { IUpdateThread } from '../../../Interfaces/Threads/update-thread';
import Swal from 'sweetalert2';

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
  replyBoxState:boolean = false;
  threadTitleEdit:string = '';
  threadContentEdit:string = '';
  editBoxState:boolean = false;

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

  ownsThread():boolean {
    const user: ISession | null = this.Auth.getSession();
    if (user && this.thread)
    {
      return Number(user.sid) == this.thread.creatorId ? true : false;
    }
    return false
  }

  showReplyBox():void {
    const loggedIn = this.Auth.getSession()
    if (loggedIn)
    {
      this.threadReply = '';
      this.replyBoxState =! this.replyBoxState;
    }
    else
    {
      this.Utilities.Alert('Debe iniciar sesion para responder.','Opps!');
    }
  }

  showEditBox():void {
    const owner = this.ownsThread()
    if (owner)
    {
      this.threadTitleEdit = String(this.thread?.title);
      this.threadContentEdit = String(this.thread?.content);
      this.editBoxState =! this.editBoxState;
    }
    else
    {
      this.Utilities.Alert('Este hilo no te pertenece.','Opps!');
    }
  }

  confirmDelete():void {
    const owner: boolean = this.ownsThread();
    if (owner)
    {
      Swal.fire({
        title: "Esta seguro que desea borrar su hilo?",
        text: "Una vez borrado no podra ser recuperado.",
        icon: "warning",
        showCancelButton: true,
        cancelButtonAriaLabel: 'Cancelar',
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, Borralo!"
      }).then((result) => {
        if (result.isConfirmed) {
          this.delete();
        }
      });
    }
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
      this.Comments.postComment(request).subscribe(
        {
          next: response => {
            if (response.status) {
              setTimeout(() => {
                this.Utilities.Alert(response.message, "Ok");
              }, 1000)
              this.Router.navigate(['thread/', this.id])           
            }
          },
          error: err => {
            this.Utilities.Alert(err.error.message,"Opps!")
          }
        }
      );
    }
    else
    {
      this.Utilities.Alert('Debe iniciar session para responder.',"Opps!")
    }
  }

  edit():void {
    const owner: boolean = this.ownsThread();
    if (owner) 
    {
      const request: IUpdateThread = {
        title: this.threadTitleEdit,
        content: this.threadContentEdit
      }
      this.Threads.updateThread(this.id, request).subscribe(
        {
          next: response => {
            if (response.status) {
              setTimeout(() => {
                this.Utilities.Alert(response.message, "Ok");
              }, 1000)
              this.Router.navigate(['thread/', this.id])           
            }
          },
          error: err => {
            this.Utilities.Alert(err.error.message,"Opps!")
          }
        }
      );
    }
    else
    {
      this.Utilities.Alert('Este hilo no te pertenece.',"Opps!")
    }
  }

  delete():void {
    const owner: boolean = this.ownsThread();
    if (owner) 
    {
      this.Threads.deleteThread(this.id).subscribe(
        {
          next: response => {
            if (response.status) {
              setTimeout(() => {
                this.Utilities.Alert(response.message, "Ok");
              }, 1000)
              this.Router.navigate(['/threads'])           
            }
          },
          error: err => {
            this.Utilities.Alert(err.error.message,"Opps!")
          }
        }
      );
    }
    else
    {
      this.Utilities.Alert('Este hilo no te pertenece.',"Opps!")
    }
  }

}
