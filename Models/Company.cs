using System.Security.Cryptography;

namespace LR2_Malyshok.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public float CompanyRating { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyPhone { get; set; }
        public  List<Tender> CompanyTenders { get; set; }
        public  List<Tendering> CompanyTenderings { get; set; }
    }
}
