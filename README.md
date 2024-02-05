# Notification Listener IN C#

Program.cs contains the code to setup Visual Studio 2022 .NET 7.0 to create a Notification Listener

## Installation

Install this SDK :
```bash
https://developer.microsoft.com/en-us/windows/downloads/sdk-archive/

```
Then using it to edit the .csproj of your project and changing the TargetFramework to this:
```bash
<TargetFramework>net7.0-windows$([Microsoft.Build.Utilities.ToolLocationHelper]::GetLatestSDKTargetPlatformVersion('Windows', '10.0'))</TargetFramework>
```
## Usage

Paste the Program,cs in your file and run it.

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.
