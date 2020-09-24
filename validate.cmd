@echo off
setlocal
set PROJECT_FILE=%~dp0tools\blog-validate\blog-validate.csproj
dotnet run --project %PROJECT_FILE% -- %~dp0