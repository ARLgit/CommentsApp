using AutoMapper;
using CommentsAPI.Entities;
using CommentsAPI.Models;
using CommentsAPI.Services;
using CommentsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;
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
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetThreads()
        {
            var threads = await _Threads.GetThreadsAsync();
            if (!threads.Any())
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Failure", Message = "No se ha encontrado ningun Hilo" });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response<IEnumerable<ThreadDTO>> { Status = "Success", Message = "Hilo encontrado", Value = _mapper.Map<IEnumerable<ThreadDTO>>(threads) });
        }

        // GET api/<ThreadsController>/5
        [HttpGet("{id}"), AllowAnonymous]
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
        [HttpPost]
        public async Task<IActionResult> PostThread([FromBody] ThreadDTO thread)
        {
            if (thread == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Hilo no valido." });
            }
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ThreadsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int threadId)
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
