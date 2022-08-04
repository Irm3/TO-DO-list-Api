using System.ComponentModel.DataAnnotations;

namespace TO_DO_list_Api.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 12,
        ErrorMessage = "Password must be 12 symbols mininium and 50 maximum.")]
        [DataType(DataType.Text)]
        public string Password { get; set; } = null!;
    }
}
