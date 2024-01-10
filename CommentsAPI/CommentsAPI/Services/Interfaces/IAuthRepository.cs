namespace CommentsAPI.Services.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> RegisterNewAccount(int threadId);
        Task<bool> LogIn(int threadId);
    }
}
