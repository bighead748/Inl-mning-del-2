using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Account;

public record RegisterUserWithRoleViewModel : RegisterUserViewModel
{
    public string RoleName { get; set; }

}
