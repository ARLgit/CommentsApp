import { ICreator } from "../Auth/creator"
import { IComment } from "../Comments/comment"

export interface IThread {
    threadId:number,
    creatorId:number,
    title:string,
    content:string,
    lastEdit:Date,
    creator:ICreator | null,
    comments: IComment[] | null
}
