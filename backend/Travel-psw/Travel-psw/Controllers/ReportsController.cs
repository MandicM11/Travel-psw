using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Travel_psw.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("monthly-report")]
        public async Task<IActionResult> GetMonthlyReport([FromQuery] DateTime month)
        {
            // Pozivanje metode koja ne vraća rezultat
            await _reportService.GenerateMonthlyReportAsync(month);

            // Vraća uspešan status kod kao odgovor
            return Ok("Monthly report has been generated successfully.");
        }
    }
}
