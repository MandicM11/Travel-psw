using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel_psw.Models;
using Travel_psw.Services;

namespace Travel_psw.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleRepository _saleRepository;

        public SalesController(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleDTO saleDTO)
        {
            if (saleDTO == null)
                return BadRequest("Sale object is null");

            var sale = new Sale
            {
                TourId = saleDTO.TourId,
                Amount = saleDTO.Amount,
                UserId = saleDTO.UserId,
                SaleDate = saleDTO.SaleDate
            };

            try
            {
                await _saleRepository.AddSaleAsync(sale);
                return CreatedAtAction(nameof(GetSaleById), new { id = sale.Id }, sale);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET api/sales
        [HttpGet]
        public async Task<IActionResult> GetSales([FromQuery] int userId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var sales = await _saleRepository.GetSalesByDateRange(userId, startDate, endDate);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/sales/last-three-months
        [HttpGet("last-three-months")]
        public async Task<IActionResult> GetSalesForLastThreeMonths([FromQuery] int tourId, [FromQuery] DateTime startDate)
        {
            try
            {
                var sales = await _saleRepository.GetSalesForLastThreeMonthsAsync(tourId, startDate);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/sales/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(int id)
        {
            try
            {
                var sales = await _saleRepository.GetSalesByDateRange(id, DateTime.MinValue, DateTime.MaxValue); // Adjust method to get by Id
                if (sales == null)
                {
                    return NotFound();
                }
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
