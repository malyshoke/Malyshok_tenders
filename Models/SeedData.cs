using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LR2_Malyshok.Models;
using System;
using System.Linq;
namespace LR2_Malyshok.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TendersDataContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TendersDataContext>>()))
            {
                // Look for any companies
                if (context.Company.Any())
                {
                    return;   // DB has been seeded
                }
                context.Company.AddRange(
                    new Company
                    {
                        CompanyId = 1,
                        CompanyRating = 5,
                        CompanyName = "Gazprom",
                        CompanyAddress = "Moscow",
                        CompanyEmail = "gazprom@gaz.ru",
                        CompanyPhone = "+94875757"
                    },
                    new Company
                    {
                        CompanyId = 2,
                        CompanyRating = 5,
                        CompanyName = "Gazprom",
                        CompanyAddress = "Moscow",
                        CompanyEmail = "gazprom@gaz.ru",
                        CompanyPhone = "+94875757"
                    }
                );

                if (context.Tender.Any())
                {
                    return;   // DB has been seeded
                }
                context.Tender.AddRange(
                    new Tender
                    {
                        TenderId = 1,
                        TenderName = "Lalal",
                        TenderStart = DateTime.Parse("2023-06-03"),
                        TenderEnd = DateTime.Parse("2023-20-03"),
                        TenderBudget = 100,
                        CompanyId = 1
                    }
                );

                if (context.Tendering.Any())
                {
                    return;   // DB has been seeded
                }
                context.Tendering.AddRange(
                    new Tendering
                    {
                        TenderingId = 1,
                        CurrentBid = 90,
                        TenderId =1,
                        CompanyId = 2,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
