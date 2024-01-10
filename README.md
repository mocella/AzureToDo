# AzureToDo

## Developer Machine Setup
Follow the Aspire setup and tooling installation directions [here](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=visual-studio)

## Deploying to Azure
- Authenticate with your Azure account: azd auth login
- From Terminal at solution root: 
    - `azd infra synth`
    - `azd up`        
    - do your testing/verification
    - `azd down`
    - *note: if you get weird errors about the resource group not existing or the `up` command isn't doing why you expected, `azd down` should sort you out, if it still doesn't work, you may need to manually create a resource group named `rg-dev` via Azure Portal*
