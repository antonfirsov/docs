January 2017 Update for ASP.NET Core 1.1
========================================

We just released an update for ASP.NET Core 1.1 due to [Microsoft Security Advisory 4010983](https://technet.microsoft.com/en-us/library/security/4010983). The advisory is for a vulnerability in ASP.NET Core MVC 1.1.0 that could allow denial of service. All of the information you need is in the advisory. A short summary is provided below.

Red Hat customers should consult the [Red Hat advisory](https://access.redhat.com/solutions/2890741) for the same issue.

## How to Obtain the Updates

The update is in the [Microsoft.AspNetCore.Mvc.Core package](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Core). You need to upgrade your project to use version 1.1.1 (or later) of the package and then re-publish your application.

See below for examples of project file updates, for project.json and csproj formats.

### Project.json

The dependencies section of an updated project.json file would look like the following (in its most minimal form):

``` json
"dependencies": {
"Microsoft.NETCore.App": {
    "version": "1.1.0",
    "type": "platform"
},
"Microsoft.AspNetCore": "1.1.0",
"Microsoft.AspNetCore.Mvc.Core": "1.1.1",
}
```

### CSProj

An updated csproj file would look like the following (in its most minimal form):

``` xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>netcoreapp1.1</TargetFramework>
  </PropertyGroup>
  <PropertyGroup>
    <PackageTargetFallback>$(PackageTargetFallback);portable-net45+win8+wp8+wpa81;</PackageTargetFallback>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore" Version="1.1.0" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Core" Version="1.1.1" />
  </ItemGroup>
</Project> 
```

## Learn more

You can ask questions on the aspnet/mvc repo, where a [discussion issue](https://github.com/aspnet/Mvc/issues/5726) has been created.
