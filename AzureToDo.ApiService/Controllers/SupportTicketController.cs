using AzureToDo.Db.Entities;
using AzureToDo.Db.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AzureToDo.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportTicketController(IGenericRepository<SupportTicket> repository) : ControllerBase
    {        
        // GET: api/SupportTicket
        [HttpGet]
        public IEnumerable<SupportTicket?> GetSupportTickets()
        {
            // obviously not something we'd ever do!
            return repository.Get();
        }

        // GET: api/SupportTicket/5
        [HttpGet("{id}")]
        public SupportTicket? GetSupportTicket(int id)
        {
            return repository.GetById(id);
        }

        // POST: api/SupportTicket
        [HttpPost]
        public void PostSupportTicket([FromBody] SupportTicket ticket)
        {
            repository.Insert(ticket);
            repository.Save();
        }

        // DELETE: api/SupportTicket/5
        [HttpDelete("{id}")]
        public void DeleteSupportTicket(int id)
        {
            repository.Delete(id);
            repository.Save();
        }
    }
}
