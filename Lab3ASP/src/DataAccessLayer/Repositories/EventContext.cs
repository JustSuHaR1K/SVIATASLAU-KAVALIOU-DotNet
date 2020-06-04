using Microsoft.EntityFrameworkCore;
using Eventus.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventusDAL.Repositories
{
    public class EventContext : IdentityDbContext<UserDto>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}