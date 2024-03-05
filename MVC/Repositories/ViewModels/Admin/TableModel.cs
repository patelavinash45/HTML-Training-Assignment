using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.Admin
{
    public class TableModel
    {
        public List<TablesData> TableDatas { get; set; }

        public int TotalRequests { get; set; }

        public int PageNo { get; set; }

        public bool IsNextPage { get; set; }

        public bool IsPreviousPage { get; set; }

        public int StartRange { get; set; }

        public int EndRange { get; set; }
    }
}
