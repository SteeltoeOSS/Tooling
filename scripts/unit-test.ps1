$pwd = Get-Location
Set-Location $PSScriptRoot\..\test

[int]$errors = 0

Get-ChildItem -Directory -Filter "*.Test"| ForEach-Object {
    Set-Location $_.Name
    dotnet test
    $errors = $errors + $lastexitcode
    Set-Location ..
}

Set-Location $pwd

if ($errors -gt 0)
{
    Throw "$errors unit test(s) failed"
}
