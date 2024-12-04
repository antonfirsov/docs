---
title: Built-in Activities in .NET
description: Overview of activities emitted by the .NET libraries
ms.date: 12/02/2024
---

# Built-in Activities in .NET

This is a reference for distributed tracing Activities (<xref:System.Diagnostics.Activity>) emitted natively by .NET-s built-in <xref:System.Diagnostics.ActivitySource> instances.

## System.Net Activities

### HTTP client request

<xref:System.Net.Http.SocketsHttpHandler> and <xref:System.Net.Http.HttpClientHandler> report the the HTTP client request span following the recommendations in OpenTelemetry [HTTP Client Semantic Conventions](https://opentelemetry.io/docs/specs/semconv/http/http-spans/#http-client).

| Availability | <xref:System.Diagnostics.ActivitySource> name | <xref:System.Diagnostics.Activity.OperationName> | <xref:System.Diagnostics.Activity.DisplayName> |
|---|---|---|---|
| .NET 9+ | `Experimental.System.Net.Http` | `Experimental.System.Net.Http.HttpRequestOut` | `{http.request.method}`  |

#### Attributes

| Attribute  | Type | Description  | Examples  | Presence |
|---|---|---|---|---|
| `http.request.method` | string | HTTP request method.  | `GET`; `POST`; `HEAD`; `_OTHER` [1] | Always |
| `server.address` | string | Host identifier of the ["URI origin"](https://www.rfc-editor.org/rfc/rfc9110.html#name-uri-origin) HTTP request is sent to. | `example.com`; `10.1.2.80` | Always |
| `server.port` | int | Port identifier of the ["URI origin"](https://www.rfc-editor.org/rfc/rfc9110.html#name-uri-origin) HTTP request is sent to. | `80`; `8080`; `443` |  Always |
| `url.full` | string | Absolute URL describing a network resource according to [RFC3986](https://www.rfc-editor.org/rfc/rfc3986) [2] | `https://www.foo.bar/search?q=*` | Always |
| `error.type` | string | Request failure reason: one of the [HTTP request errors](xref:System.Net.Http.HttpRequestError) in snake_case, or a full exception type, or an HTTP 4xx/5xx status code. | `System.OperationCanceledException`; `name_resolution_error`; `secure_connection_error` ; `404` | If the request has failed. |
| `http.request.method_original` | string | Original HTTP method sent by the client in the request line. [4] | `ACL`; `foo` | Always |
| `http.response.status_code` | int | [HTTP response status code](https://tools.ietf.org/html/rfc7231#section-6). | `200` | If response was received. |
| `network.protocol.version` | string | Version of the HTTP protocol used. | `1.1`; `2` | If response was received. |

**[1] `http.request.method`:** The value contains the upper-case method name, if the method is one of the "known" methods listed in [RFC9110](https://www.rfc-editor.org/rfc/rfc9110.html#name-methods); otherwise the value is `_OTHER`.

**[2] `url.full`:** To avoid leaking secrets the value is redacted: the entire query is replaced with a `*` character and the fragment is missing. See the [URI redaction breaking change docs](../compatibility/networking/9.0/query-redaction-logs.md) for more details and opt-out switches.

### HTTP client request: wait for connection (experimental)

This activity is a child of an *HTTP client request* activity. It represents the time interval the corresponding request is waiting for an available connection in the request queue. If a free connection is available in the pool when the request comes in, no *wait for connection* activity will be created. Note that *wait for connection* does not represent the actual connection establishment; on has to rely on *HTTP connection setup* for that.

| Availability | <xref:System.Diagnostics.ActivitySource> name | <xref:System.Diagnostics.Activity.OperationName> | <xref:System.Diagnostics.Activity.DisplayName> |
|---|---|---|---|
| .NET 9+ | `Experimental.System.Net.Http.Connections` | `Experimental.System.Net.Http.Connections.WaitForConnection` | `HTTP wait_for_connection {server.address}:{server.port}`  |

> [!TIP]
> The time it takes to get a connection from the pool is also reported by the [`http.client.request.time_in_queue`](built-in-metrics-system-net.md#metric-httpclientrequesttime_in_queue) metric.



#### Attributes

| Attribute  | Type | Description  | Examples  | Presence |
|---|---|---|---|---|
| `error.type` | string | The connection failure reason: one of the [HTTP request errors](xref:System.Net.Http.HttpRequestError), or a full exception type. | `System.OperationCanceledException`; `name_resolution_error`; `secure_connection_error` | If the connection attempt fails. |

### HTTP connection setup (experimental)

This activity describes the establishment of an HTTP connection. Typically, this includes the the time it takes to resolve the DNS, establish the socket connection, and perform the TLS handshake.

| Availability | <xref:System.Diagnostics.ActivitySource> name | <xref:System.Diagnostics.Activity.OperationName> | <xref:System.Diagnostics.Activity.DisplayName> |
|---|---|---|---|
| .NET 9+ | `Experimental.System.Net.Http.Connections` | `Experimental.System.Net.Http.Connections.ConnectionSetup` | `HTTP connection_setup {server.address}:{server.port}`  |

There is no parent-child relationship between the *HTTP client request* and the *HTTP connection setup* activities; the latter will always be a root activity, defining a separate trace. However, if the connection attempt represented by *HTTP connection setup* results in a succesful HTTP connection, and that connection is picked up by a request to serve it, the instrumentation adds an <xref:System.Diagnostics.ActivityLink> to the *HTTP client request* activity pointing to the *HTTP connection setup* activity. I.e., each request is linked to the connection that served the request.

> [!NOTE]
> This activity is experimental. It might be changed or removed in future versions!

#### Attributes

| Attribute  | Type | Description  | Examples  | Presence |
|---|---|---|---|---|
| `error.type` | string | Connection failure reason: one of the [HTTP request errors](xref:System.Net.Http.HttpRequestError) in snake_case, or a full exception type. | `System.Net.Sockets.SocketException`; `name_resolution_error`; `secure_connection_error` | If the connection attempt fails. |
| `network.peer.address` | string | Peer IP address of the socket connection. | `10.5.3.2` | If the connection attempt succeeds. |
| `server.address` | string | Host identifier of the ["URI origin"](https://www.rfc-editor.org/rfc/rfc9110.html#name-uri-origin) the initial HTTP request is sent to. | `example.com` | Always |
| `server.port` | int | | Port identifier of the ["URI origin"](https://www.rfc-editor.org/rfc/rfc9110.html#name-uri-origin) the initial HTTP request is sent to. | Always |
| `url.scheme` | string | The [URI scheme](https://www.rfc-editor.org/rfc/rfc3986#section-3.1) component identifying the used protocol. | `http`; `https` | Always |

### Socket connect (experimental)

|  | string | | | Always |

