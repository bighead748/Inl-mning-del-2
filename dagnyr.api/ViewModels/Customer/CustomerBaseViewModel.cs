using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Customer;

public class CustomerBaseViewModel
{
    public string StoreName { get; set; }
    public string ContactPerson { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
