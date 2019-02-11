#!/usr/bin/env pwsh

Push-Location $PSScriptRoot/..
try {
    dotnet test test/Steeltoe.Cli.Test
} finally {
    Pop-Location
}
