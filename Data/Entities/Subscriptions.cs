using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProject_GymManagement.Data.Entities
{
    public class Subscriptions
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int NumberOfMonths { get; set; }
        public string WeekFrequency { get; set; }
        public int TotalNumberOfSessions { get; set; }
        public decimal TotalPrice {  get; set; }
        public bool IsDeleted { get; set; }
    }
}
