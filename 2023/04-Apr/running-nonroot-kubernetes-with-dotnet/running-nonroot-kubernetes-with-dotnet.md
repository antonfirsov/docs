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

Rootless or non-root Linux containers have been the most requested feature for the .NET container team. We recently announced that all [.NET 8 container images will be configurable as non-root](https://devblogs.microsoft.com/dotnet/securing-containers-with-rootless/) with a single line of code. This change is a welcome improvement in security posture. In that last post, I promised a follow-up on how to approach non-root hosting with Kubernetes. That's what we'll cover today.

You can try hosting a non-root container on your cluster with our [non-root Kubernetes sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/non-root/README.md). It is part of a larger set of [Kubernetes samples](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/README.md) we're working on.

This post will help you follow [Kubernetes "Restricted" hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted). Non-root hosting is a key requirement of the guidance.

## `runAsNonRoot`

Most of what we'll be discussing relates to the [`SecurityContext`](https://kubernetes.io/docs/reference/generated/kubernetes-api/v1.24/#securitycontext-v1-core) section of a Kubernetes manifest. It holds security configuration that is applied by Kubernetes.

```yml
    spec:
      containers:
      - name: aspnetapp
        image: dotnetnonroot.azurecr.io/aspnetapp
        securityContext:
          runAsNonRoot: true
```

This `securityContext` object validates that the container will be run with a non-root user. It really is as simple as that.

You can see this in a broader context in [non-root.yaml](https://github.com/dotnet/dotnet-docker/blob/23b937b9aecfcbffdbb4d1cc25049abe38377f76/samples/kubernetes/non-root/non-root.yaml#L20-L22).

`runAsNonRoot` tests that the user (via UID) is a non-root user (`> 0`), otherwise pod creation will fail. Kubernetes only reads container image metadata for this test.  It doesn't read `/etc/passwd` since that would require launching the container (which defeats the purpose of the test). That means that `USER` (in a Dockerfile) must be set by UID. It will fail if `USER` is set by name.

We can simulate this same test with `docker inspect`.

```bash
% docker inspect dotnetnonroot.azurecr.io/aspnetapp -f "{{.Config.User}}"
64198
```

As you can see, our sample image sets the users by UID. However, `whoami` will still report the user as `app`.

`runAsUser` is a related setting, although not used in the example above. `runAsUser` should only be used if `USER` in the container image is unset, set by name rather than UID, or otherwise undesired. We've made it very easy to use the new `app` user as a UID, such that `runAsUser` should never be needed for .NET apps.

## `USER` best practices

We recommend using the following pattern for setting `USER` in a Dockerfile.

```dockerfile
USER $APP_UID
```

The `USER` instruction is often placed just before the `ENTRYPOINT`, although the order doesn't matter.

This pattern results in the `USER` being set as a UID, while avoiding magic numbers in your Dockerfile. The environment variable already defines the UID value as declared by the .NET image.

You can see the environment variable set in .NET images.

```bash
% docker run mcr.microsoft.com/dotnet/runtime-deps:8.0-preview bash -c "export | grep UID"
declare -x APP_UID="64198"
```

Our non-root sample [sets the user by UID](https://github.com/dotnet/dotnet-docker/blob/6f8bc3bf46a990fb954506383ec360c6ac5b7d91/samples/aspnetapp/Dockerfile.alpine-non-root#L29) according to this pattern. As a result, it works well with [`runAsNonRoot`](https://github.com/dotnet/dotnet-docker/blob/23b937b9aecfcbffdbb4d1cc25049abe38377f76/samples/kubernetes/non-root/non-root.yaml#L22).

## Non-root hosting in action

Let's take a look at the experience of non-root container hosting using our [non-root Kubernetes sample](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/non-root/README.md).

I'm using [minikube](https://minikube.sigs.k8s.io/docs/start/) for my local cluster, but any Kubernetes-compatible environment should work well with [kubectl](https://kubernetes.io/docs/tasks/tools/).

```bash
$ kubectl apply -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
deployment.apps/dotnet-non-root created
service/dotnet-non-root created
$ kubectl get po
NAME                           READY   STATUS    RESTARTS   AGE
dotnet-non-root-68f4cd45c-687zp   1/1     Running   0          13s
```

The app is running. Let's check the user.

```bash
$ kubectl exec dotnet-non-root-68f4cd45c-687zp -- whoami
app
```

We can also call an endpoint on the app. First, we need to create a proxy to it.

```bash
% kubectl port-forward service/dotnet-non-root 8080
```

We can now call the endpoint, which also reports the user as `app`.

```bash
% curl http://localhost:8080/Environment
{"runtimeVersion":".NET 8.0.0-preview.3.23174.8","osVersion":"Linux 5.15.49-linuxkit #1 SMP PREEMPT Tue Sep 13 07:51:32 UTC 2022","osArchitecture":"Arm64","user":"app","processorCount":4,"totalAvailableMemoryBytes":4124512256,"memoryLimit":0,"memoryUsage":35004416}
```

Delete the resources.

```bash
$ kubectl delete -f https://raw.githubusercontent.com/dotnet/dotnet-docker/main/samples/kubernetes/non-root/non-root.yaml
deployment.apps "dotnet-non-root" deleted
service "dotnet-non-root" deleted
```

At the time of writing, our [official samples](https://mcr.microsoft.com/product/dotnet/samples/about) have not yet moved to use a non-root user. We'll do that when we move the samples to .NET 8, probably with .NET 8 RC1. We can use our `aspnetapp` image to demonstrate what happens when `runAsNonRoot` is used with an image that uses root. It should fail, right?

I'll change the manifest a bit. Let's start without the `securityContext` section.

```yml
    spec:
      containers:
      - name: aspnetapp
        image: mcr.microsoft.com/dotnet/samples:aspnetapp
```

Let's check the user.

```bash
$ kubectl apply -f non-root.yaml
deployment.apps/dotnet-non-root created
service/dotnet-non-root created
$ kubectl get po
NAME                            READY   STATUS    RESTARTS   AGE
dotnet-non-root-85768f6c55-pb5gh   1/1     Running   0          1s
$ kubectl exec dotnet-non-root-85768f6c55-pb5gh -- whoami
root
```

That's expected. Now, let's add `runAsNonRoot` back.

```yml
    spec:
      containers:
      - name: aspnetapp
        image: mcr.microsoft.com/dotnet/samples:aspnetapp
        securityContext:
          allowPrivilegeEscalation: false
          runAsNonRoot: true
```

Let's see how this check works.

```bash
$ kubectl apply -f non-root.yaml
deployment.apps/dotnet-non-root created
service/dotnet-non-root created
$ kubectl get po
NAME                            READY   STATUS                       RESTARTS   AGE
dotnet-non-root-6df9cb77d8-74t96   0/1     CreateContainerConfigError   0          5s
```

That failed, which is what we wanted. We can get a bit more information on the reason.

```bash
$ kubectl describe po | grep Error
      Reason:       CreateContainerConfigError
  Warning  Failed     7s (x2 over 8s)  kubelet            Error: container has runAsNonRoot and image will run as root (pod: "dotnet-non-root-6df9cb77d8-74t96_default(d4df0889-4a69-481a-adc4-56f41fb41c63)", container: aspnetapp)

```

We can try to `kubectl exec` to the pod, but it will fail. That demonstrates that Kubernetes blocked the container from being created (as the error states).

```bash
$ kubectl exec dotnet-non-root-6df9cb77d8-74t96 -- whoami
error: unable to upgrade connection: container not found ("aspnetapp")
```

## `dotnet-monitor`

[`dotnet-monitor`](https://github.com/dotnet/dotnet-monitor) is a diagnostic tool for capturing diagnostic artifacts from running applications. We offer a [dotnet/monitor](https://hub.docker.com/_/microsoft-dotnet-monitor/) container image for it. Does it work well with non-root hosting? Yes.

The [`hello-dotnet`](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/hello-dotnet/README.md) Kubernetes sample demonstrates both ASP.NET and `dotnet-monitor` running as non-root. It also goes on to demonstrate collecting Prometheus metrics data, both in the [cloud](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/hello-dotnet/README.md#monitor-with-prometheus) and [locally](https://github.com/dotnet/dotnet-docker/blob/main/samples/kubernetes/dotnet-monitor/README.md#monitor-with-prometheus).

## Summary

You can switch to non-root hosting in Kubernetes with a few straightforward configuration changes. Your app will be more secure and more resilient to attack. This approach also brings your app into compliance with Kubernetes Pod hardening best practices. It's a small change with a big impact for defense in depth.

We hope that our container security initiative enables the entire .NET container ecosystem to switch to non-root hosting. We're invested in .NET apps in the cloud being high-performance and safe.

To learn more about .NET 8 and other features coming to .NET head to [dot.net/next](https://dot.net/next).