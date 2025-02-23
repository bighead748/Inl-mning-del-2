using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.Customer;

namespace dagnyr.api.InterFaces;

public interface ICustomerRepository
{
    public Task<IList<CustomersViewModel>> ListAllCustomers();
    public Task<CustomerViewModel> GetCustomer(int id);
    public Task<bool> CreateCustomer(PostCustomerViewModel model);
    

    //patch, put  bool
    // 
    // delete bool
    // 
    //  ska vara : 
}
