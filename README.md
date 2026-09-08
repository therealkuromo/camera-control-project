# Camera Control Project

A traffic monitoring and speed violation analysis system built with C#, .NET 7, Entity Framework Core, SQLite, ASP.NET Core Web API, Swagger, HTML, CSS, and JavaScript.

## Overview

This project simulates traffic events detected by multiple speed cameras and provides tools for analyzing speeding violations. The system consists of three main parts:

- A C# console application for generating traffic data and running analysis reports.
- An ASP.NET Core Web API for exposing traffic data and analysis results through HTTP endpoints.
- A browser-based frontend dashboard for viewing the API results.

The console application and API use the same SQLite database, allowing both parts of the system to work with the same traffic data.

## Features

- Generate a configurable traffic dataset.
- Store traffic events in SQLite using Entity Framework Core.
- Detect speeding violations.
- Sort violations by recorded speed.
- Count violations for each camera.
- Find the latest violation for each license plate.
- Find frequent violators with more than five violations.
- Find cameras without recorded violations.
- Find the maximum recorded speed for each camera.
- Find the three plates with the highest number of violations.
- Calculate the percentage of traffic events that are violations.
- Expose analysis results through a REST API.
- Test API endpoints with Swagger UI.
- Display API results through a web dashboard.
- Log application information to the console.
- Centralize traffic-related configuration in `appsettings.json`.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Application development |
| .NET 7 | Runtime and application framework |
| ASP.NET Core Web API | REST API |
| Entity Framework Core 7 | Data access and ORM |
| SQLite | Database |
| Swagger / OpenAPI | API documentation and testing |
| HTML | Frontend structure |
| CSS | Frontend styling and responsive layout |
| JavaScript | Frontend logic and API communication |
| LINQ | Data querying and analysis |
| Git / GitHub | Version control and project hosting |

## Project Structure

```text
camera-control-project/
├── camera control project/
│   ├── Program.cs
│   ├── TrafficMenu.cs
│   ├── TrafficEvent.cs
│   ├── TrafficEventAnalyzer.cs
│   ├── TrafficEventGenerator.cs
│   ├── CameraConfiguration.cs
│   ├── ResultModels.cs
│   ├── TrafficDbContext.cs
│   ├── TrafficSettings.cs
│   ├── AppConfiguration.cs
│   ├── AppLogger.cs
│   ├── appsettings.json
│   └── Migrations/
│
├── camera_control_api/
│   ├── Controllers/
│   │   └── TrafficController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Properties/
│       └── launchSettings.json
│
├── traffic frontend/
│   ├── index.html
│   ├── style.css
│   └── script.js
│
├── database/
│   └── traffic.db
│
├── .gitignore
└── camera control project.sln
```

## Console Application

The console application provides a menu for working with the traffic dataset. It can generate the database and execute the available analysis operations.

The main analysis operations are:

1. List traffic events.
2. List speeding violations.
3. Count violations by camera.
4. Find the last violation for each plate.
5. Find plates with more than five violations.
6. Find cameras without violations.
7. Find the maximum speed for each camera.
8. Find the top three violating plates.
9. Calculate the violation percentage.

The number of generated events, maximum allowed speed, and date range are configured through `appsettings.json`.

## Database

The project uses SQLite with Entity Framework Core.

The database file is located at:

```text
Database/traffic.db
```

The database contains the `TrafficEvents` table, which stores:

- Event ID
- License plate number
- Camera ID
- Recorded speed
- Maximum allowed speed
- Event date and time

Entity Framework Core migrations are included in the project.

## Configuration

Traffic-related settings are stored in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "../../../../Database/traffic.db"
  },
  "TrafficSettings": {
    "EventCount": 500,
    "MaxAllowedSpeed": 121,
    "DaysBack": 30
  }
}
```

The configuration controls:

- `EventCount`: number of traffic events generated.
- `MaxAllowedSpeed`: maximum permitted speed.
- `DaysBack`: number of previous days used when generating event timestamps.

## API

The ASP.NET Core API exposes traffic information under:

```text
/api/Traffic
```

### Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Traffic` | Returns all traffic events. |
| GET | `/api/Traffic/violations` | Returns speeding violations ordered by speed. |
| GET | `/api/Traffic/cameras` | Returns the number of violations detected by each camera. |
| GET | `/api/Traffic/last-violations` | Returns the latest violation for each plate. |
| GET | `/api/Traffic/frequent-violators` | Returns plates with more than five violations. |
| GET | `/api/Traffic/cameras-without-violations` | Returns cameras with no recorded violations. |
| GET | `/api/Traffic/max-speed` | Returns the maximum recorded speed for each camera. |
| GET | `/api/Traffic/top-violators` | Returns the top three violating plates. |
| GET | `/api/Traffic/violation-percentage` | Returns total events, violations, and violation percentage. |

## Swagger

When the API is running in the development environment, Swagger UI is available at:

```text
https://localhost:7163/swagger/index.html
```

Swagger can be used to inspect and test all available API endpoints.

## Frontend

The frontend is located in:

```text
traffic frontend/
```

It provides a dashboard for:

- Viewing total traffic events.
- Viewing the number and rate of violations.
- Running each API analysis endpoint.
- Displaying API responses in tables.
- Handling loading and error states.
- Refreshing traffic information.

The frontend communicates with the development API through:

```text
https://localhost:7163/api/Traffic
```

## How to Run

### Prerequisites

Install the following:

- .NET 7 SDK
- A code editor or IDE such as Visual Studio or Visual Studio Code
- A modern web browser

### Run the Console Application

Open the solution and run the console project:

```bash
dotnet run --project "camera control project"
```

Use the application menu to generate the traffic database and run the analysis operations.

### Run the API

Start the ASP.NET Core API:

```bash
dotnet run --project camera_control_api
```

Then open Swagger:

```text
https://localhost:7163/swagger/index.html
```

### Run the Frontend

Open `traffic frontend/index.html` in a browser while the API is running.

Because the frontend communicates with the local HTTPS API, the development HTTPS certificate may need to be trusted on the machine.

## Data Generation

The default configuration generates 500 traffic events across 10 cameras. Generated events contain randomly selected plate numbers, camera IDs, speeds, and timestamps.

The speed limit is configured as 121, so an event is considered a speeding violation when:

```text
Speed > MaxAllowedSpeed
```

Generating the database replaces the existing generated traffic records so that repeated generation does not cause duplicate ID conflicts.

## Architecture

The project follows a simple layered structure:

```text
Frontend
   │
   │ HTTP / REST
   ▼
ASP.NET Core Web API
   │
   │ Entity Framework Core
   ▼
SQLite Database
   ▲
   │
   │ Entity Framework Core
   │
C# Console Application
```

The console application contains the core traffic domain models, data generation, configuration, database context, logging, and analysis logic. The API project references the core project and exposes database-backed operations through HTTP endpoints.

## Project Status

Version: **1.0.0**

The project has completed its planned implementation and final functional testing. The console application, database layer, API, Swagger documentation, and frontend dashboard have been tested together as an integrated system.

## Learning Objectives

This project was developed as a practical exercise covering:

- C# fundamentals and object-oriented programming
- Collections and LINQ
- Exception handling
- Interfaces and inheritance
- Entity Framework Core
- SQLite database integration
- Configuration management
- Logging
- ASP.NET Core Web API development
- REST API design
- Swagger / OpenAPI
- Frontend-to-backend communication with JavaScript Fetch API
- Git and GitHub workflow

## License

This project is intended as an educational and internship project.
