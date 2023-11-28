using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;

namespace FinalProject_GymManagement.BusinessLayer.Services.Implementations
{
    public class SubscriptionService : ISubscription
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public SubscriptionService(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
        }

        public List<Subscriptions> GetSubscriptions()
        {
            var subs = _ApplicationDbContext.Subscriptions.Where(m => m.IsDeleted == false).ToList();
            return subs;
        }

        public bool SubscriptionExists(string code)
        {
            return _ApplicationDbContext.Subscriptions.Any(m => m.Code == code);
        }

        public void CreateMemberSubscription(Subscriptions subscription)
        {

            try
            {
                if (subscription == null)
                {
                    throw new Exception("There is no Subscription added");
                }
                subscription.IsDeleted = false;

                _ApplicationDbContext.Subscriptions.Add(subscription);
                _ApplicationDbContext.SaveChanges();

            }
            catch (Exception)
            {
                throw new Exception("Error in getting the Members details");
            }

        }

        public Subscriptions GetSubscriptionByCode(string code)
        {
            try
            {
                if (code == null)
                {
                    throw new Exception("We cant find the subscription, please check the code again");
                }
                var subscription = _ApplicationDbContext.Subscriptions.Where(m => m.Code == code).FirstOrDefault();
                var sub = new Subscriptions()
                {
                    Description = subscription.Description,
                    NumberOfMonths = subscription.NumberOfMonths,
                    WeekFrequency = subscription.WeekFrequency,
                    TotalNumberOfSessions = subscription.TotalNumberOfSessions,
                    TotalPrice = subscription.TotalPrice,
                    IsDeleted = subscription.IsDeleted,
                };
                return sub;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting subscription");
            }
        }

        public void SoftDelete(string code)
        {
            var sub = _ApplicationDbContext.Subscriptions.Where(m => m.Code == code).FirstOrDefault();

            if (sub != null)
            {
                sub.IsDeleted = true;
                _ApplicationDbContext.SaveChanges();
            }
        }

    }
}
