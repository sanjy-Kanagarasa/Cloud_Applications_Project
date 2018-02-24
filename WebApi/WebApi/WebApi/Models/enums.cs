using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public enum UserRoleTypes
    {
        CUSTOMER = 0,
        DRIVER = 1,
        ADMIN = 2,
        NOTFOUND = 3
    }
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Delivered = 2,
        Canceled = 3
    }
}
