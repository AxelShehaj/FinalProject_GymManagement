﻿using FinalProject_GymManagement.Data.Entities;
using FinalProject_GymManagement.ViewModel;

namespace FinalProject_GymManagement.BusinessLayer.Services.Interfaces
{
    public interface IMember
    {
        public List<MemberGridTableVM> GetMembers();
        public bool MemberExists(string idCard);
        public void CreateMember(MemberCreateVM memberViewModel);
        public MemberEditVM GetMemberByCardID(string cardID);
        public void SoftDelete(string cardID);
        public List<MemberGridTableVM> Search(MemberFilterSearchVM members);
        public void Edit(MemberEditVM memberEditVM);
    }
}
