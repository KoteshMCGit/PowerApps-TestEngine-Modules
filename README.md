# PowerApps TestEngine Modules

This repository is an experimental project that demonstrates sample Extensions modules for the Power Apps Test Engine that can be used to extend via custom Power Fx and Networking Requests.

## Getting Started

1. Clone the repository

```pwsh
git clone --recurse-submodules https://github.com/Grant-Archibald-MS/PowerApps-TestEngine-Modules.git
```

2. From the main folder you cloned the repository run the following commands to build the solutions

```pwsh
cd src\PowerApps-TestEngine-Modules
dotnet build
```

3. From the folder you clone the repository install Playwright Install using the following commands

```pwsh
cd PowerApps-TestEngine\bin\Debug\PowerAppsTestEngine
& .\playwright.ps1 install
```

4. Before running any sample ensure login credentials and in PowerAppsTestEngine from inside the PowerApps-TestEngine-Modules folder

```pwsh
$env:user1Email = "test@contoso.com"
$env:user1Password = "XXXXXXXXXXXXXXXXXXXXXXX"
cd PowerApps-TestEngine\bin\Debug\PowerAppsTestEngine
```

## Run a sample

Get the values for your environment and tenant id from the [Power Apps Portal](http://make.powerapps.com). See [Get the session ID for Power Apps](https://learn.microsoft.com/power-apps/maker/canvas-apps/get-sessionid#get-the-session-id-for-power-apps-makepowerappscom) for more information.

### Pause

From the main folder of the cloned repository. To run this sample the button clicker solution must be loaded into the environment you want to test.

```pwsh
dotnet PowerAppsTestEngine.dll -i ..\..\..\..\samples\pause\testPlan.fx.yaml -e 12345678-1234-1234-1234-1234567890ab -t 11111111-2222-3333-4444-555555555555
```

### PCF Control

Using the Creator Kit canvas application

```pwsh
dotnet PowerAppsTestEngine.dll -i ..\..\..\..\samples\creatorkit\testPlan.fx.yaml -e 12345678-1234-1234-1234-1234567890ab -t 11111111-2222-3333-4444-555555555555
```

### Networking

Using the [Automation Kit](https://aka.ms/AutomationCOE) Automation Project application from the Automation COE Main solution.

```pwsh
dotnet PowerAppsTestEngine.dll -i ..\..\..\..\samples\automationkit\testPlan.fx.yaml -e 12345678-1234-1234-1234-1234567890ab -t 11111111-2222-3333-4444-555555555555
```

### Playwright CSX Extension

Use any Playwright commands using CSharp script file (csx)

```pwsh
dotnet PowerAppsTestEngine.dll -i ..\..\..\..\samples\playwrightscript\testPlan.fx.yaml -e 12345678-1234-1234-1234-1234567890ab -t 11111111-2222-3333-4444-555555555555
```

### Sleep

Sleep test execution for a number of milliseconds

```pwsh
dotnet PowerAppsTestEngine.dll -i ..\..\..\..\samples\sleep\testPlan.fx.yaml -e 12345678-1234-1234-1234-1234567890ab -t 11111111-2222-3333-4444-555555555555
```
