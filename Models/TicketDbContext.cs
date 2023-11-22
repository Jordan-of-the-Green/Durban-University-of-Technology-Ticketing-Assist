using Microsoft.EntityFrameworkCore;
using Helpful_Hackers._XBCAD7319._POE.Models;

namespace Helpful_Hackers._XBCAD7319._POE.Models
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
        { }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserModel> UserModel { get; set; }

        public DbSet<UnresolvedTicket> UnresolvedTickets { get; set; } 

        public DbSet<Review> Reviews { get; set; }
    }
}
