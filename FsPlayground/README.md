# Solution, Project, and Package Management from Command Line

https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli


## Create new solution file called FSNetCore:
dotnet new sln -o FSNetCore

## Create a class library project in the src folder named Library:
dotnet new classlib -lang "F#" -o src/Library

## Install a NuGet package:
dotnet add package <PACKAGE_NAME>
dotnet add package "Newtonsoft.Json"

NuGet installs the latest version of the package when you use the dotnet add package command unless you specify the package version (-v switch).

## Install a specific version of a package:
dotnet add package <PACKAGE_NAME> -v <VERSION>
dotnet add package "Newtonsoft.Json" -v 12.0.3

## List the package references for your project:
dotnet list package

## Restore Packages
Use the dotnet restore command, which restores packages listed in the project file (see PackageReference). With .NET Core 2.0 and later, restore is done automatically with dotnet build and dotnet run. As of NuGet 4.0, this runs the same code as nuget restore.

As with the other dotnet CLI commands, first open a command line and switch to the directory that contains your project file.

To restore a package using dotnet restore:
dotnet restore








