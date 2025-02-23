using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Product;

public class ProductPostViewModel
{
    public string ProductName { get; set; }
    public string ItemNumber { get; set; }
    public double PricePerPiece { get; set; }
    public double WeightKg { get; set; }
    public int PackQuantity { get; set; }
    public DateOnly ExpiryDate { get; set; }
    public DateOnly ProductionDate { get; set; }
}
