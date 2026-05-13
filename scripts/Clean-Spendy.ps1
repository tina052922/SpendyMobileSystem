# Removes build outputs for a fresh compile. Run from repo root:  .\scripts\Clean-Spendy.ps1
# Close Spendy / Visual Studio debug session first to avoid file locks.
$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root
dotnet clean "Spendy/Spendy.csproj" -c Debug
dotnet clean "Spendy/Spendy.csproj" -c Release
Remove-Item -Recurse -Force "Spendy/bin" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "Spendy/obj" -ErrorAction SilentlyContinue
Write-Host "Done. Rebuild in Visual Studio (Build > Rebuild Solution)."
