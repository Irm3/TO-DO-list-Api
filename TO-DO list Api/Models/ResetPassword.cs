using System.ComponentModel.DataAnnotations;

namespace TO_DO_list_Api.Models
{
    public class ResetPassword
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = null!;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
