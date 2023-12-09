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
                var membersSub = _memberSubscription.GetMembersSubscription();
                return View(membersSub);
        }
        public IActionResult ActivateMemberSubscription()
        {
            var memberSb = new MemberSubscriptionVM
            {
                MemberCardList = _memberSubscription.GetMembersCardID(),
                SubscriptionCodeList = _memberSubscription.GetSubscriberCode()
            };
            return View(memberSb);
        }

        [HttpPost]
        public IActionResult ActivateMemberSubscription([FromForm] MemberSubscriptionVM memberSubscriptionVM)
        {
                if (ModelState.IsValid)
                {
                    var member = _context.Members.Where(m => m.IdCardNumber == memberSubscriptionVM.MemberCardID).FirstOrDefault();

                    if (_memberSubscription.MemberSubscriptionExist(member))
                    {
                        ModelState.AddModelError(" ", "Subscription Exists");
                        memberSubscriptionVM.MemberCardList = _memberSubscription.GetMembersCardID();
                        memberSubscriptionVM.SubscriptionCodeList = _memberSubscription.GetSubscriberCode();
                        return View(memberSubscriptionVM);
                    }
                    _memberSubscription.ActivateSubscription(memberSubscriptionVM.MemberCardID, memberSubscriptionVM.SubscribtionCode);
                    return RedirectToAction("GetAllMembersSubscription", "MemberSubscription");
                }
                memberSubscriptionVM.MemberCardList = _memberSubscription.GetMembersCardID();
                memberSubscriptionVM.SubscriptionCodeList = _memberSubscription.GetSubscriberCode();
                return View(memberSubscriptionVM);
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
        public IActionResult Edit([FromForm] MemberSubscriptionEditVM memberSubscriptionEditVM)
        {
                if (ModelState.IsValid)
                {
                _memberSubscription.Edit(memberSubscriptionEditVM);
                return RedirectToAction("GetAllMembersSubscription", "MemberSubscription");
                }
                return View(memberSubscriptionEditVM);
        }


        [HttpPost]
        public IActionResult SoftDelete(string memberCardID, string subscriptionCode)
        {
            _memberSubscription.SoftDelete(memberCardID, subscriptionCode);
            return RedirectToAction("GetAllMembersSubscription");
        }

    }
}
