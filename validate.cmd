@echo off
setlocal
set PROJECT_FILE=%~dp0tools\blog-validate\blog-validate.csproj
set CATEGORIES_FILE=%~dp0categories.txt
dotnet run --project %PROJECT_FILE% -- %~dp0 --categories %CATEGORIES_FILE% %*