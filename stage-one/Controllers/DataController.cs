using hng_uno.Models;
using Microsoft.AspNetCore.Mvc;

namespace hngx_uno.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json")]
public class DataController : ControllerBase
{
    /// <summary>
    /// Returns a response with the required parameters
    /// </summary>
    /// <param name="slack_name">Your Slack name</param>
    /// <param name="track">Your track</param>
    /// <remarks>
    /// Sample request:
    /// <code>
    /// curl -X "GET" "http://localhost:5045/api?slack_name=example_name&amp;track=backend" -H "accept: application/json"
    /// </code>
    ///    
    /// Sample response:
    /// <code>
    /// 
    /// {
    ///     "slack_name": "example_name",
    ///     "current_day": "Monday",
    ///     "utc_time": "2023-08-21T15:04:05Z",
    ///     "track": "backend",
    ///     "github_file_url": "https://github.com/poopsicles/HNGx-dotnet/blob/main/stage-one/Program.cs",
    ///     "github_repo_url": "https://github.com/poopsicles/HNGx-dotnet",
    ///     “status_code”: 200
    /// }   
    /// </code>
    /// </remarks>
    /// <returns>The response in JSON format</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Data GetData(string slack_name, string track)
    {
        return new() { slack_name = slack_name, track = track };
    }
}
