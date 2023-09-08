namespace hng_uno.Models;

#pragma warning disable IDE1006
public class Data {
    public string slack_name { get; set; } = "";
    public string current_day { get; set; } = DateTime.UtcNow.DayOfWeek.ToString();
    public DateTime utc_time { get; set; } = DateTime.UtcNow;
    public string track { get; set; } = "";
    public string github_file_url { get; set; } = ""; //! TODO: Change to url
    public string github_repo_url { get; set; } = "";
    public int status_code { get; set; } = 200;
}
#pragma warning restore IDE1006
