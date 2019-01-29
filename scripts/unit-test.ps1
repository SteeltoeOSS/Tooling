#!/usr/bin/env pwsh

Push-Location $PSScriptRoot/..
try {
    dotnet test test/Steeltoe.Tooling.Test
} finally {
    Pop-Location
}
