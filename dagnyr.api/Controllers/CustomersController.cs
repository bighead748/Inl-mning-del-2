using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.InterFaces;
using dagnyr.api.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerRepository repo) : ControllerBase
{
    private readonly ICustomerRepository _repo = repo;

    [HttpGet()]
    public async Task<ActionResult> GetAllCustomers()
    {
      try
      {
        return Ok(new { success = true, data = await _repo.ListAllCustomers() });
      }
      catch (Exception ex)
      {
        return NotFound($"Tyvärr hittade vi inget {ex.Message}");
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCustomer(int id)
    {
      try
      {
        return Ok(new { success = true, data = await _repo.GetCustomer(id) });
      }
      catch (Exception ex)
      {
        return NotFound(new { success = false, message = ex.Message });
      }
    }

  [HttpPost()]
  public async Task<ActionResult> AddCustomer(PostCustomerViewModel model)
  {

    try
    {
      var result = await _repo.CreateCustomer(model);
      if(result)
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
      return BadRequest(ex.Message);
    }

  }
}
