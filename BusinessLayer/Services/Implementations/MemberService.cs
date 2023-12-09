using FinalProject_GymManagement.BusinessLayer.Services.Interfaces;
using FinalProject_GymManagement.Data;
using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_GymManagement.BusinessLayer.Services.Implementations
{
    public class MemberService : IMember
    {
        private readonly ApplicationDbContext _ApplicationDbContext;
        public MemberService(ApplicationDbContext ApplicationDbContext)
        {
            _ApplicationDbContext = ApplicationDbContext;
        }


        private string CreateString(int stringLength)
        {
            Random rd = new Random();
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public List<MemberGridTableVM> GetMembers()
        {
            var members = _ApplicationDbContext.Members.Where(m => m.IsDeleted == false).ToList();
            var membersVM = members.Select(member => new MemberGridTableVM
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                Birthdate = member.Birthdate,
                IdCardNumber = member.IdCardNumber,
                Email = member.Email,
                RegistrationDate = member.RegistrationDate,
            }).ToList();
            return membersVM;
        }

        // Check if the member exists
        public bool MemberExists(string idCard)
        {
            try
            {
                return _ApplicationDbContext.Members.Any(m => m.IdCardNumber == idCard);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        // Creating a new member in the database
        public void CreateMember(MemberCreateVM memberViewModel)
        {
            try
            {
                if (memberViewModel == null)
                {
                    throw new Exception("There is no Member added");
                }

                var member = new Member
                {
                    FirstName = memberViewModel.FirstName,
                    LastName = memberViewModel.LastName,
                    Birthdate = memberViewModel.Birthdate,
                    Email = memberViewModel.Email,
                    RegistrationDate = DateTime.Now,
                    IdCardNumber = CreateString(6),
                    IsDeleted = false
                };

                if (!MemberExists(member.IdCardNumber))
                {
                    _ApplicationDbContext.Members.Add(member);
                    _ApplicationDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
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

        public MemberEditVM GetMemberByCardID(string cardID)
        {
            try
            {
                if (cardID == null)
                {
                    throw new Exception("We cant find a member, please check the Name, Last Name, Card ID or Email again");
                }
                var gymMember = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == cardID).FirstOrDefault();
                var member = new MemberEditVM()
                {
                    FirstName = gymMember.FirstName,
                    LastName = gymMember.LastName,
                    Email = gymMember.Email,
                    Birthdate = gymMember.Birthdate,
                    IdCardNumber = gymMember.IdCardNumber,
                };
                return member;
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<MemberGridTableVM> Search(MemberFilterSearchVM members)
        {
            try
            {
                var query = _ApplicationDbContext.Members.AsQueryable();
                if (members.IdCardNumber != null)
                {
                    query = query.Where(m => m.IdCardNumber == members.IdCardNumber);
                }
                if (!string.IsNullOrEmpty(members.FirstName))
                {
                    query = query.Where(m => m.FirstName.Contains(members.FirstName));
                }
                if (!string.IsNullOrEmpty(members.LastName))
                {
                    query = query.Where(m => m.LastName.Contains(members.LastName));
                }
                if (!string.IsNullOrEmpty(members.Email))
                {
                    query = query.Where(m => m.Email.Contains(members.Email));
                }
                var result = query.Select(m => new MemberGridTableVM
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    Birthdate = m.Birthdate,
                    RegistrationDate = m.RegistrationDate,
                    IdCardNumber = m.IdCardNumber
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Edit(MemberEditVM memberEditVM)
        {
            try
            {
                if (memberEditVM != null)
                {
                    var existingMember = _ApplicationDbContext.Members.Where(m => m.IdCardNumber == memberEditVM.IdCardNumber).FirstOrDefault();
                    if (existingMember == null)
                    {
                        throw new Exception("Existing Member is null");
                    }
                    existingMember.FirstName = memberEditVM.FirstName;
                    existingMember.LastName = memberEditVM.LastName;
                    existingMember.Email = memberEditVM.Email;
                    existingMember.Birthdate = memberEditVM.Birthdate;

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
