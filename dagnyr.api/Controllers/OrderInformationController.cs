using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dagnyr.api.Data;
using dagnyr.api.Entities;
using dagnyr.api.InterFaces;
using dagnyr.api.Repositories;
using dagnyr.api.ViewModels.OrderInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dagnyr.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderInformationController(IOrderInformationRepository repo) : ControllerBase
{
    private readonly IOrderInformationRepository _repo = repo;

    [HttpGet()]
    public async Task<ActionResult> ListAllOrders()
    {
        try
        {
            return Ok(new { success = true, data = await _repo.ListAllOrders() });
        }
        catch (Exception ex)
        {
            return NotFound($"Tyvärr hittade vi inget {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindOrder(int id)
    {
        try
        {
            return Ok(new { success = true, data = await _repo.GetOrder(id) });
        }
        catch (Exception ex)
        {
            return NotFound(new { success = false, message = ex.Message });
        }
    }

    [HttpPost()]
    public async Task<ActionResult> AddOrder(PostOrderInformationViewModel model)
    {
        try
        {
            var result = await _repo.CreateOrder(model);
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

    [HttpGet("ordernumber")]
    public async Task<ActionResult> SearchOrder(string orderNumber)
    {
        try
        {
            var order = await _repo.SearchByOrderNumber(orderNumber);
            if (order != null)
            {
                return Ok(new { success = true, data = order });
            }
            else
            {
                return NotFound(new { success = false, message = "Order not found" });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("orderdate")]
    public async Task<ActionResult> SearchOrder(DateOnly orderDate)
    {
        try
        {
            var order = await _repo.SearchByOrderDate(orderDate);
            if (order != null)
            {
                return Ok(new { success = true, data = order });
            }
            else
            {
                return NotFound(new { success = false, message = "Order not found" });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    
}
