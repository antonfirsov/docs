# Collecting .NET Core Linux Container CPU Traces from a Sidecar Container

## Introduction

In recent years, containerization has gained popularity in DevOps due it’s
valuable capacities, including more efficient resource utilization and better
agility. Microsoft and Docker have been working together to create a great
experience for running .NET applications inside containers. See the following
blog posts for more information

- [Using .NET and Docker Together](https://blogs.msdn.microsoft.com/dotnet/2017/05/25/using-net-and-docker-together/)
- [Using .NET and Docker Together – DockerCon 2018 Update](https://blogs.msdn.microsoft.com/dotnet/2018/06/13/using-net-and-docker-together-dockercon-2018-update/)

When there’s a performance problem, analyzing the problem often requires
detailed information about what was happening at the time.
[Perfcollect](https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/linux-performance-tracing.md)
is the recommended tool for gathering .NET Core performance data on Linux.
Containers bring challenges in using `perfcollect`. There are several ways to
collect performance traces .NET Core application running in a Linux container,
each has its cons:

- Collecting from the host
  - Process Ids and file system in the host don't match those in the containers
  - `perfcollect` cannot find container’s files under host paths (what is `/usr/share/dotnet/`?)
  - Some container OSes (e.g., CoreOS) do not support installing common
    packages/tools, for examples, `linux-tools` and `lttng` which are required by
    `perfcollect` tool.
- Collecting from the container running the application
  - Installation of profiling tools bloat the container and increase the attack surface.
  - Profiling affects the application performance in the same container (e.g.,
    its resource consumption is counted against quota)
  - `perf` tool needs capabilities to run from a container which defeats the
    security feature of containers
- Collecting from another "sidecar" container running on the same host
  - Possible environment mismatches between sidecar container and application
    container

Tim Gross published [a blog post on debugging python containers in
production](http://blog.0x74696d.com/posts/debugging-python-containers-in-production/).
His approach is to run tools inside another (sidecar) container on the same host
as the application container. The idea can be applied to profiling/debugging
.NET Core Linux containers. This approach has the following benefits:

- Application containers don’t need elevated privileges.
- Application container images remain mostly unchanged. They are not bloated by
  tool packages that are not required to run applications.
- Profiling does not consume application container resources which is usually
  throttled by a quota
- Sidecar container can be built as close to the application container as
  possible, so tools used by `perfcollect`, for example, `crossgen` and
  `objcopy`., could operate on files of the same versions at the same paths,
  even they are in different containers.

This article only describes the manual/one-off performance investigation
scenario. However, with additional effort the approach could work in an
automated way and/or under an orchestrator. See
[this tutorial](https://developer.ibm.com/recipes/tutorials/profiling-applications-deployed-on-kubernetes-with-sidecar-injector/)
for an example on profiling with a sidecar container in a Kubernetes environment.

**Note**: tracing .NET Core events using `LTTng` is not supported in this sidecar
approach due to how `LTTng` works (using shared memory and CPU buffer) so we
cannot collect events from the .NET Core runtime using this approach.

The rest of this doc gives a step-by-step guide of using a sidecar container to
collect CPU trace of an ASP.NET application running in a Linux container.

## Building Container Images

1. Build the base image. This image will be used to build both the application
   and the sidecar containers. The following `Dockerfile.base` example uses an
   ASP.NET Web API project that is created by the command `dotnet new webapi -o
   webapi`. In the builder stage, `dotnet restore` is executed twice, the first
   time with `-r linux-x64` argument to download `crossgen` nuget.org.

**link to Dockerfile.base to be inserted**

1. Run the following command to build the base image

```
docker build . -f Dockerfile.base -t application-base
```

1. Build the application container image. The content of Dockerfile for the
   application is listed below.

   **link to Dockerfile.app to be inserted**

   The `COMPlus_PerfMapEnabled` environment variable is required to properly
   resolve symbols for .NET code. When it is set, .NET Core generates symbol
   mapping files (`perf*.info`) under the `/tmp` directory which is then used by
   `perfcollect` to associate symbols with call stacks.

   In the example above, the environment variable is set in the Dockerfile.
   Other options of setting this environment variable include passing them
   through `docker -e` options; or setting them in the application startup
   script if there exists one.

   We can also use two additional settings to disable JIT if there are problems
   resolving .NET symbols. Note that this might affect the application start-up
   performance.

```
COMPlus_ZapDisable=1
COMPlus_ReadyToRun=0
```

1. Run this command to build the application container image

```
docker build . -f Dockerfile.app -t application_tag
```

1. Create the sidecar container image. Derive from the base image so that we
   have the same installation paths for .NET Core. Add the tools that are
   required for profiling or debugging.

   **link to Dockerfile.sidecar to be inserted**

   In the example, the most important packages are: `linux-tools`,
   `lttng-tools`, `liblttng-ust-dev`, `zip`, `curl`, `binutils` (for
   `objcopy`/`objdump` commands) and `procps` (for `ps` command). The
   `perfcollect` script is downloaded and saved to `/tools` directory. Other
   tools can be installed as needed for diagnosing and debugging purposes.

1. Build the sidecar image by running the following command

```
docker build . -f Dockerfile.sidecar -t sidecar_tag
```

## Running Docker Containers

1. The Linux `perf` tool needs to access the `perf*.map` files that are
   generated by the .NET Core application. By default, containers are isolated
   thus the `*.map` files generated inside the application container are not
   visible to `perf` tool running inside of the sidecar container. We need to make
   these `*.map` files available to `perf` tool running inside the sidecar.

   In this example we use docker volume mount to map a directory on the host to
   the `/tmp` directory of both the application container and the sidecar container.
   Since both of their `/tmp` directory is backed by the same volume the sidecar
   container can access files written by the application container.

   Run the application container with a name (`application` in this example); map
   the `/tmp` folder to an existing host directory `/home/core/shared_volume/tmp`.

```
docker run -it -p 80:80 -v /home/core/shared_volume/tmp:/tmp --name application application_tag
```

   Volume mount might not be desirable in some cases. Another option is to run
   the application container without the `-v` options then use `docker cp`
   commands to copy the `/tmp/perf*.map` files from the application container to
   the sidecar container’s `/tmp` folder before running the perfcollect tool. See
   step 9) if this is the case

1. Run the sidecar using the `pid` and `net` namespaces of the application
   container, and with /tmp mapped to the same host folder for tmp. Give this
   container a name (`sidecar` in this example) since it’s easier to refer to the
   container by using its name.

   Linux namespaces isolates containers and make resources they are using
   invisible to other containers by default, however we can make docker
   containers to share namespaces using the options like `--pid`, `--net`, etc.
   Here’s [a wiki link](https://en.wikipedia.org/wiki/Linux_namespaces) to read
   more about Linux namespaces.

   The command below lets the `sidecar` container share the same `pid` and `net`
   namespaces with the application container so that it is allowed to debug or
   profile processes in the application container from the sidecar container.
   The `--cap-add ALL --privileged` switches grant the sidecar container
   permissions to collect performance traces.

```
docker run -it --pid=container:application --net=container:application -v /home/core/shared_volume/tmp:/tmp --cap-add ALL --privileged --name sidecar sidecar_tag bash
```

1. (**Alternative**) if volume mount is not used in the previous two steps, we
   need to copy the `*.map` files to sidecar container so that `perfcollect` can
   access them. Find out the file names in the application container then copy
   those files to the sidecar container. The point is that `perfcollect` expects
   to find these map files under `/tmp`.

   On the host, run the following commands

```
docker exec application /bin/ls -al /tmp/*.map
```

   You should see output similar to

```
-rw-r--r--. 1 root root 1215081 May 19 08:56 tmp/perf-1.map
-rw-r--r--. 1 root root   23828 May 18 17:21 tmp/perfinfo-1.map
```

   Copy the files from the `application` container to the host

```
docker cp application:/tmp/perf-1.map /tmp/
docker cp application:/tmp/perfinfo-1.map /tmp/
```

   Then copy the files from the host to the `sidecar` container's `/tmp` directory

```
docker cp /tmp/perf-1.map sidecar:/tmp/
docker cp /tmp/perfinfo-1.map sidecar:/tmp/
```

## Collection CPU Performance Traces

1. Inside the sidecar container, collect CPU traces for the `dotnet` process (or
   your .NET Core application process if it is published as self-contained),
   which usually has PID of 1, but may vary depending on what else you are
   running in the `application` container before running the application.


```
ps -aux
```

   Output should be similar to the following

```
USER        PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root          1  1.1  0.5 7511164 82576 pts/0   SLsl+ 18:25   0:03 dotnet webapi.dll
root        104  0.0  0.0  18304  3332 pts/0    Ss   18:28   0:00 bash
root        198  0.0  0.0  34424  2796 pts/0    R+   18:31   0:00 ps -aux
```

   In this example, the `dotnet` process has PID of 1 so when running the
   `perfcollect` script, pass the PID of the 1 to the `-pid` option.

```
/tools/perfcollect collect sample -nolttng -pid 1
```

   By using the `-pid 1` option `perfcollect` only captures performance data for
   the `dotnet` process. Remove it to collect performance data for the whole
   system.

   Press `Ctrl + C` to stop collecting.

1. After collection is stopped, view the report using the following command

```
/tools/perfcollect view sample.trace.zip
```

1. Verify that the trace includes the map files by listing contents in the zip file

```
unzip -l sample.trace.zip
```

   You should see `perf-1.map` and `perfinfo-1.map` in the zip, along with other `*.maps` files.

   If anything went wrong during the collection, check out `perfcollect.log` file inside the zip for more details.


```
unzip sample.trace.zip sample.trace/perfcollect.log
tail -100 sample.trace/perfcollect.log
```

   Messages like below near the end of the log file indicates that you hit
   [a known issue](https://github.com/dotnet/corefx-tools/issues/84),
   please check out the Potential Issues section for a workaround

```
Running /usr/bin/perf_4.9 script -i perf.data.merged -F comm,pid,tid,cpu,time,period,event,ip,sym,dso,trace > perf.data.txt
'trace' not valid for hardware events. Ignoring.
'trace' not valid for software events. Ignoring.
'trace' not valid for unknown events. Ignoring.
'trace' not valid for unknown events. Ignoring.
Samples for 'cpu-clock' event do not have CPU attribute set. Cannot print 'cpu' field.

Running /usr/bin/perf_4.9 script -i perf.data.merged -f comm,pid,tid,cpu,time,event,ip,sym,dso,trace > perf.data.txt
  Error: Couldn't find script `comm,pid,tid,cpu,time,event,ip,sym,dso,trace'

 See perf script -l for available scripts.
```

1. On the host, retrieve the trace from the sidecar container

```
docker cp sidecar:/tools/sample.trace.zip ./
```

1. Transfer the trace from the host machine to a Windows machine for further
   investigation using [PerfView](https://github.com/Microsoft/perfview).

   PerfView supports analyzing `perfcollect` traces from Linux. Open
   `sample.trace.zip` then follow the usual workflow of working with PerfView.

   **screenshot of PerfView opening trace from Linux to be inserted**

   For more information on analyzing cpu traces from Linux using PerfView, see
   [this blog post](https://blogs.msdn.microsoft.com/vancem/2016/02/20/analyzing-cpu-traces-from-linux-with-perfview/)
   and [Channel 9 series](https://channel9.msdn.com/Series/PerfView-Tutorial)
   by Vance Morrison.

## Potential Issues

a. In some configurations the collected `cpu-clock` events do not have the `cpu`
   field. This causes a failure in step 12) when `perfcollect` tries to merge
   trace data. Here’s a workaround

   Open `perfcollect` in an editor, find the line that contains "`-F`" (capital F),
   then remove "`cpu`" from the `$perfcmd` line so it becomes

```
LogAppend "Running $perfcmd script -i $mergedFile -F comm,pid,tid,cpu,time,period,event,ip,sym,dso,trace > $outputDumpFile"
$perfcmd script -i $mergedFile -F comm,pid,tid,time,period,event,ip,sym,dso,trace > $outputDumpFile 2>>$logFile
LogAppend
```

   After applying the workaround and collecting the traces, please be aware of
   [a known PerfView issue](https://github.com/Microsoft/perfview/issues/806)
   when viewing the traces whose cpu field is missing.
   This issue has been fixed already and will be available in the future releases of PerfView.

## Conclusion

This document describes a sidecar approach to collect CPU performance trace for
.NET Core application running inside of a container. The step-by-step guide here
describes a manual/on-demand investigation. However, most of steps above may
be automated by container orchestrator or infrastructure.

## References and Useful Links

1. Linux Container Performance Analysis, talk by Brendan Gregg, inventor of FlameGraph https://www.usenix.org/conference/lisa17/conference-program/presentation/gregg
2. https://github.com/goldshtn/linux-tracing-workshop
3. Debugging and Profiling .NET Core Apps on Linux, slides from Sasha Goldshtein https://assets.ctfassets.net/9n3x4rtjlya6/1qV39g0tAEC2OSgok0QsQ6/fbfface3edac8da65fd380cc05a1a028/Sasha-Goldshtein_Debugging-and-profiling-NET-Core-apps-on-Linux.pdf
4. Debugging Python Containers in Production http://blog.0x74696d.com/posts/debugging-python-containers-in-production/
5. perfcollect source code https://github.com/dotnet/corefx-tools/blob/master/src/performance/perfcollect/perfcollect
6. Documentation on Performance Tracing on Linux for .NET Core https://github.com/dotnet/coreclr/blob/master/Documentation/project-docs/linux-performance-tracing.md
7. PerfView tutorials on Channel9 https://channel9.msdn.com/Series/PerfView-Tutorial
