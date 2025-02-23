using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.ViewModels.Product;

public class BaseProductViewModel
{
    public int SupplierId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ItemNumber { get; set; }
    public double PricePerPiece { get; set; }
    public double WeightKg { get; set; }
    public int QuantityInPackage { get; set; }
    public DateOnly ExpireDate { get; set; }
    public DateOnly ManufacturingDate { get; set; }
}
