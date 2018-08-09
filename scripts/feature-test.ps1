$pwd = Get-Location
Set-Location $PSScriptRoot\..\feature

[int]$errors = 0

Get-ChildItem -Directory -Filter "*.Feature" | ForEach-Object {
    Set-Location $_.Name
    dotnet test
    $errors = $errors + $lastexitcode
    Set-Location ..
}

Set-Location $pwd

if ($errors -gt 0)
{
    Throw "$errors feature test(s) failed"
}
