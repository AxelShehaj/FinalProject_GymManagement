using FinalProject_GymManagement.Data.Entities;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface IMember
    {
        public List<Member> GetMembers();
        public bool MemberExists(string idCard);
        public void CreateMember(Member member);
        public List<Member> GetMemberByFirstName(string name);
        public List<Member> GetMemberByLastName(string lastName);
        public Member GetMemberByCardID(string cardID);
        public List<Member> GetMemberByEmail(string email);
        public void SoftDelete(string cardID);
    }
}
