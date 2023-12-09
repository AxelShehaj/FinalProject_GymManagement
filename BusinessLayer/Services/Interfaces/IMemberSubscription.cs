using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface IMemberSubscription
    {
        public bool MemberSubscriptionExist(Member member);
        public void ActivateSubscription(string memberCardID, string subscribtionCode);
        public List<MemberSubscriptionTableVM> GetMembersSubscription();
        public MemberSubscriptionEditVM GetMemberSubscriptionByDetail(string memberCardID, string subscriptionCode);
        public void SoftDelete(string memberCardID, string subscriptionCode);
        public List<SelectListItem> GetMembersCardID();
        public List<SelectListItem> GetSubscriberCode();
        public void Edit(MemberSubscriptionEditVM memberSubscriptionEditVM);
    }
}
