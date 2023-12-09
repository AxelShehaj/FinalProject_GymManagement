using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinalProject_GymManagement.ViewModel;

namespace FinalProject_GymManagement.BusinessLayer.Services.Implementations
{
    public class SubscriptionService : ISubscription
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public SubscriptionService(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
        }

        private string CreateString(int stringLength)
        {
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
        public List<SubscriptionGridTableVM> GetSubscriptions()
        {
            try
            {
                var subs = _ApplicationDbContext.Subscription.Where(m => m.IsDeleted == false).ToList();
                var subsVM = subs.Select(sub => new SubscriptionGridTableVM
                {
                    Code = sub.Code,
                    Description = sub.Description,
                    NumberOfMonths = sub.NumberOfMonths,
                    TotalNumberOfSessions = sub.TotalNumberOfSessions,
                    TotalPrice = sub.TotalPrice,
                    WeekFrequency = sub.WeekFrequency
                }).ToList();
                return subsVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public bool SubscriptionExists(string code)
        {
            try
            {
                return _ApplicationDbContext.Subscription.Any(m => m.Code == code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public void CreateSubscription(SubscriptionCreateVM subscriptionCreateVM)
        {
            try
            {
                if (subscriptionCreateVM == null)
                {
                    throw new Exception("There is no Member added");
                }

                var sub = new Subscription
                {
                    Code = CreateString(4),
                    Description = subscriptionCreateVM.Description,
                    NumberOfMonths = subscriptionCreateVM.NumberOfMonths,
                    TotalNumberOfSessions = subscriptionCreateVM.TotalNumberOfSessions,
                    TotalPrice = subscriptionCreateVM.TotalPrice,
                    WeekFrequency = subscriptionCreateVM.WeekFrequency,
                    IsDeleted = false
                };
                if (!SubscriptionExists(sub.Code))
                {
                    _ApplicationDbContext.Subscription.Add(sub);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new Exception("Error in getting the Members details");
            }
        }
        public SubscriptionEditVM GetSubscriptionByCode(string code)
        {
            try
            {
                if (code == null)
                {
                    throw new Exception("We cant find the subscription, please check the code again");
                }
                var subscription = _ApplicationDbContext.Subscription.Where(m => m.Code == code).FirstOrDefault();
                var sub = new SubscriptionEditVM()
                {
                    Description = subscription.Description,
                    NumberOfMonths = subscription.NumberOfMonths,
                    WeekFrequency = subscription.WeekFrequency,
                    TotalNumberOfSessions = subscription.TotalNumberOfSessions,
                    TotalPrice = subscription.TotalPrice,
                    Code = subscription.Code
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
        public void Edit(SubscriptionEditVM subscriptionEditVM)
        {

            try
            {
                if (subscriptionEditVM != null)
                {
                    var existingSub = _ApplicationDbContext.Subscription.Where(m => m.Code == subscriptionEditVM.Code).FirstOrDefault();

                    if (existingSub == null)
                    {
                        throw new Exception("Existing Subscription is null");
                    }
                        existingSub.Description = subscriptionEditVM.Description;
                        existingSub.NumberOfMonths = subscriptionEditVM.NumberOfMonths;
                        existingSub.WeekFrequency = subscriptionEditVM.WeekFrequency;
                        existingSub.TotalNumberOfSessions = subscriptionEditVM.TotalNumberOfSessions;
                        existingSub.TotalPrice = subscriptionEditVM.TotalPrice;
                        _ApplicationDbContext.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SubscriptionGridTableVM> Search(SubscriptionFilterSearchVM subscriptions)
        {
            try
            {
                var query = _ApplicationDbContext.Subscription.AsQueryable().Where(s => s.IsDeleted == false);
                if (subscriptions.Code != null)
                {
                    query = query.Where(s => s.Code == subscriptions.Code);
                }
                if (!string.IsNullOrEmpty(subscriptions.Description))
                {
                    query = query.Where(s => s.Description.Contains(subscriptions.Description));
                }
                if (subscriptions.NumberOfMonths != 0)
                {
                    query = query.Where(s => s.NumberOfMonths == subscriptions.NumberOfMonths);
                }
                if (subscriptions.WeekFrequency != null)
                {
                    query = query.Where(s => s.WeekFrequency == subscriptions.WeekFrequency);
                }
                var result = query.Select(s => new SubscriptionGridTableVM
                {
                    Code = s.Code,
                    Description = s.Description,
                    NumberOfMonths = s.NumberOfMonths,
                    TotalNumberOfSessions = s.TotalNumberOfSessions,
                    TotalPrice = s.TotalPrice,
                    WeekFrequency = s.WeekFrequency
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
