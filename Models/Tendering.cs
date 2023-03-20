namespace LR2_Malyshok.Models
{
    public class Tendering
    {
        public int TenderingId { get; set; }
        public float CurrentBid { get; set; }
        public int TenderId { get; set; }
        public int CompanyId { get; set; }

        public void PlaceBet(float NewBid)
        {
            if (CurrentBid > NewBid)
            {
                CurrentBid = NewBid;
            }
            else
            {
                
            }
        }

    }
}
