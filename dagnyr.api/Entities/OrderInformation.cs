using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.Entities;

public record OrderInformation
{
    
    public int OrderInformationId { get; set; }
    public string OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public DateOnly OrderDate { get; set; }
    
    
    public IList<OrderItem> OrderItems { get; set; }
    public Customer Customer { get; set; }


    
}
