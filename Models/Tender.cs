using NuGet.Versioning;
using System.Security.Cryptography;

namespace LR2_Malyshok.Models
{
    public class Tender
    {
        public int TenderId { get; set; }
        public string TenderName { get; set; }
        public DateTime TenderStart { get; set; }
        public DateTime TenderEnd { get; set; }
        public float TenderBudget { get; set; }
        public int OwnerId { get; set; }
        public List<Tendering> Tenderings { get; set; }


        public void CreateTender(string Name, DateTime Start, DateTime End, float Budget, int CompanyId)
        {
            TenderName = Name;
            TenderStart = Start;
            TenderEnd = End;
            TenderBudget = Budget;
            OwnerId = CompanyId;
        }

        public int SelectWinner()
        {
            if (Tenderings.Count == 0)
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
            if (DateTime.Now >= TenderStart && DateTime.Now <= TenderEnd)
            return true;
            else return false;
        }

    }
}
