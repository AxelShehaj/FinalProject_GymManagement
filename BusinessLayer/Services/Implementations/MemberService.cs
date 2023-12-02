using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;

namespace FinalProject_GymManagement.BusinessLayer.Services.Implementations
{
    public class MemberService : IMember
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public MemberService(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
        }


        public List<Member> GetMembers()
        {
            //var members = new List<Member>();
            var members = _ApplicationDbContext.Members.Where(m => m.IsDeleted == false).ToList();
            return members;
        }

        // Check if the member exists
        public bool MemberExists(string idCard)
        {
            return _ApplicationDbContext.Members.Any(m => m.IdCardNumber == idCard);
        }
        // Creating a new member in the database
        public void CreateMember(Member member)
        {

            try
            {
                if (member == null)
                {
                    throw new Exception("There is no Member added");
                }

                member.IsDeleted = false;

                _ApplicationDbContext.Members.Add(member);
                _ApplicationDbContext.SaveChanges();

            }
            catch (Exception)
            {
                throw new Exception("Error in getting the Members details");
            }

        }

        #region Get members by details
        public List<Member> GetMemberByFirstName(string name)
        {
            try
            {
                if (name == null)
                {
                    throw new Exception("We cant find a member, please check the Name, Last Name, Card ID or Email again");
                }
                var gymMember = _ApplicationDbContext.Members.Where(m => m.FirstName == name).ToList();
                var member = new List<Member>();

                foreach (var item in gymMember)
                {
                    member.Add(new Member()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        IdCardNumber = item.IdCardNumber,
                        Birthdate = item.Birthdate,
                        RegistrationDate = item.RegistrationDate,
                        IsDeleted = item.IsDeleted,
                    });
                }

                return member;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting member");
            }
        }

        public List<Member> GetMemberByLastName(string lastName)
        {
            try
            {
                if (lastName == null)
                {
                    throw new Exception("We cant find a member, please check the Name, Last Name, Card ID or Email again");
                }
                var gymMember = _ApplicationDbContext.Members.Where(m => m.LastName == lastName).ToList();
                var member = new List<Member>();

                foreach (var item in gymMember)
                {
                    member.Add(new Member()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        IdCardNumber = item.IdCardNumber,
                        Birthdate = item.Birthdate,
                        RegistrationDate = item.RegistrationDate,
                        IsDeleted = item.IsDeleted,
                    });
                }
                return member;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting member");
            }
        }

        public Member GetMemberByCardID(string cardID)
        {
            try
            {
                if (cardID == null)
                {
                    throw new Exception("We cant find a member, please check the Name, Last Name, Card ID or Email again");
                }
                var gymMember = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == cardID).FirstOrDefault();
                var member = new Member()
                {
                    FirstName = gymMember.FirstName,
                    LastName = gymMember.LastName,
                    Email = gymMember.Email,
                    IdCardNumber = gymMember.IdCardNumber,
                    Birthdate = gymMember.Birthdate,
                    RegistrationDate = gymMember.RegistrationDate,
                    IsDeleted = gymMember.IsDeleted,
                };
                return member;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting member");
            }
        }

        public List<Member> GetMemberByEmail(string email)
        {
            try
            {
                if (email == null)
                {
                    throw new Exception("We cant find a member, please check the Name, Last Name, Card ID or Email again");
                }
                var gymMember = _ApplicationDbContext.Members.Where(m => m.Email == email).ToList();
                var member = new List<Member>();

                foreach (var item in gymMember)
                {
                    member.Add(new Member()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Email = item.Email,
                        IdCardNumber = item.IdCardNumber,
                        Birthdate = item.Birthdate,
                        RegistrationDate = item.RegistrationDate,
                        IsDeleted = item.IsDeleted,
                    });
                }
                return member;
            }
            catch (Exception)
            {
                throw new Exception("Error in getting member");
            }
        }
        #endregion


        public void SoftDelete(string cardID)
        {
            try
            {
                var member = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == cardID).FirstOrDefault();

                if (member != null)
                {
                    member.IsDeleted = true;
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
