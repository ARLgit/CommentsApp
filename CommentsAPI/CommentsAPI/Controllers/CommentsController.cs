﻿using AutoMapper;
using CommentsAPI.Entities;
using CommentsAPI.Models;
using CommentsAPI.Services;
using CommentsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;

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
        [HttpGet("GetComments/{threadId}"), AllowAnonymous]
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
        [HttpPost("PostComment")]
        public async Task<IActionResult> PostComment([FromBody] PostCommentDTO comment)
        {
            if (!ModelState.IsValid)
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
        [HttpPut("UpdateComment/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody, Required] string content)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Comentario no valido." });
            }
            //Get Sid from claims
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //get user by id
            var comment = await _Comments.GetCommentAsync(commentId, false);
            if (comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "El Comentario no existe." });
            }
            //update the user and check result
            comment.Content = content;
            comment.LastEdit = DateTime.Now;
            var updateResult = await _Comments.UpdateCommentAsync(comment);
            if (!updateResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "El comentario no pudo ser actualizado." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "El comentario ha side actualizado exitosamente." });
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            //get user id from Sid claim.
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //Get the comment and check it exists
            var comment = await _Comments.GetCommentAsync(commentId, false);
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
