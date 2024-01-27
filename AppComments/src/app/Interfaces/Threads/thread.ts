import { ICreator } from "../Auth/creator"
import { IComment } from "../Comments/comment"

export interface IThread {
    ThreadId:number,
    CreatorId:number,
    Title:string,
    Content:string,
    LastEdit:Date,
    Creator:ICreator | null,
    Comments: IComment[] | null
}
