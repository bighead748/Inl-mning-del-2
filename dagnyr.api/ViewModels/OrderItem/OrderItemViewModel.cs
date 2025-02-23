using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.OrderInformation;
using dagnyr.api.ViewModels.Product;

namespace dagnyr.api.ViewModels.OrderItem;

public class OrderItemViewModel
{
    
    public int Quantity { get; set; }
    public double PricePerPiece { get; set; }
    public string ProductName { get; set; }
    
    
    

}
