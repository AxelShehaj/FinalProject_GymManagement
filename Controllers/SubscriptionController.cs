using FinalProject_GymManagement.BusinessLayer.Services.Implementations;
using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_GymManagement.Controllers
{
    public class SubscriptionController : Controller
    {

        private readonly ISubscription _subscription;
        private readonly ApplicationDbContext _context;

        public IActionResult GetAllSubscriptions()
        {
                var subs = _subscription.GetSubscriptions();
                return View(subs);
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
        public IActionResult CreateSubscription([FromForm] SubscriptionCreateVM subscription)
        {
             if (ModelState.IsValid)
             {
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
        public IActionResult Edit([FromForm] SubscriptionEditVM subscription)
        {
                if (ModelState.IsValid)
                {
                    _subscription.Edit(subscription);
                    return RedirectToAction("GetAllSubscriptions", "Subscription");
                }
                return View(subscription);
        }

        [HttpPost]
        public IActionResult SoftDelete(string code)
        {
                _subscription.SoftDelete(code);
                return RedirectToAction("GetAllSubscriptions");
        }

        [HttpGet]
        public IActionResult Search(SubscriptionFilterSearchVM subscriptions)
        {
            var searchResults = _subscription.Search(subscriptions);
            return View("GetAllSubscriptions", searchResults);
        }
    }
}
