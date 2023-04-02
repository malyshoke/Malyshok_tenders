using NuGet.Versioning;
using System.Drawing.Printing;
using System.Security.Cryptography;
using static LR2_Malyshok.Models.DTOClasses;

namespace LR2_Malyshok.Models
{
    public class Tender
    {
        public int TenderId { get; set; }
        public string TenderName { get; set; }
        public DateTime TenderStart { get; set; }
        public DateTime TenderEnd { get; set; }
        public float TenderBudget { get; set; }
        public int CompanyId { get; set; }
        public ICollection<Tendering> Tenderings { get; set; } = new List<Tendering>();

        public Company Company { get; set; }

        public void AddTendering(Tendering tendering)
        {
            Tenderings.Add(tendering);
        }
        public void RemoveTendering(Tendering tendering)
        {
            Tenderings.Remove(tendering);
        }
        public int SelectWinner()
        {
            if (Tenderings.Count == 0 || Tenderings == null)
            {
                return 0;
            }
            else
            {
                var WinBet = Tenderings.OrderBy(bet => bet.CurrentBid).First();
                return WinBet.CompanyId;
            }
        }

        public bool IsOpen()
        {
            if (DateTime.UtcNow >= TenderStart && DateTime.UtcNow <= TenderEnd)
            return true;
            else return false;
        }

        public static explicit operator Tender(TenderDto tenderdto)
        {
            Tender tender = new Tender();
            tender.TenderId = tenderdto.TenderId;
            tender.TenderName = tenderdto.TenderName;
            tender.CompanyId= tenderdto.CompanyId;
            tender.TenderStart = tenderdto.TenderStart;
            tender.TenderBudget = tenderdto.TenderBudget;
            ICollection<Tendering> Tenderings =  new List<Tendering>();
            return tender;
        }
        public void ExtendTenderPeriod(int days)
        {
            TenderEnd = TenderEnd.AddDays(days);
        }

        public void UpdateBudget(float amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than 0");
            }

            TenderBudget += amount;
        }

    }
}
