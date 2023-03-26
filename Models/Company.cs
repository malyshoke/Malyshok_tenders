using System.Collections.Generic;
using System.Security.Cryptography;
using static LR2_Malyshok.Models.DTOClasses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public ICollection<Tender> CompanyTenders { get; set; }
        public ICollection<Tendering> CompanyTenderings { get; set; }

        public Company()
        {
            CompanyTenders = new List<Tender>();
            CompanyTenderings = new List<Tendering>();
        }
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

        public static explicit operator Company(DTOClasses.CompanyDto companydto)
        {
            Company comp = new Company();
            comp.CompanyId = companydto.CompanyId;
            comp.CompanyRating = companydto.CompanyRating;
            comp.CompanyName = companydto.CompanyName;
            comp.CompanyAddress = companydto.CompanyAddress;
            comp.CompanyEmail = companydto.CompanyEmail;
            comp.CompanyPhone = companydto.CompanyPhone;
            List<Tender> CompanyTenders = new List<Tender>();
            List<Tendering> CompanyTenderings = new List<Tendering>();
            return comp;
        }
    }
}
