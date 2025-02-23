using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.Helpers;
using dagnyr.api.InterFaces;
using dagnyr.api.ViewModels.OrderInformation;
using dagnyr.api.ViewModels.OrderItem;
using dagnyr.api.ViewModels.Product;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Repositories;

public class OrderInformationRepository(DataContext context) : IOrderInformationRepository
{
    private readonly DataContext _context = context;








    public async Task<bool> CreateOrder(PostOrderInformationViewModel model)
    {
        try
        {
            var  customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == model.CustomerId);
            if(customer is null)
            {
                throw new EDagnyrException("Kunden finns inte");
            }

            var order = new OrderInformation
            {
                CustomerId = model.CustomerId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in model.OrderItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == item.ProductName);
                if(product is null)
                {
                    throw new EDagnyrException("Produkten finns inte");
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    PricePerPiece = product.PricePerPiece
                };
            }

            _context.OrderInformations.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new EDagnyrException($"Error creating order: {ex.Message}");
        }
    }


    


    public async Task<OrderInformationViewModel> GetOrder(int id)
    {
        try
        {
           var order = await _context.OrderInformations
           .Where(o => o.OrderInformationId == id)
           .Include(o => o.OrderItems)
           .ThenInclude(o => o.Product)
           .SingleOrDefaultAsync(o => o.OrderInformationId == id);

              if(order is null)
              {
                throw new EDagnyrException("Ordern finns inte");
              }

                return new OrderInformationViewModel
                {
                    OrderDate = order.OrderDate,
                    OrderNumber = order.OrderNumber,
                    Id = order.OrderInformationId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        Quantity = oi.Quantity,
                        ProductName = oi.Product.ProductName,
                        PricePerPiece = oi.Product.PricePerPiece
                        
                    }).ToList()
                };
           
        }
        catch (EDagnyrException ex)
        {
            throw new EDagnyrException(ex.Message);
        }

        catch (Exception ex)
        {
            throw new Exception($"Något gick fel vid hämtning av order {ex.Message}");
        }
    }










    public async Task<IList<OrderInformationViewModel>> ListAllOrders()
    {
        var response = await _context.OrderInformations
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Product)
            .ToListAsync();
        
        var orders = response.Select(o => new OrderInformationViewModel
        {
            OrderDate = o.OrderDate,
            OrderNumber = o.OrderNumber,
            Id = o.OrderInformationId,
            OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
            {
                Quantity = oi.Quantity,
                ProductName = oi.Product.ProductName,
                PricePerPiece = oi.Product.PricePerPiece
            }).ToList()
        }).ToList();

        return [.. orders];
    }

    public async Task<OrderInformation> SearchByOrderDate(DateOnly orderDate)
    {
        return await _context.OrderInformations.FirstOrDefaultAsync(o => o.OrderDate == orderDate);
    }


    public async Task<OrderInformation> SearchByOrderNumber(string orderNumber)
    {
        return await _context.OrderInformations.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }
}
