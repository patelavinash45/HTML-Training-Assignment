using Repositories.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ViewModels.Admin
{
    public class CancelPopUp
    {
        public int Reason { get; set; }

        public List<CaseTag>? Reasons { get; set; }

        public string? PatientName { get; set; }

        public int RequestId { get; set; }

        public String AdminTransferNotes { get; set; }
    }
}
