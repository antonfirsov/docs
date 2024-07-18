---
post_title: Introducing CoreWCF and WCF Client Azure Queue Storage bindings for .NET
author1: susah
post_slug: introducing-core-wcf-and-wcf-client-azure-queue-storage-bindings-for-dotnet
microsoft_alias: susah
featured_image: introducing-core-wcf-and-wcf-client-azure-queue-storage-bindings-for-dotnet.jpg
categories: .NET, WCF
tags: CoreWCF AQS, CoreWCF, WCF, Microsoft.CoreWCF.Azure.StorageQueues, Microsoft.WCF.Azure.StorageQueues.Client
summary: The initial beta release of the official libraries Microsoft.CoreWCF.Azure.StorageQueues and Microsoft.WCF.Azure.StorageQueues.Client library for .NET is now available.
post_date: 2024-07-18 14:10:00
---


We are thrilled to announce the release of two new libraries: [CoreWCF Azure Queue Storage service library][CoreWCFAzureStorageNuget] and [WCF Azure Queue Storage client library][WCFAzureStorageNuget] for .NET. These libraries have been specifically designed to seamlessly integrate CoreWCF services with [Azure Queue Storage][azure_queue].

With these new libraries, .NET developers can now leverage the robust capabilities of Azure Queue Storage for reliable messaging and scalable communication. Historically, the .NET Framework provided support for Microsoft Message Queuing (MSMQ), but the modern offerings of [CoreWCF][corewcf-repo] and [WCF Client][wcf-repo] are targeting cross-platform compatibility, extending support from Windows to Linux and Mac.

MSMQ, while effective, is not ideally suited for cloud environments. Recognizing this limitation, we now offer support for Azure Queue Storage, a modern, cloud-enabled alternative.

There are many scenarios where using a queue to make a service request is advantageous. Maintaining an always running service can be expensive. Combining Azure Queue Storage with CoreWCF service allows your service to run periodically, process the pending requests, and shut down when complete to optimize Azure compute costs. With these libraries, you can configure your CoreWCF service to read from Azure Queue Storage, process the messages, and ensure reliable delivery, while the WCF client can send requests to the CoreWCF service.

### Key Features

- **Seamless Integration**: Easily integrate Azure Queue Storage with CoreWCF services.
- **Scalability**: Leverage robust Azure Queue Storage for high-scale applications.
- **Reliability**: Ensure message delivery with reliable storage capabilities of Azure Queue Storage.

## Getting Started

