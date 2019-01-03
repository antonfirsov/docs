FROM application-base
WORKDIR /app

# COMPlus_PerfMapEnabled is set in order to resolve symbols for .NET code.
ENV COMPlus_PerfMapEnabled=1

ENTRYPOINT ["dotnet", "webapi.dll"]