param(
    [string]$Version = "1.0.0"
)

$ErrorActionPreference = "Stop"
$ProjectDir = $PSScriptRoot
$ProjectFile = Join-Path $ProjectDir "HideDryTimeMod.csproj"
$OutputDir = Join-Path $ProjectDir "bin\Release\net6.0"
$DistDir = Join-Path $ProjectDir "dist"
$ZipName = "HideDryTimeMod-v$Version.zip"
$ZipPath = Join-Path $DistDir $ZipName
$DllPath = Join-Path $OutputDir "HideDryTimeMod.dll"

New-Item -ItemType Directory -Force -Path $DistDir | Out-Null

dotnet restore $ProjectFile --nologo
dotnet build $ProjectFile --configuration Release --nologo --no-restore

if (-not (Test-Path $DllPath)) {
    throw "Build output not found: $DllPath"
}

$StageDir = Join-Path $DistDir "stage"
Remove-Item -Recurse -Force $StageDir -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $StageDir | Out-Null

Copy-Item $DllPath (Join-Path $StageDir "HideDryTimeMod.dll")
Copy-Item (Join-Path $ProjectDir "README.md") (Join-Path $StageDir "README.md")
Copy-Item (Join-Path $ProjectDir "CHANGELOG.md") (Join-Path $StageDir "CHANGELOG.md")

if (Test-Path $ZipPath) { Remove-Item -Force $ZipPath }
Compress-Archive -Path (Join-Path $StageDir "*") -DestinationPath $ZipPath

Remove-Item -Recurse -Force $StageDir
Write-Host "[v] Release package created: $ZipPath" -ForegroundColor Green
