using System.ComponentModel.DataAnnotations;

namespace Services.ViewModels.Admin
{
    public class AssignAndTransferPopUp
    {
        [Display(Name = "Region")]
        public int SelectedRegion { get; set; }

        [Display(Name = "Physician")]
        public int SelectedPhysician { get; set; }

        public Dictionary<int, String>? Regions { get; set; }

        public int RequestId { get; set; }

        public String? AdminTransferNotes { get; set; }
    }

    public class BlockPopUp
    {
        public int RequestId { get; set; }

        public String? AdminTransferNotes { get; set; }
    }

    public class PhysicianTransferRequest
    {
        public int RequestId { get; set; }

        public String? TransferNotes { get; set; }
    }

    public class CancelPopUp
    {
        public int Reason { get; set; }

        public Dictionary<int, String>? Reasons { get; set; }

        public string? PatientName { get; set; }

        public int RequestId { get; set; }

        public String? AdminTransferNotes { get; set; }
    }

    public class SendLink
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
            "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        public string Mobile { get; set; }
    }

    public class RequestSupport
    {
        public string Message { get; set; }
    }

    public class Agreement
    {
        public int RequestId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? CancelationReson { get; set; }

        public string Number { get; set; }

        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
            "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
