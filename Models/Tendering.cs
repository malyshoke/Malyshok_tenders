namespace LR2_Malyshok.Models
{
    public class Tendering
    {
        public int TenderingId { get; set; }
        public float CurrentBid { get; set; }
        public int TenderId { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public  Tender Tender { get; set; }
        public void PlaceBet(float NewBid)
        {
            if (CurrentBid > NewBid)
            {
                CurrentBid = NewBid;
            }
            else
            {
                throw new ArgumentException("The bid must be less than the previous bid");
            }
        }

    }
}
