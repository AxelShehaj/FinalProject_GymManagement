using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface ISubscription
    {
        public void CreateSubscription(SubscriptionCreateVM subscriptionCreateVM);
        public bool SubscriptionExists(string code);
        public SubscriptionEditVM GetSubscriptionByCode(string code);
        public List<SubscriptionGridTableVM> GetSubscriptions();
        public void SoftDelete(string code);
        public void Edit(SubscriptionEditVM subscriptionEditVM);
        public List<SubscriptionGridTableVM> Search(SubscriptionFilterSearchVM subscriptions);
    }
}
