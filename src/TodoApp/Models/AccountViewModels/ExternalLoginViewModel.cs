using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
