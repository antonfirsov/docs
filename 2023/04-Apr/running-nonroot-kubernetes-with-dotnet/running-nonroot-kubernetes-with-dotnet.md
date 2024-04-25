---
post_title: Running non-root .NET containers with Kubernetes
author1: rlander@microsoft.com
post_slug: running-nonroot-kubernetes-with-dotnet
username: rlander@microsoft.com
microsoft_alias: rlander
featured_image: nonrootfeature.png
categories: .NET, .NET Core, ASP.NET Core, Containers, Docker, Security, Linux
summary: Learn how to run .NET containers in Kubernetes as a non-root user.
tags: .net 8, rootless, non-root, kubernetes
desired_publication_date: 2023-04-17
post_date: 2023-04-17 10:05:00
---

> This post was updated on **April 25, 2024** to reflect the latest releases.

Rootless or non-root Linux containers have been the most requested feature for the .NET container team. We recently announced that all [.NET 8 container images will be configurable as non-root](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) with a single line of code. This change is a welcome improvement in security posture. In that last post, I promised a follow-up on how to approach non-root hosting with Kubernetes. That's what we'll cover today.

You can try hosting a non-root container on your cluster with our [non-root Kubernetes sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/non-root/README.md). It is part of a larger set of [Kubernetes samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/README.md) we're working on.

This post will help you follow [Kubernetes "Restricted" hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted). Non-root hosting is a key requirement of the guidance.

## `runAsNonRoot`

Most of what we'll be discussing relates to the [`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) section of a Kubernetes manifest. It holds security configuration that is applied by Kubernetes.

```yml
    spec:
      securityContext:
        runAsNonRoot: true
      containers:
      - name: aspnetapp
        image: mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled
        ports:
        - containerPort: 8080
```

This `securityContext` object in this example validates that all the containers in the pod will be run with a non-root user. It really is as simple as that.

You can see this in a broader context in [non-root.yaml](https://github.com/dotnet/dotnet-docker/blob/3e8c396639c30387237fc898d9152858b479a458/samples/kubernetes/non-root/non-root.yaml#L15-L16).

`runAsNonRoot` tests that the user (via UID) is a non-root user (`> 0`), otherwise pod creation will fail. Kubernetes only reads container image metadata for this test.  It doesn't read `/etc/passwd` since that would require launching the container (which defeats the purpose of the test). That means that `USER` (in a Dockerfile) must be set by UID. It will fail if `USER` is set by name.

We can simulate this same test with `docker inspect`.

```bash
$ docker inspect mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled -f "{{.Config.User}}"
1654
```

As you can see, our sample image sets the user by UID. However, `whoami` will still report the user as `app`, since it is natural for it is running on a live operating system.

`runAsUser` is a related setting, although not used in the example above. `runAsUser` should only be used if `USER` in the container image is unset, set by name rather than UID, or otherwise needs to be changed. We've made it very easy to use the new `app` user as a UID, such that `runAsUser` should not be commonly needed for .NET apps.

## `USER` best practices

We recommend using the following pattern for setting `USER` in a Dockerfile.

```dockerfile
USER $APP_UID
```

The `USER` instruction is often placed just before the `ENTRYPOINT`, although the order doesn't matter.

This pattern results in the `USER` being set as a UID, while avoiding magic numbers in your Dockerfile. The environment variable already defines the UID value as declared by the .NET image.

You can see the environment variable set in .NET images.

```bash
$ docker run --rm -u app mcr.microsoft.com/dotnet/runtime-deps:8.0 bash -c "echo \$APP_UID"
1654
```

Our sample Dockerfile [set the user by UID](https://github.com/dotnet/dotnet-docker/blob/6f32b916ff36e8ff8e3f22b79c0978e4a9392c59/samples/aspnetapp/Dockerfile#L21). As a result, it works well with [`runAsNonRoot`](https://github.com/dotnet/dotnet-docker/blob/3e8c396639c30387237fc898d9152858b479a458/samples/kubernetes/non-root/non-root.yaml#L16).

## Non-root hosting in action

Let's take a look at the experience of non-root container hosting using our [non-root Kubernetes sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/non-root/README.md).

I'm using [Docker Desktop](https://docs.docker.com/desktop/kubernetes/) for my local cluster, but any Kubernetes-compatible environment should work well with [kubectl](https://kubernetes.io/docs/tasks/tools/).

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
deployment.apps/dotnet-non-root created
service/dotnet-non-root created
$ kubectl get po
NAME                           READY   STATUS    RESTARTS   AGE
dotnet-non-root-5b7fc97df7-476s9   1/1     Running   0          13s
```

The pod was correctly deployed, since the status us reported as `Running`.

`kubectl` reports that `runAsNonRoot` was set.

```bash
$ kubectl get pod dotnet-non-root-5b7fc97df7-476s9 -o jsonpath="{.spec.securityContext}" 
{"runAsNonRoot":true}
```

Now onto using the app, first by creating a proxy to the service.

```bash
$ kubectl port-forward service/dotnet-non-root 8080
Forwarding from 127.0.0.1:8080 -> 8080
Forwarding from [::1]:8080 -> 8080
```

We can now call the endpoint, which also reports the user as `app`.

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":8,"totalAvailableMemoryBytes":4113563648,"memoryLimit":0,"memoryUsage":34082816,"hostName":"dotnet-non-root-8467576789-9dj8g"}
```

`user` is displayed as `app`, as expected.

If we tried to deploy an image that used `root` as the user, with `runAsNonRoot` set to `true`, we would see the following error.

```bash
$ kubectl apply -f non-root.yaml
deployment.apps/dotnet-non-root created
service/dotnet-non-root created
$ kubectl get po
NAME                            READY   STATUS                       RESTARTS   AGE
dotnet-non-root-6df9cb77d8-74t96   0/1     CreateContainerConfigError   0          5s
```

With a bit of digging, we'd find the reason.

```bash
$ kubectl describe po | grep Error
      Reason:       CreateContainerConfigError
  Warning  Failed     7s (x2 over 8s)  kubelet            Error: container has runAsNonRoot and image will run as root (pod: "dotnet-non-root-6df9cb77d8-74t96_default(d4df0889-4a69-481a-adc4-56f41fb41c63)", container: aspnetapp)
```

With the tour over, we can delete the resources.

```bash
$ kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
deployment.apps "dotnet-non-root" deleted
service "dotnet-non-root" deleted
```

## `dotnet-monitor`

[`dotnet-monitor`](https://github.com/dotnet/dotnet-monitor) is a diagnostic tool for capturing diagnostic artifacts from running applications. We offer a [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/) container image for it. Does it work well with non-root hosting? Yes.

The [`dotnet-monitor`](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/dotnet-monitor/README.md) sample demonstrates both ASP.NET and `dotnet-monitor` running as non-root.

## Summary

You can switch to non-root hosting in Kubernetes with a few straightforward configuration changes. Your app will be more secure and more resilient to attack. This approach also brings your app into compliance with Kubernetes Pod hardening best practices. It's a small change with a big impact for defense in depth.

We hope that our container security initiative enables the entire .NET container ecosystem to switch to non-root hosting. We're invested in .NET apps in the cloud being high-performance and safe.
