using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TextController : ControllerBase
    {
        private readonly TextService _textService;

        public TextController(TextService textService)
        {
            _textService = textService;
        }

        [HttpPost("submit")]
        public IActionResult SubmitText([FromBody] string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return BadRequest("Content cannot be empty.");

            if (_textService.ContainsBadWords(content))
                return BadRequest("Content contains prohibited words.");

            var submission = new Submission
            {
                Id = Guid.NewGuid().GetHashCode(), // Generate a unique ID
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            _textService.AddSubmission(submission);
            return Ok(submission);
        }

        [HttpGet("content")]
        public IActionResult GetContent([FromQuery] int start = 0, [FromQuery] int count = 10)
        {
            var submissions = _textService.GetSubmissions(start, count);
            return Ok(submissions);
        }
    }
}