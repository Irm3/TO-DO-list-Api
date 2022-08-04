using System.ComponentModel.DataAnnotations;

namespace TO_DO_list_Api.Models
{
    public class ListTask
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public bool Status { get; set; }
    }
}
