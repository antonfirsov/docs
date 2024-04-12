---
post_title: Secure your .NET cloud apps with rootless Linux Containers
author1: rlander@microsoft.com
post_slug: securing-containers-with-rootless-2
microsoft_alias: rlander
featured_image: dotnet-bot_cloudapps.png
categories: .NET Core, Containers, Security
tags: .net 8, linux, containers
summary: Learn about patterns for securing your containers with a non-root user, and changes to .NET container images in .NET 8 to enable this behavior.
post_date: 2024-04-30 10:05:00
---

> This post was **updated on April 12, 2024** to reflect the latest releases.

Starting with [.NET 8](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-preview-1/#net-container-images), all of our Linux container images will [include a non-root user](https://github.com/dotnet/dotnet-docker/pull/4397). You'll be able to host your .NET containers as a non-root user with one line of code. This platform-level change will make your apps more secure and .NET one of the most secure developer ecosystems. It is a small change with a big impact for [defense in depth](https://en.wikipedia.org/wiki/Defense_in_depth_(computing)).

This change was inspired by our earlier project enabling [.NET in Ubuntu Chiseled containers](https://devblogs.microsoft.com/dotnet/dotnet-6-is-now-in-ubuntu-2204/#net-in-chiseled-ubuntu-containers). Chiseled (AKA "distroless") images are intended to be appliance-like so non-root was an easy design choice for those images. We realized that we could apply the non-root feature of Chiseled containers to all the container images we publish. By doing that, we've raised the security bar for .NET container images.

This post is about the benefit of non-root containers, workflows for creating them, and how they work. A follow-on post will discuss how to best use these images with Kubernetes. Also, if you want a simpler option, you should check out [built-in container support for the .NET SDK](https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/).

> Note: [.NET Chiseled images](https://devblogs.microsoft.com/dotnet/announcing-dotnet-chiseled-containers/) are now GA.


## Least privilege

Hosting containers as non-root aligns with [principle of least privilege](https://en.wikipedia.org/wiki/Principle_of_least_privilege). It's free security provided by the operating system. If you run your app as `root`, your app process can do anything in the container, like modify files, install packages, or run arbitrary executables. That's a concern if your app is ever attacked. If you run your app as non-root, your app process cannot do much, _greatly_ limiting what a [bad actor could accomplish](https://seclists.org/oss-sec/2019/q1/119).

Non-root containers can also be thought of as contributing to [secure supply chain](https://en.wikipedia.org/wiki/Digital_supply_chain_security). Most of the time, people talk about secure supply chain in terms of blocking bad dependency updates or [auditing component pedigree](https://en.wikipedia.org/wiki/Software_supply_chain). Non-root containers come after those two topics. If a bad dependency slips through your process (and there is a probability that one will), then a non-root container may be your best last defense. [Kubernetes hardening best practices](https://kubernetes.io/docs/concepts/security/pod-security-standards/#restricted) require running containers with a non-root user for this same reason.

## Meet `app`

All of our Linux images -- starting with .NET 8 -- will contain an `app` user. The `app` user will be able to run your app, but won't be able to delete or change any of files that come with the container image (unless you explicitly allow that). The naming is appropriate since the user can do little more than run your app.

The user `app` isn't actually new. It is the same one we're using for [our Ubuntu Chiseled images](https://github.com/dotnet/dotnet-docker/blob/6f8e695e4c0fe2708980217bc2ab2dbda68a1f79/src/runtime-deps/6.0/jammy-chiseled/amd64/Dockerfile). That's a key design point. Starting with .NET 8, all of our Linux container images will contain the `app` user. That means that you can switch between the images we offer, and the user and uid will be the same.

I'll describe the new experience in terms of the `docker` CLI.

Meet `app`.

```bash
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:8.0 cat /etc/passwd | tail -n 1
app:x:1654:1654::/home/app:/bin/sh
```

That's the last line of the [`/etc/passwd` file](https://www.redhat.com/sysadmin/linux-user-group-management) in our images. That's the file that Linux uses for managing users.

We [selected a uid just above 1000](https://github.com/dotnet/dotnet-docker/issues/4693#issuecomment-1612002902) to avoid [reserved ranges](https://en.wikipedia.org/wiki/User_identifier#Reserved_ranges). We also decided that this [user should have a home directory](https://github.com/dotnet/dotnet-docker/issues/4083).

```bash
$ docker run --rm -u app mcr.microsoft.com/dotnet/aspnet:8.0 bash -c "cd && pwd"
/home/app
```

We looked around a bit and discovered that Node.js, Ubuntu 23.04+, and Chainguard are all on this same plan. Nice!

```bash
$ docker run --rm node cat /etc/passwd | tail -n 1
node:x:1000:1000::/home/node:/bin/bash
$ docker run ubuntu:noble cat /etc/passwd | tail -n 1
ubuntu:x:1000:1000:Ubuntu:/home/ubuntu:/bin/bash
$ docker cp $(docker create --name cg cgr.dev/chainguard/dotnet-runtime):/etc/passwd ./passwd && docker rm cg && cat ./passwd | tail -n 1 && rm ./passwd
nonroot:x:65532:65532:Account created by apko:/home/nonroot:/bin/sh
```

The last one is [Chainguard](https://github.com/chainguard-images). Those images are structured differently (for good reason), so a different pattern was used. It is fine for everyone to create their own users, however, it is best to avoid matching UIDs.

There are [lots of users in container images](https://gist.github.com/richlander/6fbe3d741a3d6ed023db8f2d767f9efd), but [none of them are considered appropriate for this use case](https://en.wikipedia.org/wiki/Nobody_(username)). It would be nice to [reduce the number of users](https://github.com/alpinelinux/docker-alpine/issues/232), however, that's unlikely to happen and one of the benefits of using distroless/Chiseled images.

Windows Containers already [have a non-admin capability](https://learn.microsoft.com/virtualization/windowscontainers/manage-containers/container-security), with the `ContainerUser` user. We opted against adding `app` to our Windows Container images. You should follow Windows Team guidance on how to best secure Windows Container images.

## Using `app`

> "Non-root-capable": Configure your container as non-root with a one-line `USER` instruction.

[Docker](https://docs.docker.com/engine/reference/builder/#user) and [Kubernetes](https://kubernetes.io/docs/tasks/configure-pod-container/security-context/) make it easy to specify the user you want to use for your container. It's a one-liner. Per our definition, "non-root-capable" means you can switch to non-root as a one-liner. That's very powerful, since the ease of a one-liner removes any reason to not run more securely.

Note: `aspnetapp` is used throughout as a substitute for your app.

You can set the user via the CLI with `-u`.

```bash
$ docker run --rm -u app mcr.microsoft.com/dotnet/runtime-deps:8.0 whoami
app
```

Specifying the user via the CLI is fine, but more for testing or diagnostic scenarios. It is best for production apps to [define the `USER` in a Dockerfile](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#user), with the username or uid.

As the user:

```dockerfile
USER app
```

As the UID:

```dockerfile
USER 1654
```

Via an [environment variable for the UID](https://github.com/dotnet/dotnet-docker/issues/4506).

```dockerfile
USER $APP_UID
```

We consider this pattern a best practice because it makes it obvious which user you are using, avoids duplicated magic numbers, and uses a UID, all of which work well if you are using Kubernetes. We'll have a post on non-root hosting with Kubernetes shortly.

The following command demonstrates the value of the environment variable.

```bash
docker run --rm -u app mcr.microsoft.com/dotnet/runtime-deps:8.0 bash -c "echo \$APP_UID"
1654
```

If you don't do anything, everything will be the same as before and your image will continue to run as `root`. We hope you take the extra (small) step and run your container as the `app` user. You might be wondering why we didn't switch to the non-root user by default. That will be covered in a later section.

## Switching to port `8080`

The biggest sticking point of the project was the ports that we expose. In fact, it is so much of a sticking point that we had to make a [breaking change](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8/containers#non-root-user).

We decided to standardize on port `8080` for all container images going forward. This decision was based on our earlier experience with Chiseled images, which already [listen on port `8080`](https://github.com/dotnet/dotnet-docker/blob/6f8e695e4c0fe2708980217bc2ab2dbda68a1f79/src/runtime-deps/6.0/jammy-chiseled/amd64/Dockerfile#L50). All the images now match.

However, ASP.NET Core apps (using our .NET 7 and earlier container images) [listen on port `80`](https://github.com/dotnet/dotnet-docker/blob/305864588999a50a2112ed1ac3c39f51c04f37d7/src/runtime-deps/6.0/bullseye-slim/amd64/Dockerfile#L19). The problem is that [port 80 is a privileged port](https://www.w3.org/Daemon/User/Installation/PrivilegedPorts.html) that requires `root` permission (at least in some places). That's inherently incompatible with non-root containers.

You can see how the ports are configured in our images.

For .NET 8:

```bash
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:8.0 bash -c "echo \$ASPNETCORE_HTTP_PORTS"
8080
```

For .NET 7 (and earlier):

```bash
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:7.0 bash -c "echo \$ASPNETCORE_URLS"
http://+:80
```

Note: We also changed the environment variable we use to set the port. More on that shortly.

Going forward, your port mapping will need to change. You can do this via the CLI. You'll need `8080` on the right-hand of the mapping. The left-hand side can match or be another value.

```bash
docker run --rm -it -p 8000:8080 aspnetapp
```

Some users will want to continue to use port 80 (and `root`). You can still do that.

You can re-define `ASPNETCORE_HTTP_PORTS` in your Dockerfile or via the CLI.

For Dockerfile:

```dockerfile
ENV ASPNETCORE_HTTP_PORTS=80
```

For Docker CLI:

```bash
docker run --rm -e ASPNETCORE_HTTP_PORTS=80 -p 8000:80 aspnetapp
```

.NET 8 Windows Container images use port `8080` as well.

```bash
$ docker run --rm mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-ltsc2022 cmd /c "set | findstr ASPNETCORE"
ASPNETCORE_HTTP_PORTS=8080
```

[`ASPNETCORE_HTTP_PORTS`](https://github.com/dotnet/aspnetcore/pull/44194) is a new environment variable for specifying the port (or ports) for ASP.NET Core (actually, [Kestrel](https://learn.microsoft.com/aspnet/core/fundamentals/servers/kestrel)) to listen on. It takes a semi-colon delimited list of port values. .NET 8 images use this new environment variable, instead of `ASPNETCORE_URLS` (which is used in .NET 6 and 7 images). `ASPNETCORE_URLS` remains a useful advanced feature. It enables specifying both raw HTTP and TLS ports in one configuration and overrides both `ASPNETCORE_HTTP_PORTS` and `ASPNETCORE_HTTPS_PORTS`.

## Non-root in action

Let's take a look at what non-root looks like from a few different angles so that you can better understand what's actually going on. I'm using Ubuntu 24.04.

You can use our [aspnetapp](https://github.com/dotnet/dotnet-docker/blob/main/samples/aspnetapp/) sample to try this scenario yourself. It configures the container to always run as `app`.

```bash
$ pwd
/home/rich/git/dotnet-docker/samples/aspnetapp
$ cat Dockerfile | tail -n 2
USER $APP_UID
ENTRYPOINT ["./aspnetapp"]
$ docker build --pull -t aspnetapp -f Dockerfile .
```

Let's see if we can observe the user in action, which has been set in the [Dockerfile](https://github.com/dotnet/dotnet-docker/tree/main/samples/aspnetapp/Dockerfile) we're using.

```bash
$ docker run --rm --name aspnetapp -d -p 8000:8080 aspnetapp
5bde77feebdf76ff370815f41a8989a880d51a4037c91e2ac8c6f2c269b759ad
$ curl http://localhost:8000/Environment
{"runtimeVersion":".NET 8.0.4","osVersion":"Debian GNU/Linux 12 (bookworm)","osArchitecture":"Arm64","user":"app","processorCount":8,"totalAvailableMemoryBytes":4113694720,"memoryLimit":0,"memoryUsage":34250752,"hostName":"ead528d37b0e"}
$ docker exec aspnetapp ls -l
total 200
-rw-r--r-- 1 root root   154 Feb 22 05:46 appsettings.Development.json
-rw-r--r-- 1 root root   151 Jul 11  2023 appsettings.json
-rwxr-xr-x 1 root root 72544 Apr 12 17:11 aspnetapp
-rw-r--r-- 1 root root   457 Apr 12 17:11 aspnetapp.deps.json
-rw-r--r-- 1 root root 61952 Apr 12 17:11 aspnetapp.dll
-rw-r--r-- 1 root root 44696 Apr 12 17:11 aspnetapp.pdb
-rw-r--r-- 1 root root   469 Apr 12 17:11 aspnetapp.runtimeconfig.json
drwxr-xr-x 5 root root  4096 Apr 12 17:11 wwwroot
$ docker top aspnetapp  
UID                 PID                 PPID                C                   STIME               TTY                 TIME                CMD
1654                7833                7815                0                   17:13               ?                   00:00:00            ./aspnetapp
```

Notice the `user` property in the JSON content returned from `Environment` endpoint, above.

You can see that application is running as `app` and that the files are owned by `root`. That means that the application files are protected from being altered by this user.

From [Dockerfile reference](https://docs.docker.com/engine/reference/builder/#copy):

> All new files and directories are created with a UID and GID of 0, unless the optional `--chown` flag specifies a given username, groupname, or UID/GID combination to request specific ownership of the copied content

Let's try some rootful actions on this container, using `docker exec` on the same container.

```bash
$ docker exec aspnetapp rm aspnetapp.pdb
rm: can't remove 'aspnetapp.pdb': Permission denied
$ docker exec aspnetapp touch /file
touch: /file: Permission denied
$ docker exec aspnetapp which dotnet
/usr/bin/dotnet
$ docker exec aspnetapp rm /usr/bin/dotnet
rm: can't remove '/usr/bin/dotnet': Permission denied
$ docker exec aspnetapp apt-get update && apt-get install -y curl
Reading package lists...
E: List directory /var/lib/apt/lists/partial is missing. - Acquire (13: Permission denied)
$ docker exec aspnetapp sudo apt-get update && sudo apt-get install -y curl
OCI runtime exec failed: exec failed: unable to start container process: exec: "sudo": executable file not found in $PATH: unknown
```

The result: `Permission denied` and `sudo` isn't present to work around that. That's what we want. Let's try again, but elevate to `root`.

```bash
$ docker exec -u root aspnetapp bash -c "rm aspnetapp.pdb && ls aspnetapp.pdb"
ls: aspnetapp.pdb: No such file or directory
$ docker exec -u root aspnetapp bash -c "touch /file && ls /file"
/file
$ docker exec -u root aspnetapp bash -c "rm /usr/bin/dotnet &&  ls /usr/bin/dotnet"
ls: /usr/bin/dotnet: No such file or directory
$ docker exec -u root aspnetapp bash -c "apt-get update && apt-get install curl"   
Get:1 http://deb.debian.org/debian bookworm InRelease [151 kB]
Get:2 http://deb.debian.org/debian bookworm-updates InRelease [55.4 kB]
Get:3 http://deb.debian.org/debian-security bookworm-security InRelease [48.0 kB]
Get:4 http://deb.debian.org/debian bookworm/main arm64 Packages [8685 kB]
Get:5 http://deb.debian.org/debian bookworm-updates/main arm64 Packages [12.5 kB]
Get:6 http://deb.debian.org/debian-security bookworm-security/main arm64 Packages [147 kB]
Fetched 9099 kB in 2s (4099 kB/s)
Reading package lists...
Reading package lists...
Building dependency tree...
Reading state information...
The following additional packages will be installed:
  krb5-locales libbrotli1 libcurl4 libgssapi-krb5-2 libk5crypto3 libkeyutils1
  libkrb5-3 libkrb5support0 libldap-2.5-0 libldap-common libnghttp2-14 libpsl5
  librtmp1 libsasl2-2 libsasl2-modules libsasl2-modules-db libssh2-1
  publicsuffix
...
curl 7.88.1 (aarch64-unknown-linux-gnu) libcurl/7.88.1 OpenSSL/3.0.11 zlib/1.2.13 brotli/1.0.9 zstd/1.5.4 libidn2/2.3.3 libpsl/0.21.2 (+libidn2/2.3.3) libssh2/1.10.0 nghttp2/1.52.0 librtmp/2.3 OpenLDAP/2.5.13
Release-Date: 2023-02-20, security patched: 7.88.1-10+deb12u5
Protocols: dict file ftp ftps gopher gophers http https imap imaps ldap ldaps mqtt pop3 pop3s rtmp rtsp scp sftp smb smbs smtp smtps telnet tftp
Features: alt-svc AsynchDNS brotli GSS-API HSTS HTTP2 HTTPS-proxy IDN IPv6 Kerberos Largefile libz NTLM NTLM_WB PSL SPNEGO SSL threadsafe TLS-SRP UnixSockets zstd
```

We can now kill the container.

```bash
$ docker kill aspnetapp
aspnetapp
$ docker ps
CONTAINER ID   IMAGE     COMMAND   CREATED   STATUS    PORTS     NAMES
```

You can see that `root` is able to do a lot more, in fact, anything it wants. After `curl` is installed into the Debian image, an attacker could start executing scripts from any webserver they choose. In the case of Alpine, it ships with `wget`, which removes a step in that chain.

Surely, the answer is to remove the `root` user to avoid these risks. No. In fact, removing the `root` user has undefined behavior. The best option is to run as a non-root user. It removes a whole class of attacks via well-defined mechanisms.

The use of `docker exec -u root` might seem scary. If an attacker can run `docker exec -u root` on your running container, then they already have access to the host, and you're already in far more trouble than anything that is addressed by this post.

What about `sudo`? `sudo` isn't included in our images and never will be.

## Chiseled

We have a [similar sample](https://github.com/dotnet/dotnet-docker/blob/822166bbf88e23cbbbba6fedc3f56b99276a1e96/samples/aspnetapp/Dockerfile.chiseled) that is hosted at `mcr.microsoft.com`. It is more limited in what it can do, by design, by virtue of being based on an [Ubuntu Chiseled image](https://devblogs.microsoft.com/dotnet/announcing-dotnet-chiseled-containers/).

```bash
$ docker run --rm -d --name aspnetapp -p 8000:8080 mcr.microsoft.com/dotnet/samples:aspnetapp-chiseled 
1d2120f991562e51b65dd09fba693a68f6230e914c75e21a93811219bb52c16c
$ curl http://localhost:8000/Environment 
{"runtimeVersion":".NET 8.0.4","osVersion":"Ubuntu 22.04.4 LTS","osArchitecture":"Arm64","user":"app","processorCount":8,"totalAvailableMemoryBytes":4113694720,"memoryLimit":0,"memoryUsage":37683200,"hostName":"1d2120f99156"}
$ docker kill aspnetapp
aspnetapp
```

This image also uses the `app` user.

## Hosting in Azure container services

It is straightforward to adopt this pattern in Azure container services. There are two aspects to consider, the port and the user.

Some container services offer a higher-level experience than Kubernetes and require a different configuration option.

- Azure App Service requires [`WEBSITES_PORT`](https://learn.microsoft.com/azure/app-service/configure-custom-container#configure-port-number) to use a port other than port 80. It can be set via the CLI or in the portal.
- Azure Container Apps enables [changing the port as part of resource creation](https://learn.microsoft.com/azure/container-apps/get-started-existing-container-image-portal?pivots=container-apps-public-registry).
- Azure Container Instances enables [changing the port as part of resource creation](https://learn.microsoft.com/azure/container-instances/container-instances-quickstart-portal).

None of those services offer an obvious way to change the user. If you set the user in your Dockerfile (which is a best practice), then there is no need for that capability.

We've started to spread the word about this change to other clouds.

## Next steps

The next step is to investigate the cases where non-root could be a challenge, such as for diagnostic scenarios. Some of the examples use `docker exec -u root`. That works well in a local environment, however `kubectl exec` [doesn't offer a user argument](https://github.com/kubernetes/kubernetes/issues/30656). We'll look more deeply at Kubernetes workflows with non-root in a later post.

We're also going to continue working with container hosting services to ensure that .NET developers can move to .NET 8 container images with ease, particularly those providing higher-level experiences like Azure App Service.

## Summary

A key part of our mission on the .NET Team is defense in depth. Everyone needs to think about security, however, we are in the business of closing off whole classes of attack with a single change or feature. To be true, we could have made this change when we started publishing container images about a decade ago. We have been asked for non-root guidance and non-root container images for many years. It honestly wasn't clear to us how to approach that, in large part because the pattern we're now using didn't exist when we started out. There wasn't a leader in safe container hosting for us to learn from. It was the experience of working with Canonical on chiseled images that enabled us to discover and shape this approach.

We hope that this initiative enables the entire .NET container ecosystem to switch to non-root hosting. We're invested in .NET apps in the cloud being high-performance and safe.
