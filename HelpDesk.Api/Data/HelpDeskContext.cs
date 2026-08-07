using HelpDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Api.Data
{
    public class HelpDeskContext : DbContext
    {
        public HelpDeskContext(DbContextOptions<HelpDeskContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Priority).IsRequired().HasMaxLength(20);
                entity.Property(t => t.Status).IsRequired().HasMaxLength(20);
                entity.Property(t => t.RaisedBy).HasMaxLength(150);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