- [Getting Started with CoreWCF Azure Queue Storage service library](#getting-started-with-corewcf-azure-queue-storage-service-library)
- [Getting Started with WCF Azure Queue Storage client library](#getting-started-with-wcf-azure-queue-storage-client-library)

### Getting Started with CoreWCF Azure Queue Storage service library

1. **Installation**: Add the library to your project using NuGet by either of the following methods:

    - In Visual Studio, from the Package Explorer, search for Microsoft.CoreWCF.Azure.StorageQueues, or
    - Type `Install-Package Microsoft.CoreWCF.Azure.StorageQueues -AllowPrereleaseVersions` into the Package Manager Console, or
    - Type `dotnet add ./path/to/myproject.csproj package Microsoft.CoreWCF.Azure.StorageQueues --prerelease` in a terminal (replace ./path/to/myproject.csproj by the path to the *.csproj file you want to add the dependency)

2. **Configuration**: Configure your CoreWCF services to use Azure Queue Storage. In your startup class, add the necessary configuration:

    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelServices()
                .AddQueueTransport();
    }
    ```

    To receive requests from the Azure Queue Storage service, you'll need to configure CoreWCF with the appropriate endpoint and configure credentials. The [Azure Identity library][identity] makes it easy to add Microsoft Entra ID support for authenticating with Azure services.

    There are multiple authentication mechanisms for Azure Queue Storage which is configured via the property `AzureQueueStorageBinding.Security.Transport.ClientCredentialType`. The recommended way to handle authentication is to use [Azure managed identities][managed_identity]. This library's default credential type is `Default`, which uses `DefaultAzureCredential`. You can learn more about the DefaultAzureCredential by visiting the [Microsoft Learn documentation on this topic.][default_azure_credential]

    ```csharp
    app.UseServiceModel(serviceBuilder =>
    {
        // Configure CoreWCF to dispatch to service type Service
        serviceBuilder.AddService<Service>();

        // Create a binding instance to use Azure Queue Storage, passing an optional queue name for the dead letter queue
        // The default client credential type is Default, which uses DefaultAzureCredential
        var aqsBinding = new AzureQueueStorageBinding("DEADLETTERQUEUENAME");

        // Add a service endpoint using the AzureQueueStorageBinding
        string queueEndpointString = "https://MYSTORAGEACCOUNT.queue.core.windows.net/QUEUENAME";
        serviceBuilder.AddServiceEndpoint<Service, IService>(aqsBinding, queueEndpointString);
    });
    ```

    #### Other Authentication Credential Mechanisms

    If you are using a different credential mechanism such as [`StorageSharedKeyCredential`][shared_key], you can configure the appropriate `ClientCredentialType` on the binding and set the credential on an `AzureServiceCredentials` instance via an extension method for `IServiceBuilder`.

    ```csharp

    app.UseServiceModel(serviceBuilder =>
    {
        ...

         // Configure the client credential type to use StorageSharedKeyCredential
        aqsBinding.Security.Transport.ClientCredentialType = AzureClientCredentialType.StorageSharedKey;
        
        ...

        // Use extension method to configure CoreWCF to use AzureServiceCredentials and set the StorageSharedKeyCredential instance.
        serviceBuilder.UseAzureCredentials<Service>(credentials =>
        {
            credentials.StorageSharedKey = GetMyStorageSharedKey();
        });
    });

    ```

    Other values for ClientCredentialType are:
    - [Sas][sas]
    - [Token][token]
    - [ConnectionString][connection_string]

    The credentials class `AzureServiceCredentials` has corresponding properties to set each of these credential types.

3. **Service Implementation**: Implement your CoreWCF service and use the Azure Queue Storage as the binding:

    ```csharp
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract(IsOneWay = true)]
        Task ProcessVideoAsync(long videoId, string videoTitle, string blobStoragePath);
    }

    public class MyService : IMyService
    {
        public async Task ProcessVideoAsync(long videoId, string videoTitle, string blobStoragePath)
        {
            await VideoEngine.ProcessVideoAsync(videoId, videoTitle, blobStoragePath);
        }
    }
    ```

### Getting Started with WCF Azure Queue Storage client library

1. **Installation**: Add the library to your project using NuGet by either of the following methods:

    - In Visual Studio, from the Package Explorer, search for Microsoft.WCF.Azure.StorageQueues.Client, or
    - Type `Install-Package Microsoft.WCF.Azure.StorageQueues.Client -AllowPrereleaseVersions` into the Package Manager Console, or
    - Type `dotnet add ./path/to/myproject.csproj package Microsoft.WCF.Azure.StorageQueues.Client --prerelease` in a terminal (replace ./path/to/myproject.csproj by the path to the *.csproj file you want to add the dependency)

2. **Authentication and Configuration**: Configure your WCF Library to use Azure Queue Storage with the appropriate endpoint and credentials. To send requests to the Azure Queue Storage service, you'll need to configure WCF with the appropriate endpoint and configure credentials. The authentication is configured in a similar manner as CoreWCF.

    ```csharp
        // Create a binding instance to use Azure Queue Storage.
        // The default client credential type is Default, which uses DefaultAzureCredential
        var aqsBinding = new AzureQueueStorageBinding();

        // Create a ChannelFactory to using the binding and endpoint address, open it, and create a channel
        string queueEndpointString = "https://MYSTORAGEACCOUNT.queue.core.windows.net/QUEUENAME";
        var factory = new ChannelFactory<IService>(aqsBinding, new EndpointAddress(queueEndpointString));
        factory.Open();
        IService channel = factory.CreateChannel();

        // IService dervies from IChannel so you can call channel.Open without casting
        channel.Open();
        await channel.ProcessVideoAsync(videoId, videoTitle, blobStoragePath);

        channel.Close();
        await (factory as IAsyncDisposable).DisposeAsync();
    ```

    #### Other Azure Queue Storage Authentication Credential Mechanisms

    If you are using a different credential mechanism such as `StorageSharedKeyCredential`, you configure the appropriate `ClientCredentialType` on the binding and set the credential on an `AzureClientCredentials` instance via an extension method for `ChannelFactory`.

    Like CoreWCF, other supported values for `ClientCredentialType` are:
    - [Sas][sas]
    - [Token][token]
    - [ConnectionString][connection_string]

3. **Client Implementation**: Implement your WCF client and use the Azure Queue Storage as the binding:

    ```csharp
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract(IsOneWay = true)]
        Task ProcessVideoAsync(long videoId, string videoTitle, string blobStoragePath);
    }

    public class MyServiceClient : ClientBase<IMyService>, IMyService
    {
        public MyServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }

        public async Task ProcessVideoAsync(long videoId, string videoTitle, string blobStoragePath)
        {
            return await Channel.ProcessVideoAsync(videoId, videoTitle, blobStoragePath);
        }
    }
    ```

## Advanced Configuration and Security

For advanced users, the libraries offer a range of configuration options, including message encoding, queue settings, and custom error handling. The most secure and recommended credential management method is to use Azure Managed Identity.

The credentials you configure above are passed to an instance of Azure Queue Storage client, which manages all authentication for the Azure Queue Storage. We do not directly authenticate against Azure ourselves.

## Conclusion

The CoreWCF and WCF Azure Queue Storage libraries for .NET bring the power and scalability of Azure Queue Storage to your CoreWCF services. Whether you're building enterprise-grade solutions or small applications, these libraries help you achieve reliable and scalable messaging.

For more detailed documentation and examples, visit the [CoreWCF Azure Queue Storage repository](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/extension-wcf/Microsoft.CoreWCF.Azure.StorageQueues) and the [WCF Azure Queue Storage client repository](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/extension-wcf/Microsoft.WCF.Azure.StorageQueues).

[corewcf-repo]: https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/extension-wcf/Microsoft.CoreWCF.Azure.StorageQueues
[wcf-repo]: https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/extension-wcf/Microsoft.WCF.Azure.StorageQueues
[identity]: https://learn.microsoft.com/dotnet/azure/sdk/authentication
[azure_queue]: https://learn.microsoft.com/azure/storage/queues/storage-queues-introduction
[managed_identity]: https://learn.microsoft.com/entra/identity/managed-identities-azure-resources/overview
[shared_key]: https://learn.microsoft.com/dotnet/api/azure.storage.storagesharedkeycredential
[sas]: https://learn.microsoft.com/azure/storage/common/storage-sas-overview
[token]: https://learn.microsoft.com/azure/storage/queues/authorize-access-azure-active-directory
[connection_string]: https://learn.microsoft.com/azure/storage/common/storage-configure-connection-string
[default_azure_credential]: https://learn.microsoft.com/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet
[CoreWCFAzureStorageNuget]: https://www.nuget.org/packages/Microsoft.CoreWCF.Azure.StorageQueues
[WCFAzureStorageNuget]: https://www.nuget.org/packages/Microsoft.WCF.Azure.StorageQueues
