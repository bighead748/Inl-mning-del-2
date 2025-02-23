using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.OrderItem;
using dagnyr.api.ViewModels.Product;

namespace dagnyr.api.ViewModels.OrderInformation;

public class PostOrderInformationViewModel
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public IList<OrderItemViewModel> OrderItems { get; set; }
}
