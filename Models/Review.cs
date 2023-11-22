using System.ComponentModel.DataAnnotations;

namespace Helpful_Hackers._XBCAD7319._POE.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression("^[1-5]$", ErrorMessage = "Select a number between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Leave a comment")]
        [RegularExpression(@"^[A-Za-z0-9\s\S]+$", ErrorMessage = "You can enter words, numbers, and special characters.")]
        [StringLength(450, ErrorMessage = "The maximum allowed length is 450 characters.")]
        public string Comment { get; set; }

        public Review()
        {

        }
    }
}
