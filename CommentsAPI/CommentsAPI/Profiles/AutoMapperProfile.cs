using AutoMapper;
using CommentsAPI.Entities;
using CommentsAPI.Models;

namespace CommentsAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User
            CreateMap<ApplicationUser, RegisterUserDTO>().ReverseMap();
            #endregion

            #region Thread
            CreateMap<Entities.Thread, ThreadDTO>().ReverseMap();
            #endregion

            #region Comment
            CreateMap<Comment, CommentDTO>().ReverseMap();
            #endregion
        }
    }
}
