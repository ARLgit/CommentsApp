using AutoMapper;
using CommentsAPI.Entities;
using CommentsAPI.Models;
using CommentsAPI.Services;
using CommentsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Text.Json;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadsRepository _Threads;
        private readonly IMapper _mapper;
        public ThreadsController(IThreadsRepository threads, IMapper mapper)
        {
            _Threads = threads ??
                throw new ArgumentNullException(nameof(threads));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/<ThreadsController>
        [HttpGet("GetThreads"), AllowAnonymous]
        public async Task<IActionResult> GetThreads(int page = 0, int size = 0, string? searchQuery = null)
        {
            var (threads, paginationMetadata) = await _Threads.GetThreadsAsync(page, size, searchQuery);
            if (!threads.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "No se ha encontrado ningun Hilo" });
            }
            if (paginationMetadata != null)
            {
                Response.Headers.Append("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response<IEnumerable<ThreadDTO>> { Status = "Success", Message = "Hilo encontrado", Value = _mapper.Map<IEnumerable<ThreadDTO>>(threads) });
        }

        // GET api/<ThreadsController>/5
        [HttpGet("GetThread/{threadId}"), AllowAnonymous]
        public async Task<IActionResult> GetThread(int threadId, bool getReplies = true)
        {
            var thread = await _Threads.GetThreadAsync(threadId, getReplies);
            if (thread == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "No se ha encontrado ningun Hilo con esa Id" });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response<ThreadDTO> { Status = "Success", Message = "Hilo encontrado", Value = _mapper.Map<ThreadDTO>(thread) });
        }

        // POST api/<ThreadsController>
        [HttpPost("PostThread")]
        public async Task<IActionResult> PostThread([FromBody] PostThreadDTO thread)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Hilo no valido." });
            }
            //get user id from Sid claim.
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            // check CreatorId matches Sid from claims
            if (thread.CreatorId.ToString() != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "Su Id no corresponde con la creatorId proveida en el nuevo hilo." });
            }
            // Create thread
            var result = await _Threads.CreateThreadAsync(_mapper.Map<Entities.Thread>(thread));
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "No se ha podido crear su hilo." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Hilo creado exitosamente." });
        }

        // PUT api/<ThreadsController>/5
        [HttpPut("UpdateThread/{threadId}")]
        public async Task<IActionResult> UpdateThread(int threadId, [FromBody] UpdateThreadDTO updatedThread)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Hilo no valido." });
            }
            //Get Sid from claims
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //get user by id
            var thread = await _Threads.GetThreadAsync(threadId, false);
            if (thread == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "El Hilo no existe." });
            }
            //update the user and check result
            thread.Title = updatedThread.Title;
            thread.Content = updatedThread.Content;
            thread.LastEdit = DateTime.Now;
            var updateResult = await _Threads.UpdateThreadAsync(thread);
            if (!updateResult)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "Hilo no pudo ser actualizado." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "El Hilo ha side actualizado exitosamente." });
        }

        // DELETE api/<ThreadsController>/5
        [HttpDelete("DeleteThread/{threadId}")]
        public async Task<IActionResult> DeleteThread(int threadId)
        {
            //get user id from Sid claim.
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //Get the comment and check it exists
            var thread = await _Threads.GetThreadAsync(threadId, false);
            if (thread == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "Hilo no encontrado." });
            }
            if (thread.CreatorId.ToString() != id)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Failure", Message = "Este Hilo no te pertenece." });
            }
            //delete the comment
            var result = await _Threads.DeleteThreadAsync(thread);
            if (!result)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "No se ha podido borrar su hilo." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "Hilo borrado exitosamente." });
        }
    }
}
