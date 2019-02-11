#!/usr/bin/env pwsh

$project = "Steeltoe.Cli"
Push-Location $PSScriptRoot/..
try {
    dotnet tool uninstall --global $project
    dotnet pack
    dotnet tool install --global --add-source src/$project/bin/Debug $project
}
finally {
    Pop-Location
}
