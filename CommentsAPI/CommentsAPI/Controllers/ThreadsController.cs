using AutoMapper;
using CommentsAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : ControllerBase
    {
        private readonly ThreadsRepository _Threads;
        private readonly IMapper _mapper;
        public ThreadsController(ThreadsRepository threads, IMapper mapper)
        {
            _Threads = threads ??
                throw new ArgumentNullException(nameof(threads));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/<ThreadsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ThreadsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ThreadsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ThreadsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ThreadsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
