import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MaterialsModule } from '../../../../MaterialsModule/materials/materials.module';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { IThread } from '../../../../Interfaces/Threads/thread';
import { ThreadsService } from '../../../../Services/threads.service';
import { ResponseApi } from '../../../../Interfaces/response-api';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IComment } from '../../../../Interfaces/Comments/comment';

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
  showReplyBox:boolean = false;
  commentReply:string = '';
  
  constructor(
    private Router:Router,
    private Route:ActivatedRoute,
    private Threads:ThreadsService
  ) 
  {
    
  }

  ngOnInit(): void 
  {
    this.commentId = this.comment ? this.comment.commentId : 0
  }

  getReplies(id:number):IComment[] 
  {
    return id > 0 ? this.repliesList[id] : [] 
  }

  reply():void {
    
  }

}
