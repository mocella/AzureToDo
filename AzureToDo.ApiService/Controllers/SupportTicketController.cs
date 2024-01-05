using AzureToDo.ApiService.Entities;
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
            return _ticketContext.Tickets.ToList();
        }

        // GET: api/SupportTicket/5
        [HttpGet("{id}")]
        public SupportTicket? GetSupportTicket(int id)
        {
            return _ticketContext.Tickets.FirstOrDefault(t => t.Id == id);
        }

        // POST: api/SupportTicket
        [HttpPost]
        public void PostSupportTicket([FromBody] SupportTicket ticket)
        {
            _ticketContext.Tickets.Add(ticket); 
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
            _ticketContext.Tickets.Remove(ticket);
            _ticketContext.SaveChanges();    
        }
    }
}
