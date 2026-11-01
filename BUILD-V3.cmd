@echo off
cd /d "%~dp0"
dotnet restore
dotnet build
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
pause
