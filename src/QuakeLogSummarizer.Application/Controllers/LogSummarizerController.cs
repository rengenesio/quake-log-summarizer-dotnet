using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.Model;
using QuakeLogSummarizer.Core.Model.Output;
using Swashbuckle.AspNetCore.Annotations;

namespace QuakeLogSummarizer.Application.Controllers
{
    [ApiController]
    [Route("logs")]
    public sealed class LogSummarizerController : ControllerBase
    {
        private readonly LogSummarizer _logSummarizer;

        public LogSummarizerController(LogSummarizer logSummarizer)
        {
            this._logSummarizer = logSummarizer;
        }

        [HttpPost]
        [Route("summarize")]
        public async Task<IActionResult> Summarize(IFormFile file)
        {
            IEnumerable<Game> gameList = await this._logSummarizer.SummarizeAsync(file.OpenReadStream());

            LogSummary summary = new LogSummary(gameList);

            return base.Ok(summary);
        }
    }
}
