using System.ComponentModel.DataAnnotations;

namespace HelloDoc.ViewModels
{
    public class PatientLogin
    {
        [Required(ErrorMessage = "Password is required.")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
            "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }
    }
}
