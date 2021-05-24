# Quake Log Summarizer - .NET

This repository contains a C# console application that parses a [Quake III Arena](https://github.com/id-Software/Quake-III-Arena) server log to summarize matches statistics.

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

### Building

Assuming the .NET SDK is installed and the `dotnet` CLI is included in user's path:

```bash
$ dotnet build
```

### Running Tests

```bash
$ dotnet test
```

## Running Application

TODO.

The application parses `LOG_FILE` and prints all matches summaries in JSON format (one JSON per match).

## TODO List

The following may be future enhancements:

- Improve application JSON response to include all player's name inside a match.
- Warn when a match log is truncated.
- Change match summary object to improve integrations (read the current object may be painful).
- Extract more useful information from log messages.

## Contributing

TODO.
(Feel free to open issues and submit pull requests)
