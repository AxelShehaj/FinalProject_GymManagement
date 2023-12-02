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

        public List<Subscription> GetSubscriptions()
        {
            var subs = _ApplicationDbContext.Subscription.Where(m => m.IsDeleted == false).ToList();
            return subs;
        }

        public bool SubscriptionExists(string code)
        {
            return _ApplicationDbContext.Subscription.Any(m => m.Code == code);
        }

        public void CreateSubscription(Subscription subscription)
        {

            try
            {
                if (subscription == null)
                {
                    throw new Exception("There is no Subscription added");
                }
                subscription.IsDeleted = false;

                _ApplicationDbContext.Subscription.Add(subscription);
                _ApplicationDbContext.SaveChanges();

            }
            catch (Exception)
            {
                throw new Exception("Error in getting the Members details");
            }

        }

        public Subscription GetSubscriptionByCode(string code)
        {
            try
            {
                if (code == null)
                {
                    throw new Exception("We cant find the subscription, please check the code again");
                }
                var subscription = _ApplicationDbContext.Subscription.Where(m => m.Code == code).FirstOrDefault();
                var sub = new Subscription()
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
            try
            {
                var sub = _ApplicationDbContext.Subscription.Where(m => m.Code == code).FirstOrDefault();

                if (sub != null)
                {
                    sub.IsDeleted = true;
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error");
            }
            
        }

    }
}
