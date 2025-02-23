using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dagnyr.api.Entities;

public class Customer
{
    public int Id { get; set; }
    public string StoreName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ContactPerson { get; set; }

    public IList<OrderInformation> OrderInformations { get; set; }
    public IList<CustomerAddress> CustomerAddresses { get; set; }
}
