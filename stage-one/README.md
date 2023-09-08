# Stage One

Render link is at `https://hngx-stage-one-c.onrender.com`

Run it like:

```sh
$ curl -X "GET" "https://hngx-stage-one-c.onrender.com?slack_name=example_name&amp;track=backend" -H "accept: application/json"

{
    "slack_name": "example_name",
    "current_day": "Monday",
    "utc_time": "2023-08-21T15:04:05Z",
    "track": "backend",
    "github_file_url": "https://github.com/poopsicles/HNGx-dotnet/blob/main/stage-one/Program.cs",
    "github_repo_url": "https://github.com/poopsicles/HNGx-dotnet",
    "status_code": 200
}   
```

## Objectives
Create and host an endpoint using any programming language of your choice.

The endpoint should take two GET request query parameters and return specific information in JSON format.
The information required includes:

- Slack name
- Current day of the week
- Current UTC time (with validation of +/-2 minutes)
- Track
- The GitHub URL of the file being run
- The GitHub URL of the full source code.
- A Status Code of Success

For example:

```sh
curl -H "GET" http://example.com/api?slack_name=example_name&track=backend -H "accept: application/json"
```

```json
{
    "slack_name": "example_name",
    "current_day": "Monday",
    "utc_time": "2023-08-21T15:04:05Z",
    "track": "backend",
    "github_file_url": "https://github.com/username/repo/blob/main/file_name.ext",
    "github_repo_url": "https://github.com/username/repo",
    "status_code": "200"
}
```

## Build locally

- Clone the repository:

```sh
$ git clone https://github.com/poopsicles/HNGx-dotnet.git
$ cd HNGx-dotnet/stage-one
```

- Build the image:

```sh
$ docker build --pull -t hngx-uno/dotnet
```

- Run the container:

```sh
$ docker run --rm -it -p 8000:80 hngx-uno/dotnet
```

It will be running at `http://localhost:8000`