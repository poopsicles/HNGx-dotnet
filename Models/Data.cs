namespace hng_uno.Models;

#pragma warning disable IDE1006
public class Data {
    public string slack_name { get; set; } = "";
    public string current_day { get; set; } = DateTime.UtcNow.DayOfWeek.ToString();
    public string utc_time { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
    public string track { get; set; } = "";
    public string github_file_url { get; set; } = "https://github.com/poopsicles/hngx-uno/blob/main/Program.cs";
    public string github_repo_url { get; set; } = "https://github.com/poopsicles/hngx-uno";
    public int status_code { get; set; } = 200;
}
#pragma warning restore IDE1006
