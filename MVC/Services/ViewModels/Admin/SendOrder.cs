namespace Services.ViewModels.Admin
{
    public class SendOrder
    {
        public int RequestId { get; set; }

        public int NoOfRefill { get; set; }

        public Dictionary<int, String>? Professions { get; set; }

        public Dictionary<int, Tuple<int,String>>? Businesses { get; set; }

        public int SelectedProfession { get; set; }

        public int SelectedBusiness { get; set; }

        public String Contact { get; set; }

        public string Email { get; set; }

        public string FaxNumber { get; set; }

        public string OrderDetails { get; set; }

    }
}
