# BAT-Manager - Local Release Packaging Script
[CmdletBinding()]
param (
    [string]$Version = "1.2.0",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$root = $PSScriptRoot
Set-Location $root

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "   BAT-Manager - Release Packaging Tool (v$Version)  " -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# 1. Locate MSBuild
$msBuildCandidates = @(
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
)

$msBuild = $null
foreach ($path in $msBuildCandidates) {
    if (Test-Path $path) {
        $msBuild = $path
        break
    }
}

if (-not $msBuild) {
    throw "MSBuild could not be found. Please install Visual Studio or ensure .NET Framework 4.8 is available."
}

Write-Host "[1/5] Using MSBuild: $msBuild" -ForegroundColor Green

# 2. Build Release
Write-Host "[2/5] Building solution ($Configuration)..." -ForegroundColor Yellow
& $msBuild "BAT-Manager.sln" /t:Clean,Build /p:Configuration=$Configuration /p:Platform="Any CPU" /v:m
if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE."
}

# 3. Run Unit Tests
Write-Host "[3/5] Running automated tests..." -ForegroundColor Yellow
$testsExe = Join-Path $root "BAT-Manager.Tests\bin\$Configuration\BAT-Manager.Tests.exe"
if (Test-Path $testsExe) {
    & $testsExe
    if ($LASTEXITCODE -ne 0) {
        throw "Unit tests failed!"
    }
} else {
    Write-Warning "Tests executable not found at $testsExe, skipping tests."
}

# 4. Prepare Distribution Directory
Write-Host "[4/5] Packaging distribution artifacts..." -ForegroundColor Yellow
$distDir = Join-Path $root "dist"
$bundleDir = Join-Path $distDir "BAT-Manager-v$Version"
$zipPath = Join-Path $distDir "BAT-Manager-v$Version-windows.zip"
$targetExe = Join-Path $distDir "BAT-Manager.exe"

if (Test-Path $distDir) {
    Remove-Item $distDir -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $bundleDir | Out-Null
New-Item -ItemType Directory -Force -Path (Join-Path $bundleDir "scripts") | Out-Null

$srcExe = Join-Path $root "BAT-Manager\bin\$Configuration\BAT-Manager.exe"
Copy-Item $srcExe -Destination $targetExe
Copy-Item $srcExe -Destination (Join-Path $bundleDir "BAT-Manager.exe")
Copy-Item (Join-Path $root "README.md") -Destination (Join-Path $bundleDir "README.md")
Copy-Item (Join-Path $root "LICENSE") -Destination (Join-Path $bundleDir "LICENSE")
Copy-Item (Join-Path $root "BAT-Manager\scripts\*") -Destination (Join-Path $bundleDir "scripts\") -Recurse

# Compress bundle to zip
Compress-Archive -Path "$bundleDir\*" -DestinationPath $zipPath -Force

# 5. Calculate Checksums
Write-Host "[5/5] Generating SHA256 checksums..." -ForegroundColor Yellow
$exeHash = (Get-FileHash $targetExe -Algorithm SHA256).Hash
$zipHash = (Get-FileHash $zipPath -Algorithm SHA256).Hash

$checksums = @"
$exeHash  BAT-Manager.exe
$zipHash  BAT-Manager-v$Version-windows.zip
"@

$checksumsFile = Join-Path $distDir "SHA256SUMS.txt"
$checksums | Set-Content $checksumsFile -Encoding utf8

Write-Host ""
Write-Host "==================================================" -ForegroundColor Green
Write-Host ">> PACKAGING COMPLETED SUCCESSFULLY!              " -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green
Write-Host "Artifacts are ready in: $distDir" -ForegroundColor Cyan
Write-Host ""
Get-ChildItem $distDir | Select-Object Name, Length, LastWriteTime | Format-Table -AutoSize
Write-Host "SHA256 Checksums:" -ForegroundColor Magenta
Write-Host $checksums -ForegroundColor Gray
Write-Host ""
Write-Host "You can now attach these files directly to a GitHub Release!" -ForegroundColor Green
