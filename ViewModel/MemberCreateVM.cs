using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace FinalProject_GymManagement.ViewModel
{
    public class MemberCreateVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
