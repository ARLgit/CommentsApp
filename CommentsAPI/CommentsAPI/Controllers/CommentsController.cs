using AutoMapper;
using CommentsAPI.Entities;
using CommentsAPI.Models;
using CommentsAPI.Services;
using CommentsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _Comments;
        private readonly IMapper _mapper;
        public CommentsController(ICommentsRepository comments, IMapper mapper)
        {
            _Comments = comments ?? 
                throw new ArgumentNullException(nameof(comments));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // GET api/<CommentsController>/5
        [HttpGet("{threadId}"), AllowAnonymous]
        public async Task<IActionResult> GetComments(int threadId)
        {
            var comments = await _Comments.GetCommentsAsync(threadId);
            if (comments == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "No se ha encontrado ningun comentario" });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response<IEnumerable<CommentDTO>> { Status = "Success", Message = "Comentario encontrado", Value = _mapper.Map<IEnumerable<CommentDTO>>(comments) });
        }

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CommentDTO comment)
        {
            if (comment == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Comentario no valido." });
            }
            var result = await _Comments.CreateCommentAsync(_mapper.Map<Comment>(comment));
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "No se ha podido crear su comentario." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Comentario creado exitosamente." });
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //hay que implementar esto :S
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int commentId, bool getReplies = false)
        {
            //check ClaimsPrincipal Exists
            if (User is null)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            //get user id from Sid claim.
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //Get the comment and check it exists
            var comment = await _Comments.GetCommentAsync(commentId, getReplies);
            if (comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "Comentario no encontrado." });
            }
            if (comment.CreatorId.ToString() != id) 
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Failure", Message = "Este comentario no te pertenece." });
            }
            //delete the comment
            var result = await _Comments.DeleteCommentAsync(comment);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "No se ha podido borrar su comentario." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Comentario borrado exitosamente." });

        }
    }
}
