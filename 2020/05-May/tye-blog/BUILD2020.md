
# Project Tye

[Project Tye](https://github.com/dotnet/tye) is an experimental developer tool that makes developing, testing, and deploying microservices and distributed applications easier. The project has two main goals:

1. Making development of microservices easier by:
    - Run many services with one command
    - Use dependencies in containers
    - Discover addresses of other services using simple conventions
1. Automating deployment of .NET applications to Kubernetes by:
   - Automatically containerizing .NET applications
   - Generating Kubernetes manifests with minimal knowledge or configuration
   - Using the same conventions as development to keep it consistent

## Tour of Tye

### Installation

To get started with Tye, you will first need to have .[NET Core 3.1](https://dotnet.microsoft.com/download) installed on your machine. 

Tye can then be installed as a global .NET tool using the following command:
```
dotnet tool install -g Microsoft.Tye --version "0.2.0-alpha.20258.3"
```

To verify that the installation was successful and to see the current running version of Tye, you can use the version command as shown below:

```
tye --version
```

To display a list of other commands available for Tye, run:
```
tye --help
```

If your applications have external dependencies that are Docker images, then you will also need to install [Docker Desktop](https://www.docker.com/products/docker-desktop) for either Windows/MacOS/Linux depending on your machine and current operating system. This is required so that Tye can build and run those images locally. 

If you wish to deploy your application to Kubernetes using Tye, then you will also need to have Kubernetes installed on your machine. You can enable Kubernetes through Docker Desktop or explore different options for a Kubernetes cluster such as [Azure Kubernetes Service](https://azure.microsoft.com/en-us/services/kubernetes-service/) (AKS) and [Azure Container Registry](https://azure.microsoft.com/en-us/services/container-registry/) (ACR).

### Setting up Tye for local development
Tye makes it much easier to build and run multi-service appplications locally on your machine. One way that Tye accomplishes this, is by generating a manifest yaml file that contains all of your projects and external dependencies. If you have an existing solution, Tye will automatically populate this file with all of your current projects. 

To initalize this file and use Tye for local development, you will need to run the following command in the solution directory:

```
tye init
```

The sample application below displays an example of the generated `tye.yaml` file output for a multi-services solution. This application consists of multiple projects including a frontend, backend, and an external dependency on Redis for storing data and caching the results of the backend API.

![tye-yaml-redis](https://user-images.githubusercontent.com/20052391/82242125-2fd1af00-98f2-11ea-9f12-079ff007346b.PNG)

Two services were added to the `tye.yaml` file above. The `redis` service itself and a `redis-cli` service that we will use to watch the data being sent to and retrieved from redis.

> The `"${host}:${port}"` format in the `connectionString` property will substitute the values of the host and port number to produce a connection string that can be used with StackExchange.Redis.

To learn more about Tye's yaml specifications and schema, you can check it out [here](https://github.com/dotnet/tye/blob/master/docs/reference/schema.md) in Tye's repository on Github.

### Running your multi-services using service discovery
Once the yaml file has been initalized, Tye uses this to build and run all of your projects and external dependencies locally.

To begin the build process, use the run command as shown below:

```
tye run
```
Continuing with the sample multi-service application from the previous section, this is a snippet of the console output that gets generated when running the command above:

![tye-run-output](https://user-images.githubusercontent.com/20052391/82242069-129ce080-98f2-11ea-93d4-ae84ebf7a476.PNG)

This shows how Tye is processing, listening, and building all of the services contained in the yaml file. A key feature from this output is the dashboard that gets generated. 

![tye-dashboard](https://user-images.githubusercontent.com/20052391/82242209-54c62200-98f2-11ea-954a-24f283e91491.PNG)

The dashboard is the UI for Tye that displays a list of all of your services. It also allows you to view the running logs for each service - not only just for local services, but also for services running in containers. The dashboard also contains the port bindings for each service. Tye automatically assigns ports for each service to avoid common issues like port conflicts.

![tye-logs](https://user-images.githubusercontent.com/20052391/82242273-6dced300-98f2-11ea-90dc-29817fe1d794.PNG)

So now you may be thinking, well how does each service know about all of the other services to successfully run the application?

To help your services communicate effectively with each other, Tye utilizes service discovery. In general terms, service discovery describes the process by which one service figures out the address of another service. Tye uses environment variables for specifying connection strings and URIs of services.

The simple way to use Tye's service discovery is through the `Microsoft.Extensions.Configuration` system - available by default in ASP.NET Core or .NET Core Worker projects. In addition to this, we provide the `Microsoft.Tye.Extensions.Configuration` package with some Tye-specific extensions layered on top of the configuration system.

To access URIs use the `GetServiceUri()` extension method and provide the service name.

```c#
// Get the URI of the 'backend' service and create an HttpClient.
var uri = Configuration.GetServiceUri("backend");
var httpClient = new HttpClient()
{
    BaseAddress = uri
};
```

`URIs` are available by default for all of your services and bindings. A `URI` will not be available through the service discovery system for bindings that provide a `connectionString` in config.

To access a connection string, use the `GetConnectionString()` method and provide the service name.


```c#
// Get the connection string of the 'postgres' service and open a database connection.
var connectionString = Configuration.GetConnectionString("postgres");
using (var connection = new NpgsqlConnection(connectionString))
{
    ...
}
```

Connection strings will be available for bindings that use the `connectionString` property in configuration. Specifying a connection string in `tye.yaml` will usually involve the use of templating to fill in values that are provided by Tye.

*Example: Redis*

```yml
services:
- name: redis
  image: redis
  bindings:
  - port: 6379
    connectionString: ${host}:${port}
```
This fragment will launch `redis` when used with `tye run` on port `6379` (the typical listening port for Redis) *and* provide a connection string to other services with the value of `localhost:6379`.

To see more in-depth explanations and examples centered around service discovery and Tye's philosphy on the subject, check out this [reference doc](https://github.com/dotnet/tye/blob/master/docs/reference/service_discovery.md) in Tye's Github repository.

### Deploying your applications to Kubernetes
Tye makes the process of deploying your application to Kubernetes very simple with minimal knowlege or configuration required.

> *Tye will use your current credentials for pushing Docker images and accessing kubernetes clusters. If you have configured kubectl with a context already, that's what [`tye deploy`](/docs/reference/commandline/tye-deploy.md) is going to use!*

Prior to deploying your application, make sure to have the following:

1. Docker installed based off on your operating system
1. A container registry. Docker by default will create a container registry on [DockerHub](). You could also use [Azure Container Registry]() (ACR) or another container registry of your choice.
1. A Kubernetes Cluster. There are many different options here, including:
   - [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/tutorial-kubernetes-deploy-cluster)
   - [Kubernetes in Docker Desktop](https://www.docker.com/blog/docker-windows-desktop-now-kubernetes/)
   - [Minikube](https://kubernetes.io/docs/tasks/tools/install-minikube/)
   - [K3s](https://k3s.io) - *a lightweight single-binary certified Kubernetes distribution from Rancher*.
   - Another Kubernetes provider of your choice.

> *If you choose a container registry provided by a cloud provider (other than Dockerhub), you will likely have to take some steps to configure your kubernetes cluster to allow access. Follow the instructions provided by your cloud provider.*  

Now that we have our sample application running locally, let's deploy the application. In this example, we will deploy to Kubernetes by using `tye deploy`.

You can deploy your application by running the follow command:
```
tye deploy --interactive
```
> *Enter the Container Registry (ex: `example.azurecr.io` for Azure or `example` for dockerhub):*


You will be prompted to enter your container registry. This is needed to tag images, and to push them to a location accessible by kubernetes.

![tye-deploy-output](https://user-images.githubusercontent.com/20052391/82242391-9c4cae00-98f2-11ea-9f30-cd9f55e1120b.PNG)

If you are using dockerhub, the registry name will your dockerhub username. If you are a standalone container registry (for instance from your cloud provider), the registry name will look like a hostname, eg: `example.azurecr.io`.

`tye deploy` does many different things to deploy an application to Kubernetes. It will:

- Create a docker image for each project in your application.
- Push each docker image to your container registry.
- Generate a Kubernetes `Deployment` and `Service` for each project.
- Apply the generated `Deployment` and `Service` to your current Kubernetes context.

![tye-deploy-output2](https://user-images.githubusercontent.com/20052391/82242449-b5555f00-98f2-11ea-884f-9a42e3257bea.PNG)


You should now see three pods running after deploying.

```
kubectl get pods
```

![kubernetes-pods](https://user-images.githubusercontent.com/20052391/82242484-c3a37b00-98f2-11ea-821c-ff485fc735b5.PNG)

You'll have three services in addition to the built-in kubernetes service.

```
kubectl get service
```
![kubernetes-services](https://user-images.githubusercontent.com/20052391/82242490-c56d3e80-98f2-11ea-909d-61edcf3f3fb6.PNG)

You can visit the frontend application, you will need to port-forward to access the frontend from outside the cluster.

```
kubectl port-forward svc/frontend 5000:80
```

Now navigate to http://localhost:5000 to view the frontend application working on Kubernetes.

![port-forwarding](https://user-images.githubusercontent.com/20052391/82242541-e2097680-98f2-11ea-9316-052c8a92d42c.PNG)

> *Currently tye does not automatically enable TLS within the cluster, and so communication takes place over HTTP instead of HTTPS. This is typical way to deploy services in kubernetes - we may look to enable TLS as an option or by default in the future.*

If you want to use `tye deploy` as part of a CI/CD system, it's expected that you'll have a `tye.yaml` file initalized. You will then need to add a container registry to `tye.yaml`. Based on what container registry you configured, add the following line in the `tye.yaml` file:

```
registry: <registry_name>
```

Now it's possible to use `tye deploy` without `--interactive` since the registry is stored as part of configuration

> *This step may not make much sense if you're using tye.yaml to store a personal Dockerhub username. A more typical use case would storing the name of a private registry for use in a CI/CD system*

For a conceptual overview of how Tye behaves when using `tye deploy` for deployment, check out this [document](https://github.com/dotnet/tye/blob/master/docs/reference/deployment.md).
### Undeploying your application

After deploying and playing around with the application, you may want to remove all resources associated from the Kubernetes cluster. You can remove resources by running:

```
tye undeploy
```

This will remove all deployed resources. If you'd like to see what resources would be deleted, you can run:

```
tye undeploy --what-if
```

### Tutorials
If you want to experiment more with using Tye, we have a variety of different sample applications and tutorials that you can walk through, check them out down below:

* [Tye tutorials](https://github.com/dotnet/tye/blob/master/docs/tutorials/hello-tye/00_run_locally.md)
* [Tye samples](https://github.com/dotnet/tye/tree/master/samples)

## Tye Roadmap

We have been diligently working on adding new capabilities and integrations to continuously improve Tye. Here are some of the things below that we have recently released. There is also information provided on how to get started for each of these:

* [Ingress](https://github.com/dotnet/tye/blob/master/docs/recipes/ingress.md) - *to expose pods/services created to the public internet*.
* [Redis](https://github.com/dotnet/tye/blob/master/docs/tutorials/hello-tye/02_add_redis.md) - *to store data, cache, or as a message broker*.
* [Dapr](https://github.com/dotnet/tye/blob/master/docs/recipes/dapr.md) - *for integrating a Dapr application with Tye*.
* [Zipkin](https://github.com/dotnet/tye/blob/master/docs/recipes/distributed_tracing.md) - *using Zipkin for distributed tracing*. 
* [Elastic Stack](https://github.com/dotnet/tye/blob/master/docs/recipes/logging.md) - *using Elastic Stack for logging*.

While we are excited about the promise Tye holds, it's an experimental project and not a committed product. During this experimental phase we expect to engage deeply with anyone trying out Tye to hear feedback and suggestions. The point of doing experiments in the open is to help us explore the space as much sa we can and use what we learn to determine what we should be building and shipping in the Future.

Project Tye is currently commited as an experiment until .NET 5 ships. At which point we will be evaluating what we have and all that we've learnt to decide what we should do in the future.

Our goal is to [ship every month](https://github.com/dotnet/tye/releases), and some neew capabilities that we are looking into for Tye include:

- More deployment targets
- Sidecar support
- Connected development
- Database migrations

## Conclusion

We are excited by the potential Tye has to make developing distributed applications easier and we need your feedback to make sure it reaches that potential. We'd really love for you to try it out and tell us what you think, there is a link to a survey on the Tye dashboard that you can fill out or you can create issues and talk to us on GitHub. Either way we'd love to hear what you think.
