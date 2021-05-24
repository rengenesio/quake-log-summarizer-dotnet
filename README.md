# Quake Log Summarizer - .NET

This repository contains a multiplatform C# console application that parses a [Quake III Arena](https://github.com/id-Software/Quake-III-Arena) server log to summarize matches statistics.

## Match Summary

The application reads a server log and prints a [JSON](https://www.json.org/) object that summarizes each match (not an JSON array contaning all matches summaries). This is a sample:

```json
{
  "game_2": {
    "players": [
      "Isgalamido"
    ],
    "total_kills": "0",
    "kills": {
      "Isgalamido": "0"
    }
  }
}
{
  "game_3": {
    "players": [
      "Isgalamido",
      "Dono da Bola"
    ],
    "total_kills": "11",
    "kills": {
      "Isgalamido": "-7",
      "Dono da Bola": "0"
    }
  }
}
```

| Field         | Description                                                                               |
|---------------|-------------------------------------------------------------------------------------------|
| `game_{index}`| A match name (generated by the application). Uses an one-based index                           |
| `players`     | All players' name that joined the match                                                   |
| `total_kills` | Total kills in the match (includes world's kills and suicides)                            |
| `kills`       | The match final score. The keys are the players' names and the values are the final score |

### Notes

1. A player may change his name during a match. This parser will show the output based on the last player's name inside a match (some players may ingress a match with a default name `UnnamedPlayer`).
2. Suicides are included on match total kills.
3. Suicides are ignored on player's score (neither increases nor decreases his kills).
4. Some matches may contain truncated log data (maybe due to a crash on the server). This application will still summarize these matches.

## Usage

### Dependencies

This application depends on:
- [.NET 5.0](https://dotnet.microsoft.com/download)
  - SDK (to build and run tests)
  - Runtime (to run application)

This is a multiplatform application. The following commands may be used on Linux or Windows operating systems.

### Building

Assuming the .NET SDK is installed and the `dotnet` CLI is included in user's path:

```bash
$ dotnet build
```

### Running Tests

```bash
$ dotnet test
```

### Running Application

```bash
$ dotnet build/QuakeLogSummarizer.Application/Debug/QuakeLogSummarizer.Application.dll LOG_FILE
```

The application parses `LOG_FILE` and prints all matches summaries in JSON format (one JSON per match).

## Project Structure

This structure is organized as following:

- :file_folder: **build** - Directory created when building the source code. Stores all executables and temporary build objects.
- :file_folder: **src** - Contains the executable source code projects.
    - :file_folder: **QuakeLogSummarizer.Application** - Project containing the console application and dependency injection code.
    - :file_folder: **QuakeLogSummarizer.Core** - Project containing the application's core processing rules.
    - :file_folder: **QuakeLogSummarizer.Infrastructure** - Project containing the integrations with external resources (e.g: filesystem, REST API's).
- :file_folder: **test** - Contains the executable source code projects.
    - :file_folder: **QuakeLogSummarizer.IntegrationTest** - Project containing tests that interacts with a external resources.
    - :file_folder: **QuakeLogSummarizer.UnitTest** - Project containing tests that runs only with in memory objects.
- :page_facing_up: **Directory.Build.Props** - File containing common build specifications. Avoids duplicated code on `csproj` files.
- :page_facing_up: **QuakeLogSummarizer.sln** - Solution file.


## TODO List

The following may be future enhancements:

- Improve application JSON response to include all player's name inside a match.
- Warn when a match log is truncated.
- Change match summary object to improve integrations (read the current object may be painful).
- Extract more useful information from log messages.

## Contributing

TODO.
(Feel free to open issues and submit pull requests)
