$BaseDir = Split-Path (Resolve-Path $MyInvocation.MyCommand.Path)

Set-Alias NuGet "$BaseDir\.NuGet\NuGet.exe"

NuGet Pack HipChat.NET.nuspec