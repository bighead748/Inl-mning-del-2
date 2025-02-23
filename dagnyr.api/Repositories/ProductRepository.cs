using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.Helpers;
using dagnyr.api.InterFaces;
using dagnyr.api.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Repositories;

public class ProductRepository(DataContext context) : IProductRepository
{
    private readonly DataContext _context = context;

    public async Task<bool> CreateProduct(ProductPostViewModel model)
    {
        try
        {
            if (await _context.Products.FirstOrDefaultAsync(c => c.ItemNumber == model.ItemNumber) != null)
            {
                throw new EDagnyrException("Produkten finns redan");
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                ItemNumber = model.ItemNumber,
                PricePerPiece = model.PricePerPiece,
                PackQuantity = model.PackQuantity,
                ProductionDate = model.ProductionDate,
                ExpiryDate = model.ExpiryDate,
                WeightKg = model.WeightKg
            };

            await _context.Products.AddAsync(product);

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }






    public async Task<ProductGetViewModel> GetProduct(int id)
    {
        try
        {
           var product = await _context.Products
           .Where(p => p.Id == id)
              .Select(p => new ProductGetViewModel
              {
                ProductId = p.Id,
                ProductName = p.ProductName,
                ItemNumber = p.ItemNumber,
                PricePerPiece = p.PricePerPiece,
                QuantityInPackage = p.PackQuantity,
                ExpireDate = p.ExpiryDate.AddDays(30),
                ManufacturingDate = p.ProductionDate.AddDays(1),
                WeightKg = p.WeightKg
              }).SingleOrDefaultAsync();

              if (product is null)
              {
                  throw new Exception($"Produkten {id} finns inte");
              }

              var view = new ProductGetViewModel
              {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ItemNumber = product.ItemNumber,
                    PricePerPiece = product.PricePerPiece,
                    WeightKg = product.WeightKg,
                    QuantityInPackage = product.QuantityInPackage,
                    ExpireDate = product.ExpireDate,
                    ManufacturingDate = product.ManufacturingDate
              };

              return view;


        }
        catch (EDagnyrException ex)
        {
            throw new EDagnyrException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IList<AllProductViewModel>> ListAllProducts()
    {
        var response = await _context.Products.ToListAsync();
        var products = response.Select(p => new AllProductViewModel
        {
            ProductId = p.Id,
            ProductName = p.ProductName,
            ItemNumber = p.ItemNumber,
            PricePerPiece = p.PricePerPiece,
            WeightKg = p.WeightKg
            
        }).ToList();

        return products;
    }

    public async Task<bool> UpdateProductPrice(int id, PatchProductViewModel model)
    {
        try
        {
            var result = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);

            if (result is null)
            {
                throw new EDagnyrException($"Produkten {id} finns inte");
            }

            result.PricePerPiece = model.PricePerPiece;

            _context.Products.Update(result);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
