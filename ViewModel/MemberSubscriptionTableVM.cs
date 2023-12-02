namespace FinalProject_GymManagement.ViewModel
{
    public class MemberSubscriptionTableVM
    {
        public int Id { get; set; } = 1;
        public string MemberCardId { get; set; }
        public string SubscriptionCode { get; set; }
        public string Email { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal PaidPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RemainingSessions { get; set; }
        public bool IsDeleted { get; set; }
    }
}
