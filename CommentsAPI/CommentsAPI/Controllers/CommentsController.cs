using AutoMapper;
using CommentsAPI.Services;
using CommentsAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/<CommentsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CommentsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
