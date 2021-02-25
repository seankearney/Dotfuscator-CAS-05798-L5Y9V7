# Dotfuscator Support Case CAS-05798-L5Y9V7

This project is a demo of a .NET 3.1 Console Application intended to run on Win/OSX. This console application is meant to run without a terminal window and be hidden from the user (i.e. a Daemon/Service).

- Attempt to get a working Azure Pipeline. There were issues with `DotNetCoreCLI@2` resulting from `Sdk` changes in `.csproj` file
- "Window-less" console app not being generated. 

Note: This commit history on this repo has been flattened to hide countless attempts at getting pipeline to work.

## Repro

1. Ensure that `DotfuscatorMSBuildDir` MSBuild property is set to valid path. By default it will use `$(MSBuildProgramFiles32)\MSBuild\PreEmptive\Dotfuscator\6`. See `Directory.Build.props`
2. Execute `dotnet publish .\Obfuscation\Obfuscation.csproj -c Release -o publish/obfuscated -r win-x64 --self-contained`
3. Execute `dotnet clean`
4. Edit the `Obfuscation.csproj` file to disable Dotfuscator
   1. Change Line 1 from `<Project>` to `<Project Sdk="Microsoft.NET.Sdk.Web">`
   2. Comment out lines 28, 29, 30
5. Execute `dotnet publish .\Obfuscation\Obfuscation.csproj -c Release -o publish/not-obfuscated -r win-x64 --self-contained`

## Demonstrate Differences

The desired behavior is that `Obfuscation.exe` runs without a window.

1. Execute `./publish/not-obfuscated/Obfuscation.exe`. You should observe that no console window was opened, yet if you look in Task Manager > Details you will see `Obfuscation.exe` running.
2. Kill `Obfuscation.exe` in Task Manager
3. Execute `./publish/obfuscated/Obfuscation.exe`. You should observe that a console window was opened for the executable, which isn't desired.

It seems evident that by including the `PreEmptive.Dotfuscator.Common.targets` in our build/publish process we get different behavior.

## Azure Pipeline "Gotchas"

When using `DotNetCoreCLI@2` to `publish` you must be aware of a few things.

1. By Removing the `Sdk="Microsoft.NET.Sdk.Web"` from the top `<Project` element of your `.csproj` file it will prevent the task from finding your project as a valid "web" project. Therefore, 
	1. You **must** set `publishWebProjects: false` 
	2. You **must** set `projects:` to the path of your "web" project.

> Note: nuget.config was edited. You must provide a valid provate feed to fetch Dotfuscator nupkg