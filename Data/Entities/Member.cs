using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_GymManagement.Data.Entities
{
    public class Member
    {

        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string IdCardNumber { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate {  get; set; }
        public bool IsDeleted { get; set; }
    }
}
