using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Helpful_Hackers._XBCAD7319._POE.Models
{
    public class UserModel
    {
        [Key]
        public int id { get; set; }

        [EmailAddress]
        [DisplayName("Email")]
        [Required]
        [RegularExpression(@"^\w+@vcconnect\.edu\.za$", ErrorMessage = "Invalid email address. Please use a valid @vcconnect.edu.za email address.")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? UserName { get; internal set; }
    }
}

