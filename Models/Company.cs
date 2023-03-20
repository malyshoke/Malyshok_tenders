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
        public virtual List<Tender> CompanyTenders { get; set; }
        public virtual List<Tendering> CompanyTenderings { get; set; }


        public void AddTender(Tender tender)
        {
            CompanyTenders.Add(tender);
        }

        public void DeleteTender(Tender tender)
        {
            CompanyTenders.Remove(tender);
        }

        public void PartInTendering(Tendering tendering)
        {
            CompanyTenderings.Add(tendering);
        }

        public void CancelTendering(Tendering tendering)
        {
            CompanyTenderings.Remove(tendering);
        }

        public List<Tender> GetActiveTenders()
        {
            var activeTenders = new List<Tender>();
            foreach (var tender in CompanyTenders)
            {
                if (tender.IsOpen())
                {
                    activeTenders.Add(tender);
                }
            }
            return activeTenders;
        }
    }
}
