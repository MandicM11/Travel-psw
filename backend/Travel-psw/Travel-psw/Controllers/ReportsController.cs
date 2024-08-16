using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetMonthlyReport(DateTime month)
        {
            var report = await _reportService.GenerateMonthlyReport(month);
            return Ok(report);
        }
    }

}
