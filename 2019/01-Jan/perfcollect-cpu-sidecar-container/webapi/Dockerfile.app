FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /build
COPY . .

RUN dotnet publish -c release -o /publish-output

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /publish-output .

# COMPlus_PerfMapEnabled is set in order to resolve symbols for .NET code.
ENV COMPlus_PerfMapEnabled=1

ENTRYPOINT ["dotnet", "webapi.dll"]
