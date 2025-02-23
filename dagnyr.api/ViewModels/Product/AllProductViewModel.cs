using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Product;

public class AllProductViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ItemNumber { get; set; }
    public double PricePerPiece { get; set; }
    public double WeightKg { get; set; }
}
