FROM microsoft/dotnet:2.2-sdk AS builder
WORKDIR /build
COPY . .

RUN dotnet publish -c release -o /publish-output

# Restore with `-r linux-x64` so that the runtime package that contains crossgen is downloaded
RUN dotnet restore -r linux-x64
RUN cp `find ~/.nuget/packages -name crossgen` /publish-output

FROM microsoft/dotnet:2.2-aspnetcore-runtime as application

# COMPlus_PerfMapEnabled is set in order to resolve symbols for .NET code.
ENV COMPlus_PerfMapEnabled=1

WORKDIR /app
COPY --from=builder /publish-output .

ENTRYPOINT ["dotnet", "webapi.dll"]

FROM application as sidecar
# Add whatever tools you want here
RUN apt-get update \
    && apt-get install -y \
       binutils \
       curl \
       htop \
       procps \
       linux-tools \
       lttng-tools \
       liblttng-ust-dev \
       zip \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /tools

RUN curl -OL http://aka.ms/perfcollect \
    && chmod a+x perfcollect

# perfcollect expects to find crossgen along side libcoreclr.so
RUN cp /app/crossgen $(dirname `find /usr/share/dotnet/ -name libcoreclr.so`)

ENTRYPOINT ["/bin/bash"]
