using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Entities;
using dagnyr.api.ViewModels.OrderInformation;

namespace dagnyr.api.InterFaces;

public interface IOrderInformationRepository
{
    public Task<IList<OrderInformationViewModel>> ListAllOrders();
    public Task<OrderInformationViewModel> GetOrder(int id);
    public Task<bool> CreateOrder(PostOrderInformationViewModel model);
    Task<OrderInformation> SearchByOrderNumber(string orderNumber);
    Task<OrderInformation> SearchByOrderDate(DateOnly orderDate);
}
