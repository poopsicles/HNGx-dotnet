namespace hngx_uno.Models;

#pragma warning disable IDE1006
public class Data {
    public string slack_name { get; set; } = "";
    public string current_day { get; set; } = DateTime.UtcNow.DayOfWeek.ToString();
    public string utc_time { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
    public string track { get; set; } = "";
    public string github_file_url { get; set; } = new Uri("https://github.com/poopsicles/HNGx-dotnet/blob/main/stage-one/Program.cs").ToString();
    public string github_repo_url { get; set; } = new Uri("https://github.com/poopsicles/HNGx-dotnet").ToString();
    public int status_code { get; set; } = StatusCodes.Status200OK;
}
#pragma warning restore IDE1006
