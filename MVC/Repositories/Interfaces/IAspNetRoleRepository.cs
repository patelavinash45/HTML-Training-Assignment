﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    internal interface IAspNetRoleRepository
    {
        bool checkUserRole(string role);

        bool addUserRole(string role);
    }
}