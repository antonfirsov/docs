@echo off
setlocal
set PROJECT_FILE=%~dp0tools\blog-new-post\blog-new-post.csproj
dotnet run --project %PROJECT_FILE% -- %~dp0
