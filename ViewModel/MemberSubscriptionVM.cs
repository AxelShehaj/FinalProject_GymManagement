using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject_GymManagement.ViewModel
{
    public class MemberSubscriptionVM
    {
        public string MemberCardID { get; set; }
        public string SubscribtionCode { get; set; }
        public List<SelectListItem> MemberCardList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> SubscriptionCodeList { get; set; } = new List<SelectListItem>();
    }
}
