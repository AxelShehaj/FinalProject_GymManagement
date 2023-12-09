namespace FinalProject_GymManagement.ViewModel
{
    public class SubscriptionGridTableVM
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int NumberOfMonths { get; set; }
        public string WeekFrequency { get; set; }
        public int TotalNumberOfSessions { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
