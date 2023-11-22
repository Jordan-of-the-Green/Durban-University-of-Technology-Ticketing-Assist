using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Helpful_Hackers._XBCAD7319._POE.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "This field is required.")]
        [RegularExpression(@"^\w+@vcconnect\.edu\.za$", ErrorMessage = "Invalid email address. Please use a valid @vcconnect.edu.za email address.")]
        public string Email { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "This field is required.")]
        public string Category { get; set; }

        [DisplayName("Date Ticket")]
        [Required(ErrorMessage = "Enter the issued date.")]
        [DataType(DataType.Date)]
        public DateTime DateTicket { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; }
    }
}

