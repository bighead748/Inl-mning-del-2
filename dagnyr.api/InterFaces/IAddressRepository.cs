using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Entities;
using dagnyr.api.ViewModels.Address;

namespace dagnyr.api.InterFaces;

public interface IAddressRepository
{
    public Task<Address> Add(AddressPostViewModel model);
}
