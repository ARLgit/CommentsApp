import { ICreator } from "../Auth/creator";

export interface IComment {
    commentId:number,
    threadId:number,
    creatorId:number,
    parentId:number | null,
    content:string,
    creationDate:Date,
    lastEdit:Date,
    isActive:boolean,
    creator:ICreator | null
}
