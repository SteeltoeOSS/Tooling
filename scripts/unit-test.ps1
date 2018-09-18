#!/usr/bin/env pwsh

$pwd = Get-Location
Set-Location $PSScriptRoot\..\test\Steeltoe.Tooling.Test
dotnet test
$errors = $lastexitcode
Set-Location $pwd
if ($errors -gt 0)
{
    Throw "$errors unit test(s) failed"
}
