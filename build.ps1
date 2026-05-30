# build.ps1 — Build and install HideDryTimeMod
param(
    [switch]$Debug,
    [switch]$Clean
)

$ErrorActionPreference = "Stop"
$ProjectDir  = $PSScriptRoot
$ProjectFile = Join-Path $ProjectDir "HideDryTimeMod.csproj"
$Config      = if ($Debug) { "Debug" } else { "Release" }
$OutputDir   = Join-Path $ProjectDir "bin\$Config\net6.0"
$ModsDir     = Join-Path $ProjectDir "..\..\Mods"
$ModName     = "HideDryTimeMod"

if ($Clean) {
    dotnet clean $ProjectFile --configuration $Config
    exit 0
}

dotnet restore $ProjectFile --nologo
if ($LASTEXITCODE -ne 0) { exit 1 }

dotnet build $ProjectFile --configuration $Config --nologo --no-restore
if ($LASTEXITCODE -ne 0) { exit 1 }

$SrcDll  = Join-Path $OutputDir "$ModName.dll"
$DestDll = Join-Path $ModsDir "$ModName.dll"
Copy-Item -Force $SrcDll $DestDll
Write-Host "[v] Installed to $DestDll" -ForegroundColor Green
