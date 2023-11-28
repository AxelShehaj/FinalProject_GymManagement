using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
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

        public IActionResult Create()
        {
            var member = new Member();
            return View(member);
        }

        [HttpPost]
        public IActionResult Create([Bind("FirstName,LastName,Birthdate,IdCardNumber,Email,RegistrationDate")] Member member)
        {
            if (ModelState.IsValid)
            {
                if (_members.MemberExists(member.IdCardNumber))
                {
                    ModelState.AddModelError("","The member already exists, please check your email or Id card number!");
                    return View(member);
                }
                _members.CreateMember(member);
                return RedirectToAction("GetAllMembers", "Member");
            }
            return View(member);
        }

        
        public IActionResult GetAllMembers()
        {
            try
            {
                var members = _members.GetMembers();
                return View(members);
            }

            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }
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
        public async Task<IActionResult> Edit(string cardID, [Bind("FirstName,LastName,Birthdate,IdCardNumber,Email,RegistrationDate,IsDeleted")] Member member)
        {
            try
            {
                var existingMember = _context.Members.Where(m => m.IdCardNumber == cardID).FirstOrDefault();

                if (existingMember == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    existingMember.FirstName = member.FirstName;
                    existingMember.LastName = member.LastName;
                    existingMember.Email = member.Email;
                    existingMember.Birthdate = member.Birthdate;
                    existingMember.IdCardNumber = member.IdCardNumber;
                    existingMember.IsDeleted = member.IsDeleted;

                    _context.SaveChanges();

                    return RedirectToAction("GetAllMembers", "Member");
                }
                return View(member);
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Error");
            }

        }
        [HttpPost]
        public IActionResult SoftDelete(string cardID)
        {
            _members.SoftDelete(cardID);
            return RedirectToAction("GetAllMembers");
        }
    }
}
