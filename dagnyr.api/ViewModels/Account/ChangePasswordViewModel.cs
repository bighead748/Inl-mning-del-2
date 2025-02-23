using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Account;

public record ChangePasswordViewModel
{
    public string UserName { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
