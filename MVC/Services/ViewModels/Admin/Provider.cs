namespace Services.ViewModels.Admin
{
    public class Provider
    {
        public List<ProviderTable> providers { get; set; }

        public Dictionary<int, string> Regions { get; set; }

        public ContactProvider ContactProvider { get; set; }
    }

    public class ProviderTable
    {
        public int providerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool Notification { get; set; }

        public string Role { get; set; }

        public bool OnCallStatus { get; set; }

        public String Status { get; set; }
    }

    public class ContactProvider
    {
        public int providerId { get; set;}

        public string Message { get; set; }

        public bool email { get; set; } = false;

        public bool sms { get; set; } = false;
    }
}
