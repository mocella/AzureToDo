using Microsoft.EntityFrameworkCore;

namespace AzureToDo.Db.Entities
{
    public class TicketContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<SupportTicket> SupportTickets => Set<SupportTicket>();
    }
}
