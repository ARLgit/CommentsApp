import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IThread } from '../../../../Interfaces/Threads/thread';
import { ThreadsService } from '../../../../Services/threads.service';
import { ResponseApi } from '../../../../Interfaces/response-api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IComment } from '../../../../Interfaces/Comments/comment';
import { ISession } from '../../../../Interfaces/Auth/session';
import { CommentsService } from '../../../../Services/comments.service';
import { AuthService } from '../../../../Services/auth.service';
import { UtilitiesService } from '../../../../Services/utilities.service';
import { IPostComment } from '../../../../Interfaces/Comments/post-comment';
import { IUpdateComment } from '../../../../Interfaces/Comments/update-comment';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-comment-partial',
  standalone: true,
  imports: [CommonModule, MaterialsModule, RouterModule, FormsModule, CommentPartialComponent],
  templateUrl: './comment-partial.component.html',
  styleUrl: './comment-partial.component.css'
})
export class CommentPartialComponent implements OnInit{

  @Input()
  repliesList:{[key: string]: IComment[]} = {};
  @Input()
  comment: IComment  | null  = null;

  commentId:number = 0;
  threadId:number = 0;
  commentReply:string = '';
  replyBoxState:boolean = false;
  commentContentEdit:string = '';
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
    this.commentId = this.comment ? this.comment.commentId : 0;
    this.threadId = this.comment ? this.comment.threadId : 0;
  }

  getReplies(id:number):IComment[] 
  {
    return id > 0 ? this.repliesList[id] : [] 
  }

  ownsComment():boolean {
    const user: ISession | null = this.Auth.getSession();
    if (user && this.comment)
    {
      return Number(user.sid) == this.comment.creatorId ? true : false;
    }
    return false
  }

  showReplyBox():void {
    const loggedIn = this.Auth.getSession()
    if (loggedIn)
    {
      this.commentReply = '';
      this.editBoxState? this.editBoxState = false : false;
      this.replyBoxState =! this.replyBoxState;
    }
    else
    {
      this.Utilities.Alert('Debe iniciar sesion para responder.','Opps!');
    }
  }

  showEditBox():void {
    const owner = this.ownsComment()
    if (owner)
    {
      this.commentContentEdit = String(this.comment?.content);
      this.replyBoxState? this.replyBoxState = false : false;
      this.editBoxState =! this.editBoxState;
    }
    else
    {
      this.Utilities.Alert('Este comentario no te pertenece.','Opps!');
    }
  }

  confirmDelete():void {
    const owner: boolean = this.ownsComment();
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

  reply(parentId:number, threadId:number):void {
    const user: ISession | null = this.Auth.getSession();
    if (user) 
    {
      const request: IPostComment = {
        threadId: Number(threadId),
        creatorId: Number(user.sid),
        parentId: parentId,
        content: this.commentReply
      }
      
      if (parentId > 0 && threadId > 0)
      {
        this.Comments.postComment(request).subscribe(
          {
            next: response => {
              if (response.status) {
                setTimeout(() => {
                  this.Utilities.Alert(response.message, "Ok");
                }, 1000)
                this.Router.navigate(['thread/', this.threadId])
              }
            },
            error: err => {
              this.Utilities.Alert(err.error.message,"Opps!")
            }
          }
        );
      }
      
    }
    else
    {
      this.Utilities.Alert('Debe iniciar session para responder.',"Opps!")
    }
  }

  edit():void {
    const owner: boolean = this.ownsComment();
    if (owner) 
    {
      const request: IUpdateComment = {
        content: String(this.commentContentEdit)
      };
      
      if (this.commentId > 0)
      {
        this.Comments.updateComment(this.commentId, request).subscribe(
          {
            next: response => {
              console.log(response);
              if (response.status) {
                setTimeout(() => {
                  this.Utilities.Alert(response.message, "Ok");
                }, 1000)
              }
            },
            error: err => {
              console.log(err);
              this.Utilities.Alert(err.error.message,"Opps!")
            },
            complete: () => {
              this.Router.navigate(['thread/', this.threadId])
            }
          }
        );
      }
      
    }
    else
    {
      this.Utilities.Alert('Este mensaje no te pertenece.',"Opps!")
    }
  }
  
  delete():void {
    const owner: boolean = this.ownsComment();
    if (owner) 
    {      
      if (this.commentId > 0)
      {
        this.Comments.deleteComment(this.commentId).subscribe(
          {
            next: response => {
              console.log(response);
              if (response.status) {
                setTimeout(() => {
                  this.Utilities.Alert(response.message, "Ok");
                }, 1000)
                this.Router.navigate(['thread/', this.threadId])
              }
            },
            error: err => {
              console.log(err);
              this.Utilities.Alert(err.error.message,"Opps!")
            }
          }
        );
      }
      
    }
    else
    {
      this.Utilities.Alert('Este mensaje no te pertenece.',"Opps!")
    }
  }

}
