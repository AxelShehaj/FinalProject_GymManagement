using FinalProject_GymManagement.BusinessLayer.Services.Implementations;
using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_GymManagement.Controllers
{
    public class MemberController : Controller
    {

        private readonly IMember _members;
        private readonly ApplicationDbContext _context;


        public MemberController(ApplicationDbContext context, IMember members)
        {
            _context = context;
            _members = members;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(MemberFilterSearchVM members)
        {
            var searchResults = _members.Search(members);
            return View("GetAllMembers", searchResults);
        }

        public IActionResult Create()
        {
            var member = new MemberCreateVM();
            return View(member);
        }

        [HttpPost]
        public IActionResult Create([FromForm] MemberCreateVM memberCreateVM)
        {
            if (ModelState.IsValid)
            {
                _members.CreateMember(memberCreateVM);
                return RedirectToAction("GetAllMembers", "Member");
            }
            return View(memberCreateVM);
        }

        
        public IActionResult GetAllMembers()
        {
                var members = _members.GetMembers();
                return View(members);
        }

        public IActionResult Edit(string cardID)
        {
            var member = _members.GetMemberByCardID(cardID);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        [HttpPost]
        public IActionResult Edit([FromForm] MemberEditVM memberEditVM)
        {
                if (ModelState.IsValid)
                {
                    _members.Edit(memberEditVM);
                    return RedirectToAction("GetAllMembers", "Member");
                }
                return View(memberEditVM);
        }

        [HttpPost]
        public IActionResult SoftDelete(string cardID)
        {
            _members.SoftDelete(cardID);
            return RedirectToAction("GetAllMembers");
        }
    }
}
