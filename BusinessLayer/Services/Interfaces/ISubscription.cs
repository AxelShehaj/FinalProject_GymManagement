using FinalProject_GymManagement.Data.Entities;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface ISubscription
    {
        public void CreateMemberSubscription(Subscriptions subscription);
        public bool SubscriptionExists(string code);
        public Subscriptions GetSubscriptionByCode(string code);
        public List<Subscriptions> GetSubscriptions();
        public void SoftDelete(string code);
    }
}
