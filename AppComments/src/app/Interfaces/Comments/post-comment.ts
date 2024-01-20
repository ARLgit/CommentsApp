export interface IPostComment {
    threadId: number,
    creatorId: number,
    parentId?: number,
    content: string
}
