using static LR2_Malyshok.Models.DTOClasses;

namespace LR2_Malyshok.Models
{
    public class DTOClasses
    {
        public class CompanyDto
        {
            public int CompanyId { get; set; }
            public float CompanyRating { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string CompanyEmail { get; set; }
            public string CompanyPhone { get; set; }

            public static explicit operator CompanyDto(Company company)
            {
                CompanyDto comp = new CompanyDto();
                comp.CompanyId = company.CompanyId;
                comp.CompanyRating = company.CompanyRating;
                comp.CompanyName = company.CompanyName;
                comp.CompanyAddress = company.CompanyAddress;
                comp.CompanyEmail = company.CompanyEmail;
                comp.CompanyPhone = company.CompanyPhone;
                return comp;
            }
        }
    }
    public class TenderDto
    {
        public int TenderId { get; set; }
        public string TenderName { get; set; }
        public DateTime TenderStart { get; set; }
        public DateTime TenderEnd { get; set; }
        public float TenderBudget { get; set; }
        public int CompanyId { get; set; }

        public static explicit operator TenderDto(Tender tender)
        {
            TenderDto tenderDto = new TenderDto();
            tenderDto.TenderId = tender.TenderId;
            tenderDto.TenderName= tender.TenderName;
            tenderDto.TenderBudget = tender.TenderBudget;
            tenderDto.CompanyId = tender.CompanyId;
            tenderDto.TenderEnd = tender.TenderEnd;
            tenderDto.TenderStart = tender.TenderStart;
            return tenderDto;
        }
    }

    public class TenderingDto
    {
        public int TenderingId { get; set; }
        public float CurrentBid { get; set; }
        public int TenderId { get; set; }
        public int CompanyId { get; set; }
    }
    }
