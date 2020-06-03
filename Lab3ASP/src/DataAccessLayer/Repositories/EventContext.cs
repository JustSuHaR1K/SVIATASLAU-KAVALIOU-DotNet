using Microsoft.EntityFrameworkCore;
using Eventus.DAL.Models;

namespace EventusDAL.Repositories
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<EventDto> Events { get; set; }

        public DbSet<ClientDto> Clients { get; set; }

        public DbSet<MasterDto> Masters { get; set; }

        public DbSet<OrderDto> Orders { get; set; }
    }
}