using FinalProject_GymManagement.Data.Entities;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface ISubscription
    {
        public void CreateSubscription(Subscription subscription);
        public bool SubscriptionExists(string code);
        public Subscription GetSubscriptionByCode(string code);
        public List<Subscription> GetSubscriptions();
        public void SoftDelete(string code);
    }
}
