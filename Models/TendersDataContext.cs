using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LR2_Malyshok.Models
{
    public class TendersDataContext : DbContext
    {
     
            public TendersDataContext(DbContextOptions<TendersDataContext> options)
                : base(options)
            {
            Database.EnsureCreated();
            }

            public DbSet<LR2_Malyshok.Models.Company> Company { get; set; } = default!;
            public DbSet<LR2_Malyshok.Models.Tender> Tender { get; set; } = default!;
            public DbSet<LR2_Malyshok.Models.Tendering> Tendering { get; set; } = default!;
        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tendering>()
                .HasOne(t => t.Tender)
                .WithMany(ten => ten.Tenderings)
                .HasForeignKey(t => t.TenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Tendering>()
                .HasOne(c => c.Company)
                .WithMany(com => com.CompanyTenderings)
                .HasForeignKey(c => c.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}

