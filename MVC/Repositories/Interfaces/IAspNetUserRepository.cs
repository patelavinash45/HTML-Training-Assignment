﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interface
{
    public interface IAspNetUserRepository
    {
        int ValidateUser(String email,String password);
    }
}
