using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_GymManagement.Controllers
{
    public class SubscriptionController : Controller
    {

        private readonly ISubscription _subscription;
        private readonly ApplicationDbContext _context;

        public IActionResult GetAllSubscriptions()
        {
            try
            {
                var subs = _subscription.GetSubscriptions();
                return View(subs);
            }

            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        public SubscriptionController(ApplicationDbContext context, ISubscription subscription)
        {
            _context = context;
            _subscription = subscription;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateSubscription()
        {
            var subscription = new Subscriptions();
            return View(subscription);
        }

        [HttpPost]
        public IActionResult CreateSubscription([Bind("Code,Description,NumberOfMonths,WeekFrequency,TotalNumberOfSessions,TotalPrice")] Subscriptions subscription)
        {
            if (ModelState.IsValid)
            {
                if (_subscription.SubscriptionExists(subscription.Code))
                {
                    ModelState.AddModelError("", "This subscription already exists, please check your code number again!");
                    return View(subscription);
                }
                _subscription.CreateMemberSubscription(subscription);
                return RedirectToAction("GetAllMembers", "Member");
            }
            return View(subscription);
        }

        public IActionResult Edit(string code)
        {

            var subscription = _subscription.GetSubscriptionByCode(code);

            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] Subscriptions subscription/*string code, [Bind("Description,NumberOfMonths,WeekFrequency,TotalNumberOfSessions,TotalPrice,IsDeleted")] Subscriptions subscription*/)
        {
            try
            {
                var existingSub = _context.Subscriptions.Where(m => m.Code == subscription.Code).FirstOrDefault();

                if (existingSub == null)
                {
                    //return NotFound();
                    return Json(new { success = false, message = "Subscription not found" });
                }

                if (ModelState.IsValid)
                {
                    existingSub.Description = subscription.Description;
                    existingSub.NumberOfMonths = subscription.NumberOfMonths;
                    existingSub.WeekFrequency = subscription.WeekFrequency;
                    existingSub.TotalNumberOfSessions = subscription.TotalNumberOfSessions;
                    existingSub.TotalPrice = subscription.TotalPrice;
                    existingSub.IsDeleted = subscription.IsDeleted;

                    _context.SaveChanges();

                    return RedirectToAction("GetAllMembers", "Member");
                    //return Json(RedirectToAction("GetAllMembers", "Member"));
                }
                return View(existingSub);
            }
            catch (Exception ex)
            {
                //ViewBag.errorMessage = ex.Message;
                //return View("Error");
                return Json(new { success = false, message = "Error saving changes" });
            }
        }

        [HttpPost]
        public IActionResult SoftDelete(string code)
        {
            _subscription.SoftDelete(code);
            return RedirectToAction("GetAllMembers");
        }
    }
}
