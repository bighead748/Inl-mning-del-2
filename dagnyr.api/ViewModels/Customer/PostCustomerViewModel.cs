using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.Address;
using dagnyr.api.ViewModels.OrderInformation;
using dagnyr.api.ViewModels.Product;

namespace dagnyr.api.ViewModels.Customer;

public class PostCustomerViewModel : CustomerBaseViewModel
{
    
    public IList<AddressPostViewModel> Addresses { get; set; }
    public IList<OrderInformationViewModel> OrderInformations { get; set; }
}
