using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface IMemberSubscription
    {
        public bool MemberSubscriptionExist(Member member);
        public void ActivateSubscription(string memberCardID, string subscribtionCode);
        public List<MemberSubscriptionTableVM> GetMembersSubscription();
        public MemberSubscriptionTableVM GetMemberSubscriptionByDetail(string memberCardID, string subscribtionCode);
        public void SoftDelete(string memberCardID, string subscriptionCode);

    }
}
