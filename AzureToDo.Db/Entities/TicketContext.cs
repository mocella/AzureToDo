using Microsoft.EntityFrameworkCore;

namespace AzureToDo.Db.Entities
{
    public class TicketContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<SupportTicket> Tickets => Set<SupportTicket>();
    }
}
