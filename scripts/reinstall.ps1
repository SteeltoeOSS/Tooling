$globalToolProject = "Steeltoe.Tooling.Cli"

Set-Location $PSScriptRoot\..

dotnet tool uninstall --global $globalToolProject
dotnet pack
dotnet tool install --global --add-source src/$globalToolProject/bin/Debug $globalToolProject
