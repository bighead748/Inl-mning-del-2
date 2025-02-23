using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.Entities;

public class OrderItem
{
    
    public int OrderInformationId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double PricePerPiece { get; set; }
    public string ProductName { get; set; }
    public double WeightKg { get; set; }

    public OrderInformation OrderInformation { get; set; }
    public Product Product { get; set; }
    
    
}
