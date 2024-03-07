using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels
{
    public class SetNewPassword
    {
        public bool IsValidLink { get; set; }

        public String AspNetUserId { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        //[RegularExpression("(?=^.{8,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{\":;'?/>.<,])(?!.*\\s).*$",
        //    ErrorMessage = "Password must be 8-10 characters long with at least one numeric,one upper case character and one special character.")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Conform Password is required.")]
        //[RegularExpression("(?=^.{8,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{\":;'?/>.<,])(?!.*\\s).*$",
        //    ErrorMessage = "Password must be 8-10 characters long with at least one numeric,one upper case character and one special character.")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string? ConformPassword { get; set; }

    }
}
