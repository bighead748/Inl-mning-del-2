using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.ViewModels.Product;

namespace dagnyr.api.InterFaces;

public interface IProductRepository
{
    public Task<IList<AllProductViewModel>> ListAllProducts();
    public Task<ProductGetViewModel> GetProduct(int id);
    public Task<bool> CreateProduct(ProductPostViewModel model);
    public Task<bool> UpdateProductPrice(int id, PatchProductViewModel model);

}
