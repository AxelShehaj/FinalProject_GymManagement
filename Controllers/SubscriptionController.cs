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
            var subscription = new Subscription();
            return View(subscription);
        }

        [HttpPost]
        public IActionResult CreateSubscription([FromForm] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                if (_subscription.SubscriptionExists(subscription.Code))
                {
                    ModelState.AddModelError("", "This subscription already exists, please check your code number again!");
                    return View(subscription);
                }
                _subscription.CreateSubscription(subscription);
                return RedirectToAction("GetAllSubscriptions", "Subscription");
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
        public IActionResult Edit([FromForm] Subscription subscription)
        {
            try
            {
                var existingSub = _context.Subscription.Where(m => m.Code == subscription.Code).FirstOrDefault();

                if (existingSub == null)
                {
                    return NotFound();
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
                }
                return View(existingSub);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult SoftDelete(string code)
        {
            _subscription.SoftDelete(code);
            return RedirectToAction("GetAllSubscriptions");
        }
    }
}
