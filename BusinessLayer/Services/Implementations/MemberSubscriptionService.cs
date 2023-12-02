using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using System.Net.Sockets;

namespace FinalProject_GymManagement.BusinessLayer.Services.Implementations
{
    public class MemberSubscriptionService : IMemberSubscription
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public MemberSubscriptionService(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
        }

        public List<MemberSubscriptionTableVM> GetMembersSubscription()
        {
            var membersSub = _ApplicationDbContext.MemberSubscriptions.Where(m => m.IsDeleted == false).Select(ms => new MemberSubscriptionTableVM
            {
                Id = ms.ID,
                MemberCardId = ms.Member.IdCardNumber,
                SubscriptionCode = ms.Subscription.Code,
                Email = ms.Member.Email,
                OriginalPrice = ms.OriginalPrice,
                DiscountValue = ms.DiscountValue,
                PaidPrice = ms.PaidPrice,
                StartDate = ms.StartDate,
                EndDate = ms.EndDate,
                RemainingSessions = ms.RemainingSessions,
                IsDeleted = ms.IsDeleted
            }).ToList();
            return membersSub;
        }

        public bool MemberSubscriptionExist (Member member)
        {
            return _ApplicationDbContext.MemberSubscriptions.Any(ms => ms.MemberID == member.ID);
        }

        public void ActivateSubscription(string memberCardID, string subscribtionCode)
        {

            try
            {
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberCardID).FirstOrDefault();
                var subscription = _ApplicationDbContext.Subscription.Where(s => s.Code == subscribtionCode).FirstOrDefault();

                if (member == null && subscription == null)
                {
                    throw new Exception("Error there is no member neither subscription!");
                }

                decimal discountValue = 0;
                decimal paidPrice = subscription.TotalPrice - discountValue;

                var newMemberSubscription = new MemberSubscription
                {
                    MemberID = member.ID,
                    SubscriptionID = subscription.ID,
                    OriginalPrice = subscription.TotalPrice,
                    DiscountValue = discountValue,
                    PaidPrice = paidPrice,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddMonths(subscription.NumberOfMonths),
                    RemainingSessions = subscription.TotalNumberOfSessions,
                    IsDeleted = subscription.IsDeleted,
                };

                _ApplicationDbContext.MemberSubscriptions.Add(newMemberSubscription);
                _ApplicationDbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Error in adding the subscription to the member");
            }
            
        }

        public MemberSubscriptionTableVM GetMemberSubscriptionByDetail(string memberCardID, string subscriptionCode)
        {
            try
            {
                if (memberCardID == null && subscriptionCode == null)
                {
                    throw new Exception("We cant find the members subscription, please check the details again");
                }
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberCardID).FirstOrDefault();
                var subscription = _ApplicationDbContext.Subscription.Where(s => s.Code == subscriptionCode).FirstOrDefault();

                var ms = _ApplicationDbContext.MemberSubscriptions.Where(ms => ms.MemberID == member.ID && ms.SubscriptionID == subscription.ID).FirstOrDefault();
                var sub = new MemberSubscriptionTableVM()
                {
                    Email = ms.Member.Email,
                    OriginalPrice = ms.OriginalPrice,
                    DiscountValue = ms.DiscountValue,
                    PaidPrice = ms.PaidPrice,
                    StartDate = ms.StartDate,
                    EndDate = ms.EndDate,
                    RemainingSessions = ms.RemainingSessions,
                };
                return sub;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting subscription");
            }
        }


        public void SoftDelete(string memberCardID, string subscriptionCode)
        {
            try
            {
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberCardID).FirstOrDefault();
                var subscription = _ApplicationDbContext.Subscription.Where(s => s.Code == subscriptionCode).FirstOrDefault();

                var memberSub = _ApplicationDbContext.MemberSubscriptions.Where(ms => ms.MemberID == member.ID && ms.SubscriptionID == subscription.ID).FirstOrDefault();

                if (memberSub != null)
                {
                    memberSub.IsDeleted = true;
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
