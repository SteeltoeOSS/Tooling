#!/usr/bin/env pwsh

$baseDir = "$PSScriptRoot/.."
$toolDir = "$baseDir/tools"
$appSettings = "$PSScriptRoot/appsettings.json"
if ($null -eq $ArtifactDirectory) {
    $ArtifactDirectory = "."
}

if ([string]::IsNullOrEmpty($Env:SignClientSecret)) {
    Write-Host "SignClientSecret not set, exiting"
    Exit 1
}

New-Item -ItemType Directory -Force -Path $toolDir
dotnet tool install --tool-path $toolDir signclient

Write-Host "looking for nugets in $ArtifactDirectory"
$nupkgs = Get-ChildItem $ArtifactDirectory/Steeltoe*.*nupkg -recurse | Select-Object -ExpandProperty FullName
if ($nupkgs) {
    foreach ($nupkg in $nupkgs) {
        Write-Host "signing $nupkg"
        # & $toolDir/SignClient 'sign' -c $appSettings -i $nupkg -r $Env:SignClientUser -s $Env:SignClientSecret -n 'Steeltoe' -d 'Steeltoe' -u 'https://github.com/SteeltoeOSS'
    }
}
else {
    Write-Host "no nugets found"
}
