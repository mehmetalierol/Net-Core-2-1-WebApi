using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Company.Application.WebApi.SignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Company.Application.WebApi.Controllers
{
    [Route("Message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        public static List<string> Source { get; set; } = new List<string>();

        private IHubContext<MessageHub> context;

        public MessageController(IHubContext<MessageHub> hub)
        {
            this.context = hub;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Source;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return Source[id];
        }

        // POST api/values
        [HttpPost]
        public async void Post([FromBody] string value)
        {
            Source.Add(value);
            await context.Clients.All.SendAsync("Add", value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            Source[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            var item = Source[id];
            Source.Remove(item);
            await context.Clients.All.SendAsync("Delete", item);
        }
    }
}