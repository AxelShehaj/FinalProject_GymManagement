using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            try
            {
                var membersSub = _ApplicationDbContext.MemberSubscriptions
                .Where(m => m.IsDeleted == false && m.Member.IsDeleted == false && m.Subscription.IsDeleted == false)
                .Select(ms => new MemberSubscriptionTableVM
                {
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool MemberSubscriptionExist (MemberSubscriptionVM memberSubscriptionVM)
        {
            try
            {
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberSubscriptionVM.MemberCardID).FirstOrDefault();
                return _ApplicationDbContext.MemberSubscriptions.Any(ms => ms.MemberID == member.ID && ms.IsDeleted == false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActivateSubscription(string memberCardID, string subscribtionCode)
        {

            try
            {
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberCardID).FirstOrDefault();
                var subscription = _ApplicationDbContext.Subscription.Where(s => s.Code == subscribtionCode).FirstOrDefault();

                decimal CalculateDiscount()
                {
                    switch (subscription.NumberOfMonths)
                    {
                        case 6:
                            return 0.10m * subscription.TotalPrice;
                        case 8:
                            return 0.20m * subscription.TotalPrice;
                        case 12:
                            return 0.25m * subscription.TotalPrice;
                        default:
                            return 0;
                    }
                } // Discounut method

                if (member == null && subscription == null)
                {
                    throw new Exception("Error there is no member neither subscription!");
                }
                decimal discountValue = CalculateDiscount();
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
        public MemberSubscriptionEditVM GetMemberSubscriptionByDetail(string memberCardID, string subscriptionCode)
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
                var sub = new MemberSubscriptionEditVM()
                {
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
        public List<SelectListItem> GetMembersCardID()
        {
            try
            {
                var memberCardID = _ApplicationDbContext.Members.Where(m => m.IsDeleted == false).ToList();
                return memberCardID.Select(mc => new SelectListItem
                {
                    Value = mc.IdCardNumber,
                    Text = $"Full Name:{mc.FirstName + " " + mc.LastName}, Card Id:{mc.IdCardNumber}"
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> GetSubscriberCode()
        {
            try
            {
                var subscriptionsCode = _ApplicationDbContext.Subscription.Where(m => m.IsDeleted == false).ToList();
                return subscriptionsCode.Select(sub => new SelectListItem
                {
                    Value = sub.Code,
                    Text = $"Subscription Code:{sub.Code}, Description:{sub.Description}"
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public void Edit(MemberSubscriptionEditVM memberSubscriptionEditVM)
        {
            try
            {
                if (memberSubscriptionEditVM != null)
                {
                    var existingMemberSubscription = _ApplicationDbContext.MemberSubscriptions
                        .FirstOrDefault(ms => ms.Member.IdCardNumber == memberSubscriptionEditVM.MemberCardId && ms.Subscription.Code == memberSubscriptionEditVM.SubscriptionCode && ms.IsDeleted == false);

                    if (existingMemberSubscription == null)
                    {
                        throw new Exception("ExistingMemberSubscription Member is null");
                    }
                    existingMemberSubscription.RemainingSessions = memberSubscriptionEditVM.RemainingSessions;
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
