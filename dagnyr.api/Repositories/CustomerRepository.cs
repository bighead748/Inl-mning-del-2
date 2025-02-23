using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.Helpers;
using dagnyr.api.InterFaces;
using dagnyr.api.ViewModels.Address;
using dagnyr.api.ViewModels.Customer;
using dagnyr.api.ViewModels.OrderInformation;
using dagnyr.api.ViewModels.OrderItem;
using dagnyr.api.ViewModels.Product;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace dagnyr.api.Repositories;

public class CustomerRepository(DataContext context, IAddressRepository addressRepo, IOrderInformationRepository order) : ICustomerRepository
{

    private readonly DataContext _context = context;
    private readonly IAddressRepository _addressRepo = addressRepo;
    private readonly IOrderInformationRepository _order = order;







    public async Task<bool> CreateCustomer(PostCustomerViewModel model)
    {
        try
        {
            if (await _context.Customers.FirstOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToLower().Trim()) != null)
            {
                throw new EDagnyrException("Kunden finns redan");
            }

            var customer = new Customer
            {
                StoreName = model.StoreName,
                ContactPerson = model.ContactPerson,
                Email = model.Email,
                Phone = model.Phone
            };

            await _context.Customers.AddAsync(customer);

            foreach (var a in model.Addresses)
            {
                var address = await _addressRepo.Add(a);

                await _context.CustomerAddresses.AddAsync(new CustomerAddress
                {
                Address = address,
                Customer = customer
                });
            }


            return await _context.SaveChangesAsync() > 0;
        }
        catch (EDagnyrException ex)
        {
            throw new Exception(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
      
    }




    

   

    public async Task<CustomerViewModel> GetCustomer(int id)
    {
        try
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.PostalAddress)
                .Include(c => c.CustomerAddresses)
                    .ThenInclude(c => c.Address)
                    .ThenInclude(c => c.AddressType)
                .Include(c => c.OrderInformations)
                    .ThenInclude(c => c.OrderItems)
                    .ThenInclude(c => c.Product)
                .SingleOrDefaultAsync();

            if (customer == null)
            {
                throw new EDagnyrException($"Hittar ingen kund med id {id}");
            }
            

            var view = new CustomerViewModel
            {
                Id = customer.Id,
                StoreName = customer.StoreName,
                ContactPerson = customer.ContactPerson,
                Email = customer.Email,
                Phone = customer.Phone,
            };

            var address = customer.CustomerAddresses.Select(c => new AddressViewModel
            {
                AddressLine = c.Address.AddressLine,
                PostalCode = c.Address.PostalAddress.PostalCode,
                City = c.Address.PostalAddress.City,
                AddressType = c.Address.AddressType.Value
            });

            view.Addresses = [.. address];

            var orders = customer.OrderInformations.Select(o => new OrderInformationViewModel
            {
               Id = o.OrderInformationId,
               OrderDate = o.OrderDate,
                OrderNumber = o.OrderNumber,
               OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
               {
                ProductName = oi.Product.ProductName,
                Quantity = oi.Quantity,
                PricePerPiece = oi.Product.PricePerPiece
               }).ToList()
            
            }).ToList();

            
            
            view.OrderInformations = [.. orders];

            

            return view;
        }
        catch (Exception ex)
        {
            throw new Exception($"Hoppsan det gick fel {ex.Message}");
        }
    }





    public async Task<IList<CustomersViewModel>> ListAllCustomers()
    {
        

        var response = await _context.Customers.ToListAsync();
        var customers = response.Select(c => new CustomersViewModel
        {
            Id = c.Id,
            StoreName = c.StoreName,
            ContactPerson = c.ContactPerson,
            Email = c.Email,
            Phone = c.Phone
        });

        return [.. customers];
    }

    
}
