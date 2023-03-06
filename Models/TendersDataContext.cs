using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LR2_Malyshok.Models
{
    public class TendersDataContext : DbContext
    {
     
            public TendersDataContext(DbContextOptions<TendersDataContext> options)
                : base(options)
            {
            }

            public DbSet<LR2_Malyshok.Models.Company> Company { get; set; } = default!;
            public DbSet<LR2_Malyshok.Models.Tender> Tender { get; set; } = default!;
            public DbSet<LR2_Malyshok.Models.Tendering> Tendering { get; set; } = default!;
        }
    }

