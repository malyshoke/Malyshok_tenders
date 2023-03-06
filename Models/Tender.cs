using System.Security.Cryptography;

namespace LR2_Malyshok.Models
{
    public class Tender
    {
        public int TenderId { get; set; }
        public string TenderName { get; set; }
        public DateTime TenderStartDate { get; set; }
        public DateTime TenderEndDate { get; set; }
        public float TenderBudget { get; set; }
        public int OwnerId { get; set; }
        public List<Tendering> Tenderings { get; set; }
    }
}
