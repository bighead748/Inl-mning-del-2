using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.Helpers;
using dagnyr.api.InterFaces;
using dagnyr.api.Repositories;
using dagnyr.api.ViewModels;
using dagnyr.api.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductRepository repo) : ControllerBase
{
    private readonly IProductRepository _repo = repo;

    
    [HttpGet()]
    public async Task<ActionResult> ListAllProducts()
    {
        try
        {
          return Ok(new { success = true, data = await _repo.ListAllProducts() });
        }
        catch (Exception ex)
        {
          return NotFound($"Tyvärr hittade vi inget {ex.Message}");
        }
    }

    [HttpGet("{id}")]
  public async Task<ActionResult> FindProduct(int id)
  {
    try
    {
      return Ok(new { success = true, data = await _repo.GetProduct(id) });
    }
    catch (Exception ex)
    {
      return NotFound(new { success = false, message = ex.Message });
    }
  }

  [HttpPost()]
  //[Authorize(Roles = "Admin")]
  public async Task<ActionResult> AddProduct(ProductPostViewModel model)
  {
    try
    {
      var result = await _repo.CreateProduct(model);
      if (result)
      {
        return StatusCode(201);
      }
      else
      {
        return BadRequest();
      }
    }
    catch (Exception ex)
    {
      return BadRequest(new { success = false, message = ex.Message });
    }
  }

  [HttpPatch("{id}/price")]
        public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] PatchProductViewModel model)
        {
            if (model == null || id <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                var result = await _repo.UpdateProductPrice(id, model);
                if (result)
                {
                    return Ok("Product price updated successfully.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the product price.");
                }
            }
            catch (EDagnyrException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

  
    
}
