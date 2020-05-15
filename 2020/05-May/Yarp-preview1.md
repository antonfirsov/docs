# Introducing YARP Preview 1

[YARP](https://github.com/microsoft/reverse-proxy) is a project to create a reverse proxy server. It started when we noticed a pattern of questions from internal teams at Microsoft who were either building a reverse proxy for their service or had been asking about APIs and technology for building one, so we decided to get them all together to work on a common solution, which has become YARP.

YARP is a reverse proxy toolkit for building fast proxy servers in .NET using the infrastructure from ASP.NET and .NET. The key differentiator for YARP is that it is being designed to be easily customized and tweaked to match the specific needs of each deployment scenario. YARP plugs into the ASP.NET pipeline for handling incoming requests, and then has its own sub-pipeline for performing the steps to proxy the requests to backend servers. Customers can add additional modules, or replace stock modules as needed.

With the basic infrastructure of the proxy now in place, we've produced the first official build of YARP (Preview 1) in order to better collaborate and get feedback.

## What is in Preview 1
* Core proxy infrastructure
* Config-based route definitions
* Pipeline model for extensibility
* Forwarded Headers (hard coded)

## What is not in Preview 1
* Targeting both .NET Core 3.1 and .NET Core 5 [\#159](https://github.com/microsoft/reverse-proxy/issues/159)
* Session Affinity [\#45](https://github.com/microsoft/reverse-proxy/issues/45)
* Forwarded Headers (configurable) [\#60](https://github.com/microsoft/reverse-proxy/issues/60)
* Code-based route definition & per-request routing [\#8](https://github.com/microsoft/reverse-proxy/issues/8), [\#46](https://github.com/microsoft/reverse-proxy/issues/46)
* Metrics & logging [\#61](https://github.com/microsoft/reverse-proxy/issues/61)
* Performance tweaks [\#51](https://github.com/microsoft/reverse-proxy/issues/51)
* Connection filtering [\#53](https://github.com/microsoft/reverse-proxy/issues/53)
* And much more - see the [backlog project board](https://github.com/microsoft/reverse-proxy/projects/5) for more details

# Getting started with YARP Preview 1

YARP is designed as a library that provides the core proxy functionality, that you can then customize by adding or replacing modules. For preview 1, YARP is being supplied as a nuget package and code snippets. We plan on having a project template, and pre-built exe in future previews.

1. Download the preview 3 (or greater) of .NET 5 SDK from https://dotnet.microsoft.com/download/dotnet/5.0
2. Create an "Empty" ASP.NET Core application using 
   * `dotnet new web -n MyProxy`
   
> Or

   * Create a new ASP.NET Core web application in Visual Studio, and choose "Empty" for the project template.
3. Open the Project and make sure it includes:

```	
<PropertyGroup>
  <TargetFramework>net5.0</TargetFramework>
</PropertyGroup>
```

And

```
<ItemGroup>
  <PackageReference Include="Microsoft.ReverseProxy.Core" Version="1.0.0-preview.1" />
</ItemGroup>
```

4. **Startup.cs**

   YARP is implemented as a ASP.NET Core component, and so the majority of the sample code is in Startup.cs.

   The configure method defines the ASP.NET pipeline for processing requests. The reverse proxy is plugged in to ASP.NET endpoint routing, and then has its own sub pipeline for the proxy. Here proxy pipeline modules, such as load balancing can be added to customize the handling of the request.

```
/// <summary>
/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
/// </summary>
public void Configure(IApplicationBuilder app)
{
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapReverseProxy(proxyPipeline =>
        {
            proxyPipeline.UseProxyLoadBalancing();
        });
    });
}
```

YARP currently uses configuration to define the routes and endpoints for the proxy. That is loaded in the ConfigureServices method.

```
/// <summary>
/// This method gets called by the runtime. Use this method to add services to the container.
/// </summary>
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddReverseProxy()
        .LoadFromConfig(_configuration.GetSection("ReverseProxy"))
}
```

5. **Configuration**

The configuration for YARP is defined in JSON in the appsettings.json file. It defines a set of:
* `Backends` - which are the clusters of servers that requests can be routed to
   * The destination name is an identifier that is used in metrics and logging - it has no special meaning
   * `Address` is the URI prefix that will have the original request path appended to it
* `Routes` - which map incoming requests to the backend clusters based on aspects of the request such as host name, path, method, request headers etc.
	Routes are ordered, so the `app1` route needs to be defined first as `route2` will act as a catchall for all paths that have not already been matched.
	
	The following needs to be a peer to the `Logging` and `AllowedHosts` sections.
```
"ReverseProxy": {
  "Backends": {
    "backend1": {
      "LoadBalancing": {
        "Mode": "Random"
      },
      "Destinations": {
        "backend1_server1": {
          "Address": "https://example.com:10000/"
        },
        "backend1_server2": {
          "Address": "http://example.com:10010/"
        }
      }
    },
    "backend2": {
      "Destinations": {
        "backend2_dest": {
          "Address": "https://example.com:10001/"
        }
      }
    }
  },
  "Routes": [
    {
      "RouteId": "app1",
      "BackendId": "backend2",
      "Match": {
        "Methods": [ "GET", "POST" ],
        "Path": "/app1/"
      }
    },
    {
      "RouteId": "route2",
      "BackendId": "backend1",
      "Match": {
        "Methods": [ "GET", "POST" ],
        "Host": "localhost",
        "Path": "/"
      }
    }
  ]
}
```

# Survey

We are interested to see how you are currently using a reverse proxy and what problems we need to solve with this project. We'd appreciate your input [here](https://www.surveymonkey.com/r/3D53JKL). 


# Code Feedback

If you have feature suggestions, changes, bugs etc, please provide feedback using the issue templates in the project repo. We welcome contributions, please see [contibuting.md]() for more details.
