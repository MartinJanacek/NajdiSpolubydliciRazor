using Microsoft.EntityFrameworkCore;
using NajdiSpolubydliciRazor.Entities;

namespace NajdiSpolubydliciRazor.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Demand> Demands { get; set; }

        public DbSet<ContactReceiver> ContactReceivers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new OfferEntityTypeConfiguration().Configure(modelBuilder.Entity<Offer>());
            new DemandEntityTypeConfiguration().Configure(modelBuilder.Entity<Demand>());
        }
    }
}
