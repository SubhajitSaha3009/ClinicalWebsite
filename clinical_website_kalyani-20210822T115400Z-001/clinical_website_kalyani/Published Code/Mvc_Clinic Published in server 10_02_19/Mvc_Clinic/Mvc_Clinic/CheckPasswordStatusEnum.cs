using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_Clinic
{
    public enum CheckPasswordStatusEnum
    {
        Updated, NewUser, NotInRole, Deactivated, WrongPassword
    }
}