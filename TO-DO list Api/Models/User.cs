namespace TO_DO_list_Api.Models
{
    public class User
    {
        private int User_id { get; set; }
        private string Email { get; set; } = null!;
        private string Password { get; set; } = null!;
        private string? Role { get; set; }


    }
}
