using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace dagnyr.api.Entities;

public class User: IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
