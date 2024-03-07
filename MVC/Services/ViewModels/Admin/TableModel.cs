namespace Services.ViewModels.Admin
{
    public class TableModel
    {
        public List<TablesData> TableDatas { get; set; }

        public int TotalRequests { get; set; }

        public int PageNo { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public bool IsNextPage { get; set; }

        public bool IsPreviousPage { get; set; }

        public int StartRange { get; set; }

        public int EndRange { get; set; }
    }
}
