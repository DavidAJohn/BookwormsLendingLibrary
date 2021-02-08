using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", 
            ErrorMessage= "Password must be 6 characters or more, with at least 1 uppercase, 1 lowercase, 1 number and 1 non-alphanumeric character")]
        public string Password { get; set; }
    }
}