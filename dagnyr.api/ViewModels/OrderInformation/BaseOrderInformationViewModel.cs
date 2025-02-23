using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.OrderItem;

namespace dagnyr.api.ViewModels.OrderInformation;

public class BaseOrderInformationViewModel
{
    public DateOnly OrderDate { get; set; }
    public string OrderNumber { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; }


    
}
