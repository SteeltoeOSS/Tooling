#!/usr/bin/env pwsh

$baseDir = "$PSScriptRoot/.."
$toolDir = "$baseDir/tools"
$appSettings = "$PSScriptRoot/appsettings.json"

if ([string]::IsNullOrEmpty($Env:SignClientSecret)) {
    Write-Host "!!! SignClientSecret not set, exiting"
    Exit 1
}

if ($null -eq $Env:ArtifactDirectory) {
    Write-Host "!!! env var ArtifactDirectory not set; using default"
    $artifactDirectory = "."
}
else {
    $artifactDirectory = $Env:ArtifactDirectory
}
"--- using artifact directory $artifactDirectory"

"--- installing signclient"
New-Item -ItemType Directory -Force -Path $toolDir
dotnet tool install --tool-path $toolDir signclient

Write-Host "--- looking for nugets in $artifactDirectory"
$nupkgs = Get-ChildItem $artifactDirectory/Steeltoe*.*nupkg -recurse | Select-Object -ExpandProperty FullName
if ($nupkgs) {
    foreach ($nupkg in $nupkgs) {
        "signing $nupkg"
        $"$toolDir/SignClient 'sign' -c $appSettings -i $nupkg -r $Env:SignClientUser -s $Env:SignClientSecret -n 'Steeltoe' -d 'Steeltoe' -u 'https://github.com/SteeltoeOSS'"
        & $toolDir/SignClient 'sign' -c $appSettings -i $nupkg -r $Env:SignClientUser -s $Env:SignClientSecret -n 'Steeltoe' -d 'Steeltoe' -u 'https://github.com/SteeltoeOSS'
    }
}
else {
    Write-Host "no nugets found"
}
