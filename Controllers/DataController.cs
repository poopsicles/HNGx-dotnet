using hng_uno.Models;
using Microsoft.AspNetCore.Mvc;

namespace hng_uno.Controllers;

[ApiController]
[Route("[controller]")]
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
    /// curl -X "GET" \
    ///     "http://localhost:5045/Data?slack_name=tet&amp;track=tet" \
    ///     -H 'accept: application/json'
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
    ///     "github_file_url": "https://github.com/username/repo/blob/main/file_name.ext",
    ///     "github_repo_url": "https://github.com/username/repo",
    ///     “status_code”: 200
    /// }   
    /// </code>
    /// </remarks>
    /// <returns>The response in json format</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public List<Data> GetData(string slack_name, string track)
    {
        return new List<Data>() { new() { slack_name = slack_name, track = track } };
    }
}
