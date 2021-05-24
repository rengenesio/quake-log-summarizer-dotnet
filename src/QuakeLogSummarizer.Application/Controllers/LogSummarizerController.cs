using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuakeLogSummarizer.Application.Controllers.DataContracts.V1;
using QuakeLogSummarizer.Core;
using QuakeLogSummarizer.Core.Model;

namespace QuakeLogSummarizer.Application.Controllers
{
    // TODO: Use a centralized prefix provider to automatically add the 'api/' prefix on all routes.
    /// <summary>
    /// Controller to execute log file operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/logs")]
    public sealed class LogSummarizerController : ControllerBase
    {
        private readonly LogSummarizer _logSummarizer;

        /// <summary />
        public LogSummarizerController(LogSummarizer logSummarizer)
        {
            this._logSummarizer = logSummarizer;
        }

        /// <summary>
        /// Summarizes a server log file.
        /// </summary>
        /// <param name="file">Log file to be summarized.</param>
        /// <returns>A summary for all games in the log file.</returns>
        [HttpPost]
        [Route("summarize")]
        public async Task<LogSummary> Summarize(IFormFile file)
        {
            IEnumerable<Game> gameList = await this._logSummarizer.SummarizeAsync(file.OpenReadStream());

            return new LogSummary(gameList);
        }
    }
}
