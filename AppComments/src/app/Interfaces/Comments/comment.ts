import { ICreator } from "../Auth/creator";

export interface IComment {
    CommentId:number,
    ThreadId:number,
    CreatorId:number,
    ParentId:number | null,
    Content:string,
    CreationDate:Date,
    LastEdit:Date,
    IsActive:boolean,
    Creator:ICreator | null
}
