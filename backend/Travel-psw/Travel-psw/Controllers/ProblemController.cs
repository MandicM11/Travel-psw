namespace Travel_psw.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Travel_psw.Models;
    using Travel_psw.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class ProblemController : ControllerBase
    {
        private readonly ProblemService _problemService;

        public ProblemController(ProblemService problemService)
        {
            _problemService = problemService;
        }

        // Endpoint za prijavljivanje problema
        [HttpPost("report")]
        public async Task<IActionResult> ReportProblem([FromBody] ReportProblemRequest request)
        {
            var problem = await _problemService.ReportProblem(request.TouristId, request.TourId, request.Title, request.Description);
            return Ok(problem);
        }

        // Endpoint za rešavanje problema
        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> ResolveProblem(int id)
        {
            var problem = await _problemService.ResolveProblem(id);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        // Endpoint za slanje problema na reviziju
        [HttpPost("{id}/send-for-review")]
        public async Task<IActionResult> SendProblemForReview(int id)
        {
            var problem = await _problemService.SendProblemForReview(id);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        // Endpoint za odbacivanje problema
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectProblem(int id)
        {
            var problem = await _problemService.RejectProblem(id);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        // Endpoint za dobavljanje trenutnog stanja problema (rekonstrukcija stanja iz događaja)
        [HttpGet("{id}/state")]
        public async Task<IActionResult> GetProblemState(int id)
        {
            var problem = await _problemService.RebuildProblemState(id);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }

        // Endpoint za dobijanje svih problema
        [HttpGet("problems")]
        public async Task<ActionResult<IEnumerable<Problem>>> GetProblems()
        {
            var problems = await _problemService.GetAllProblemsAsync();
            return Ok(problems);
        }

        // Endpoint za ažuriranje statusa problema
        [HttpPut("problems/{id}/status")]
        public async Task<IActionResult> UpdateProblemStatus(int id, [FromBody] ProblemStatusUpdateDto statusUpdate)
        {
            await _problemService.UpdateProblemStatusAsync(id, statusUpdate.Status);
            return NoContent();
        }

        // Endpoint za dobijanje problema po autoru
        [HttpGet("author/{authorId}/problems")]
        public async Task<ActionResult<IEnumerable<Problem>>> GetProblemsByAuthorId(int authorId)
        {
            var problems = await _problemService.GetProblemsByAuthorIdAsync(authorId);
            return Ok(problems);
        }

        // Endpoint za odbacivanje problema (menjanje statusa na UnderReview)
        [HttpPost("{id}/discard")]
        public async Task<IActionResult> DiscardProblem(int id)
        {
            var problem = await _problemService.DiscardProblemAsync(id);
            if (problem == null)
                return NotFound();
            return Ok(problem);
        }
    }

    public class ReportProblemRequest
    {
        public int TouristId { get; set; }
        public int TourId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ProblemStatusUpdateDto
    {
        public ProblemStatus Status { get; set; }
    }
}
