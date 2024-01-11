# AzureToDo

## Developer Machine Setup
Follow the Aspire setup and tooling installation directions [here](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=visual-studio)

## Building the App
- install Nuke.Build via `dotnet tool install Nuke.GlobalTool --global`
- run the build process from repo root: `nuke`
    - This triggers the following steps:
        - Clean
        - Restore
        - Compile
        - Test

## Deploying to Azure
- Authenticate with your Azure account: `azd auth login`
- From Terminal at repo root: 
    - `azd infra synth`
    - `azd up`        
    - do your testing/verification
    - `azd down`
    - *note: if you get weird errors about the resource group not existing or the `up` command isn't doing why you expected, `azd down` should sort you out, if it still doesn't work, you may need to manually create a resource group named `rg-dev` via Azure Portal*
