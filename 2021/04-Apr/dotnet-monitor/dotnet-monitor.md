---
post_title: "What's new in dotnet monitor"
username: soshir@microsoft.com
microsoft_alias: soshir
categories: .NET
summary: "Learn about what's new in dotnet monitor"
desired_publication_date: '2021-04-21'
---

We've previously introduced [`dotnet monitor` as an experimental tool](https://devblogs.microsoft.com/dotnet/introducing-dotnet-monitor/) to access diagnostics information in a dotnet process. We're now pleased to announce `dotnet monitor` has graduated to a supported tool in the .NET ecosystem beginning with today's release. 

If you are new to `dotnet monitor` , we recommend checking out the [official documentation](https://github.com/dotnet/dotnet-monitor/tree/main/documentation) which includes walkthroughs on using `dotnet monitor` on a local machine, with Docker, and Kubernetes, 

This blog post details some of the new major features in the ***preview4***  release of `dotnet monitor`:

- Egress providers
- Custom metrics
- Security and Hardening


## Egress providers

In previous previews, the only way to egress diagnostic artifacts from `dotnet monitor` was via the HTTP response stream. While this works well over reliable connections, this becomes increasingly challenging for very large artifacts and less reliable connections.

In ***preview4***, you can configure `dotnet monitor` to egress artifacts to other destinations: Azure Blob Storage and the local filesystem. It is possible to specify multiple egress providers via configuration as shown in the example below:

```json
{
    "Egress": {
        "Providers": {
            "sampleBlobStorageEgressProvider": {
                "type": "azureBlobStorage",
                "accountUri": "https://exampleaccount.blob.core.windows.net",
                "containerName": "dotnet-monitor",
                "blobPrefix": "artifacts",
                "accountKeyName": "MonitorBlobAccountKey"
            }
        },
        "Properties": {
            "MonitorBlobAccountKey": "accountKey"
        }
    }
}
```

Once configured, at the time of triggering the artifact collection via an HTTP request, you can now specify which egress provider to use. With the configuration above, you can now make the following request:

```http
GET /dump/?egressProvider=sampleBlobStorageEgressProvider HTTP/1.1
```

For more detailed instructions on configuring egress providers, look at the [egress configuration documentation](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/configuration.md#egress-configuration)

## Custom metrics

In addition to the collection of `System.Runtime` and `Microsoft.AspNetCore.Hosting` metrics, it is now possible to collect additional metrics (emitted via [EventCounters](https://docs.microsoft.com/dotnet/core/diagnostics/event-counters)) for exporting in the [Prometheus exposition format](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/api/metrics.md#sample-request).

You can configure `dotnet monitor` to collect additional metrics as shown in the example below:

```json
{
  "Metrics": {
    "Providers": [
      {
        "ProviderName": "Microsoft-AspNetCore-Server-Kestrel",
        "CounterNames": [
          "connections-per-second",
          "total-connections"
        ]
      }
    ]
  }
}
```

For more detailed instructions on collecting custom metric, look at the [metrics configuration documentation](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/configuration.md#metrics-configuration)


## Security and Hardening

Requiring authentication is part of the work that's gone into hardening `dotnet monitor` to make it suitable for deployment in production environments. Additionally, to protect the credentials sent over the wire as part of authentication, `dotnet monitor` will also default to requiring that the underlying channel uses HTTPS.

In `preview4 `  the `/processes`, `/dump`, `/gcdump`, `/trace`, and `/logs` API endpoints will require authentication. The `/metrics` endpoint will still be available without authentication on a separately configured `metricsUrl` for scraping via external tools like Prometheus.

In the local machine scenario with .NET SDK already installed, `dotnet monitor` will default to using ASP.NET Core HTTPS development certificate. If running on Windows, we also enable Windows authentication for secure experience as an alternative to API token auth.

> Some steps in configuring `dotnet monitor` securely have been omitted for brevity in the blog post. We recommend looking at the **[official documentation](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/authentication.md) for detailed instructions**.

To get started with `dotnet monitor` in production, you will require an SSL certificate and an API Token.


### [Generating an SSL Certificate](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/README.md).

To configure `dotnet monitor` to run securely, you will need to generate an SSL certificate with an EKU for server usage. You can either request this certificate from your certificate authority or generate a self-signed certificate.

If you wish to generate another self-signed certificate for use on another machine you may do so by invoking the `dotnet dev-certs` tool:

```cmd
dotnet dev-certs https -export-path self-signed-certificate.pfx -p <your-cert-password>
```

### [Generating an API token](https://github.com/dotnet/dotnet-monitor/blob/main/documentation/authentication.md#api-key-authentication)

You should generate a 32-byte cryptographically random secret to use an API token: 

```bash
dotnet monitor generatekey
```

That should produce an output that resembles this:

```cmd
Authorization: MonitorApiKey H2O2yT1c9yLkbDnU9THxGSxje+RhGwhjjTGciRJ+cx8=
ApiKeyHash: B4D54269DB7D948A8C640DB65B46D2D705A516134DA61CD97E424AC08E5021ED
ApiKeyHashType: SHA256
```

Once you have both an SSL certificate and an API Token generated, you can configure `dotnet monitor` to respond to authenticated HTTP requests over a secure TLS channel using the following configuration:


```json
{
  "ApiAuthentication": {
    "ApiKeyHash": "<HASHED-TOKEN>",
    "ApiKeyHashType": "SHA256"
  },
  "Kestrel": {
    "Certificates": {
      "Default":{
        "Path": "<path-to-cert.pfx>",
        "Password": "<your-cert-password>"
      }
    }
  }
}
```

When using Windows Authentication, your browser will automatically handle the Windows authentication challenge. If you are using an API Key, you must specify it via the `Authorization` header on HTTP requests

```cmd
curl.exe -H "Authorization: MonitorApiKey Uf6Yq8ZGkcn+ltq2QLHxuXpKA6/Q5VH5Mb5aSIJBxRc=" https://localhost:52323/processes
```

## Roadmap

We will continue to iterate on `dotnet monitor` with monthly updates until we'll release a stable version later this year. `dotnet monitor` supports .NET Core 3.1 as well as .NET 5 and later.

## Conclusion

We are excited to introduce this major update to `dotnet monitor` and want your feedback. Let us know what we can do to make it easier to diagnose what’s wrong with your .NET application.

[Let us know what you think!](https://github.com/dotnet/dotnet-monitor/issues/new/choose).
