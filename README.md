# AzureToDo - Aspire Proof of Concept
This repo provides a working example of the Aspire Stack.  While this technology is still in Preview status, it offers a quick path to getting developer workstations setup for launching and debugging cloud native applications.  It can even get your application deployed into Azure, allowing us to quickly stand up proof of concept applications in Azure that we can share with stakeholders.

## Application Overview
The following components make up this application:
- ApiService - .NET8 ASP.NET API
- DBMigrations - Postgres Database migrations, powered by FluentMigrator
- Web - ASP.NET Blazor app (server rendered)
- Redis container
- PostgreSQL container
- Aspire Related:
    - AppHost - orchestrator/start-up project that makes all these apps work togther
    - ServiceDefaults - extension methods used by the Aspire infra for things like OpenTelemetry, HealthChecks, etc.
- Build - [Nuke.Build](https://nuke.build) build target definitions 
- *note: first run of the day on dev workstation will fire up Docker Desktop and will sometimes cause the DBMigrations app to fail as the PostgreSQL container hasn't fully started.  A restart or two should clear that error*

## Developer Machine Setup
- Follow the Aspire setup and tooling installation directions [here](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=visual-studio)
    - Docker Desktop
    - Visual Studio 2022 Preview
    - .NET Aspire Workload

## Building the App
- install Nuke.Build via `dotnet tool install Nuke.GlobalTool --global`
- run the build process from repo root: `nuke`
    - This triggers the following steps:
        - Clean
        - Restore
        - Compile
        - Test
     
## Running the App
- Ensure AzureToDo.AppHost is set as the startup project
- hit F5 in Visual Studio 2022

## Deploying to Azure
- Authenticate with your Azure account: `azd auth login`
- From Terminal at repo root: 
    - `azd infra synth`
    - `azd up`        
    - do your testing/verification
    - `azd down`
    - *note: if you get weird errors about the resource group not existing or the `up` command isn't doing why you expected, `azd down` should sort you out, if it still doesn't work, you may need to manually create a resource group named `rg-dev` via Azure Portal*
