using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FinalProject_GymManagement.Controllers
{
    public class MemberSubscriptionController : Controller
    {
        private readonly IMemberSubscription _memberSubscription;
        private readonly ApplicationDbContext _context;

        public MemberSubscriptionController(ApplicationDbContext context, IMemberSubscription memberSubscription)
        {
            _context = context;
            _memberSubscription = memberSubscription;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllMembersSubscription()
        {
            try
            {
                var membersSub = _memberSubscription.GetMembersSubscription();
                return View(membersSub);
            }

            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }
        public IActionResult ActivateMemberSubscription()
        {
            var memberSb = new MemberSubscriptionVM();
            return View(memberSb);
        }

        [HttpPost]
        public IActionResult ActivateMemberSubscription([FromForm] MemberSubscriptionVM memberSubscriptionVM)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var member = _context.Members.Where(m => m.IdCardNumber == memberSubscriptionVM.MemberCardID).FirstOrDefault();

                    if (_memberSubscription.MemberSubscriptionExist(member))
                    {
                        ModelState.AddModelError(" ", "Subscription Exists");
                        return View(memberSubscriptionVM);
                    }
                    _memberSubscription.ActivateSubscription(memberSubscriptionVM.MemberCardID, memberSubscriptionVM.SubscribtionCode);
                    return RedirectToAction("GetAllMembers", "Member");
                }
                return View(memberSubscriptionVM);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
            
        }


        public IActionResult Edit(string memberCardID, string subscriptionCode)
        {

            var memberSub = _memberSubscription.GetMemberSubscriptionByDetail(memberCardID, subscriptionCode);

            if (memberSub == null)
            {
                return NotFound();
            }

            return View(memberSub);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] MemberSubscriptionTableVM memberSubscriptionTableVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMemberSubscription = _context.MemberSubscriptions
                        .FirstOrDefault(ms => ms.Member.IdCardNumber == memberSubscriptionTableVM.MemberCardId && ms.Subscription.Code == memberSubscriptionTableVM.SubscriptionCode);

                    if (existingMemberSubscription == null)
                    {
                        return NotFound();
                    }
                    existingMemberSubscription.OriginalPrice = memberSubscriptionTableVM.OriginalPrice;
                    existingMemberSubscription.DiscountValue = memberSubscriptionTableVM.DiscountValue;
                    existingMemberSubscription.PaidPrice = memberSubscriptionTableVM.PaidPrice;
                    existingMemberSubscription.StartDate = memberSubscriptionTableVM.StartDate;
                    existingMemberSubscription.EndDate = memberSubscriptionTableVM.EndDate;
                    existingMemberSubscription.RemainingSessions = memberSubscriptionTableVM.RemainingSessions;

                    _context.SaveChanges();

                    return RedirectToAction("GetAllMembersSubscription", "Member");
                }
                return View(memberSubscriptionTableVM);

            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }


        [HttpPost]
        public IActionResult SoftDelete(string memberCardID, string subscriptionCode)
        {
            _memberSubscription.SoftDelete(memberCardID, subscriptionCode);
            return RedirectToAction("GetAllMembersSubscription");
        }

    }
}
