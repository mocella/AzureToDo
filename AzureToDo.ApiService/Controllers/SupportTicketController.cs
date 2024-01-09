using AzureToDo.Db.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AzureToDo.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportTicketController : ControllerBase
    {
        private readonly TicketContext _ticketContext;

        public SupportTicketController(TicketContext ticketContext)
        {
            _ticketContext = ticketContext;
        }

        // GET: api/SupportTicket
        [HttpGet]
        public IEnumerable<SupportTicket?> GetSupportTickets()
        {
            // obviously not something we'd ever do!
            return _ticketContext.SupportTickets.ToList();
        }

        // GET: api/SupportTicket/5
        [HttpGet("{id}")]
        public SupportTicket? GetSupportTicket(int id)
        {
            return _ticketContext.SupportTickets.FirstOrDefault(t => t.Id == id);
        }

        // POST: api/SupportTicket
        [HttpPost]
        public void PostSupportTicket([FromBody] SupportTicket ticket)
        {
            _ticketContext.SupportTickets.Add(ticket); 
            _ticketContext.SaveChanges();
        }

        // DELETE: api/SupportTicket/5
        [HttpDelete("{id}")]
        public void DeleteSupportTicket(int id)
        {
            var ticket = GetSupportTicket(id);
            if (ticket == null)
            {
                return;
            }   
            _ticketContext.SupportTickets.Remove(ticket);
            _ticketContext.SaveChanges();    
        }
    }
}
