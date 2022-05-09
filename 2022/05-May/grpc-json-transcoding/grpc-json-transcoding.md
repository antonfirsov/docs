---
post_title: Announcing gRPC JSON transcoding for .NET
username: jamesnk
microsoft_alias: jamesnk
categories: .NET, ASP.NET Core, ASP.NET, gRPC
featured_image: grpc-icon-color-1.png
desired_publication_date: 2022-05-11
summary: gRPC JSON transcoding is a new feature for .NET that allows gRPC services to be called with REST + JSON. Try it now in .NET 7 preview 4.
---

gRPC is a modern way to communicate between apps. gRPC uses HTTP/2, streaming, binary serialization, and message contracts to create high-performance, real-time services. .NET has excellent support for building gRPC apps.

However, like most technology choices, there are trade-offs when choosing gRPC over an alternative like REST+JSON. gRPC's strengths include performance and developer productivity. Meanwhile, REST+JSON can be used everywhere, and its human-readable JSON messages are easy to debug.

Wouldn't it be great if we could build services once in ASP.NET Core and get both gRPC and REST? Now you can! Introducing gRPC JSON transcoding for .NET.

## gRPC or REST? Why not both

gRPC JSON transcoding is an extension for ASP.NET Core that creates RESTful HTTP APIs for gRPC services. Once configured, JSON transcoding allows you to call gRPC methods with familiar HTTP concepts:

* HTTP verbs
* URL parameter binding
* JSON requests/responses

This is a [highly requested feature](https://github.com/grpc/grpc-dotnet/issues/167) that was previously available as an experiment. We're excited to ship the first preview today, and we aim to ship a stable release with .NET 7.

## Getting started

1. The first step is to create a gRPC service (if you don't already have one). [Create a gRPC client and service](https://docs.microsoft.com/aspnet/core/tutorials/grpc/grpc-start) is a great tutorial for getting started.
2. Next, add a package reference to `Microsoft.AspNetCore.Grpc.JsonTranscoding` to the server. Register it in server startup code by adding `AddJsonTranscoding()`. For example, `services.AddGrpc().AddJsonTranscoding()`.
3. The last step is annotating your gRPC *.proto* file with HTTP bindings and routes. The annotations define how gRPC services map to the JSON request and response. You will need to add `import "google/api/annotations.proto";` to the gRPC proto file and have a copy of [*annotations.proto*](https://github.com/dotnet/aspnetcore/blob/main/src/Grpc/JsonTranscoding/test/testassets/Sandbox/google/api/annotations.proto) and [*http.proto*](https://github.com/dotnet/aspnetcore/blob/8b601c3a73ba66de4e6ca35530b5d32a48c76c5b/src/Grpc/JsonTranscoding/test/testassets/Sandbox/google/api/http.proto) in the `google/api` folder in your project.

```proto
syntax = "proto3";

import "google/api/annotations.proto";

package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply) {
    option (google.api.http) = {
      get: "/v1/greeter/{name}"
    };
  }
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

In the sample above, the SayHello gRPC method has been annotated with HTTP information. It can now be invoked as gRPC and as a RESTful API:

* Request: `HTTP/1.1 GET /v1/greeter/world`
* Response: `{ "message": "Hello world" }`

And browser apps call it like any other RESTful API:

```js
fetch('https://localhost:5001/v1/greeter/world')
    .then((response) => response.json())
    .then((result) => {
        console.log(result.message);
        // Hello world
    });
```

This is a simple example. See [HttpRule](https://cloud.google.com/service-infrastructure/docs/service-management/reference/rpc/google.api#google.api.HttpRule) for more customization options.

## JSON transcoding vs gRPC-Web

Both JSON transcoding and gRPC-Web allow gRPC services to be called from a browser. However, the way each does this is different:

* gRPC-Web lets browser apps call gRPC services from the browser with the gRPC-Web client and Protobuf. gRPC-Web requires the browser app to generate a gRPC client and has the advantage of sending small, fast Protobuf messages.
* JSON transcoding allows browser apps to call gRPC services as RESTful APIs with JSON. The browser app doesn't need to generate a gRPC client or know anything about gRPC.

## About JSON transcoding implementation

JSON transcoding with gRPC isn't a new concept. [grpc-gateway](https://grpc-ecosystem.github.io/grpc-gateway/) is another technology for creating RESTful JSON APIs from gRPC services. It uses the same *.proto* annotations to map HTTP concepts to gRPC services. The important difference is in how each is implemented.

grpc-gateway uses code generation to create a reverse-proxy server. The reverse-proxy translates RESTful calls into gRPC+Protobuf and sends them over HTTP/2 to the gRPC service. The benefit of this approach is the gRPC service doesn't know about the RESTful JSON APIs. Any gRPC server can use grpc-gateway.

Meanwhile, gRPC JSON transcoding runs inside an ASP.NET Core app. It deserializes JSON into Protobuf messages, then invokes the gRPC service directly. We believe JSON transcoding offers many advantages to .NET app developers:

* Fewer moving parts. Both gRPC services and mapped RESTful JSON API run out of one ASP.NET Core application.
* Performance. JSON transcoding deserializes JSON to Protobuf messages and invokes the gRPC service directly. There are significant performance benefits in doing this in-process vs. making a new gRPC call to a different server.
* Cost. Fewer servers = smaller monthly hosting bill.

JSON transcoding doesn't replace MVC or Minimal APIs. It only supports JSON, and it's very opinionated about how Protobuf maps to JSON.

## What's next

This is the first release of gRPC JSON transcoding for .NET. Future previews of .NET 7 will focus on improving performance and OpenAPI support.

## Try it with .NET today

gRPC JSON transcoding is available now with .NET 7 Preview 4.

For more information about JSON transcoding, check out the [documentation](https://docs.microsoft.com/aspnet/core/grpc/httpapi), or try out [a sample app that uses JSON transcoding](https://github.com/grpc/grpc-dotnet/tree/master/examples#transcoder).

We look forward to seeing what you create with .NET, gRPC, and now gRPC JSON transcoding!
