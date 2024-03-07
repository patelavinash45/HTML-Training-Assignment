using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels
{
    public class UserDataModel
    {
        public int AspNetUserId { get; set; }

        public int UserId { get; set; }

        public int AdminId { get; set; }

        public string UserType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
