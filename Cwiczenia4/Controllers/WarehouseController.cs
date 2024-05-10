using Microsoft.AspNetCore.Mvc;
using Cwiczenia4.Model;
using Cwiczenia4.Services;
using System;

namespace Cwiczenia4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public WarehouseController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order newOrder)
        {
            try
            {
                int IdProductWarehouseFromDb = _ordersService.CreateOrder(newOrder.IdProduct, newOrder.IdWarehouse, newOrder.Amount, newOrder.CreatedAt);
                return Ok(IdProductWarehouseFromDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}