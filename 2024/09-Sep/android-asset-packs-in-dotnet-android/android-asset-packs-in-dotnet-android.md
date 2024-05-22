---
post_title: 'Android Asset Packs for .NET & .NET MAUI Android Apps'
author1: deellis@microsoft.com
post_slug: android-asset-packs-in-dotnet-android
microsoft_alias: deellis
featured_image: android-assets-featured.jpg
categories: .NET, .NET MAUI, C#
tags: asset pack, .net maui, .net 9, android
ai_note: show
summary: Introducing a new way to build and deploy Android Asset Packs using .NET for Android and .NET MAUI. If your app has allot of AndroidAsset items this feature will make your life easier.
post_date: 2024-09-12 10:05:00
---

We have introducing a new way to generate asset packs for your .NET & .NET MAUI Android applications in [.NET 9](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-9) that you can try out today. What are Asset Packs? Why should you use them? How to get started? Let's get into it!

## What is an Asset Pack?

Back in 2018 Google introduced a new package format for deploying Android applications to the Google Play Store. Support for this new format, Android App Bundles (AAB), has been in .NET Android since .NET 6. There are many feature of Android App Bundles and we prioritized the top features that developers needed. One advanced features that is part of the AAB format is Asset Packs which will now be part of .NET 9. So what are asset packs?

Part of the new package format was the ability to place `Assets` into a separate package.
This would allow developers to upload games and apps which would normally be larger that the basic package size allowed by Google Play. By putting these assets into a separate package you get the ability to upload a package which is up to 2 gigabytes in size. The basic package size is 200 megabytes. There is a condition, asset packs can ONLY contain `Assets`. In the case of .NET Android this means items which have the `AndroidAsset` build action.

Asset Packs can can have different delivery options. This controls when your assets will install on the device. Install Time packs are installed at the same time as the application. This type pack can be up to 1 Gigabyte in size, but you can only have one of them. The Fast Follow type of pack will install at some point shortly AFTER the app has finished installing. The app will be able to start while this type of pack is being installed so developers should check it has finished installing before trying to use the assets. This kind of asset pack can be up to 512 Megabytes in size. The final type is the On Demand type. These will never be downloaded to the device unless the application specifically requests it. As mentioned the total size of ALL your asset packs cannot exceed 2 Gigabytes, and you can have up to 50 separate asset packs.

If you have an application with allot of Assets such as a game, you can see how these types of pack would be very useful. Assets like music, movies or textures can take up allot of space inside your application package. Having the ability to split them out gives you more freedom to put code based features in your actual game or app.

For more information you can read up on Asset Delivery at [https://developer.android.com/guide/playcore/asset-delivery](https://developer.android.com/guide/playcore/asset-delivery)

## The Problem: Building Asset Packs

With the .NET 8 version of .NET Android building an asset pack was not supported by the .NET build system. There is an MSBuild ItemGroup 'AndroidAppBundleModules' which can be used to add additional `zip` files to the `aab` package. However users would still need to figure out a way to build the asset pack manually using `gradle` or `aapt2`. Some community based hacks are available but it would be nice to be able to use this feature directly in .NET Android. Most of these hacks involve setting up a separate project and adding custom build targets to your solution in order to build the appropriate zip files. This makes it a bit awkward to work with.

## The Solution: AssetPack Metadata

We thought long and hard on the best way to implement asset pack support. Ideally we wanted something which would allow not only .NET Android developers to easily create asset packs, but also allow .NET Maui developers to make use of it as well.

We decided on was to make use of MSBuild Metadata to control the creation of the asset packs. There will be no need to create any extra projects or mess about with custom target. Adding some simple Metadata to `AndroidAsset` Items will allow users to define asset packs.

Given the following files in an example project

```bash
MyProject.csproj
MainActivity.cs
Assets/
    MyLargeAsset.mp4
    MySmallAsset.json
AndroidManifest.xml
```

The `MyLargeAsset.mp4` and `MySmallAsset.json` will both end up in the main `.aab` file when we publish the application. Lets say that `MyLargeAsset.mp4` is over the 200 Megabyte limit, so having it in the main app bundle is not an option. So what we want to do is place this asset in an `Install Time` asset pack. This way we can make sure it is in place as soon as the app is installed on a device.

With the new system we can make use of two new Metadata attributes we are supporting for the `AndroidAsset` ItemGroup.

The first piece of Metadata is `AssetPack`. If present this attribute will control which asset pack the asset will end up in. If not present the asset will end up in the main app bundle by default.
The AssetPack name will be in the form of `$(AndroidPackage).%(AssetPack)`, so if the package name of your project is `com.foo.myproject` and the `AssetPack` value was `bar`, the asset pack would be `com.foo.myproject.bar`.
There is a special value for `AssetPack` which is `base`. This can be used to make sure certain assets end up in the main app bundle rather than an asset pack. More on this later.
The other is `DeliveryType`. If present you can use this to control what type of asset pack is created. Valid values for this are `InstallTime`, `FastFollow` and `OnDemand`. If not present the default value is `InstallTime`.

So in our example if we wanted to move the `MyLargeAsset.mp4` asset into its own asset pack we can use the following in the `MyProject.csproj`. Note we are using `Update` rather than include since .NET Android supports auto import of assets.

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/MyLargeAsset.mp4" AssetPack="myassets" />
</ItemGroup>
```

This will cause the .NET Android build system to create a new asset pack called `com.foo.myproject.myassets` and include the `MyLargeAsset.mp4` in that pack. This pack will be automatically included in the final `.aab` file. There is no need to manually create this pack at all. Because the default value of `DeliveryType` is `InstallTime` by default, there is no additional work needed in this case.

## A More Complete Example

Now there are probably use cases where you need to control which assets go into a pack and which ones do not. But you also have hundreds of assets. In this case you will probably want to use wildcards to update the auto imported items.

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/*" AssetPack="myassets" DeliveryType="FastFollow" />
</ItemGroup>
```

The above snippet will make ALL the assets picked up from the Assets folder go into the `myassets` asset pack which is `FastFollow`. However if you have one or two items that you need in the main package, because the asset pack might not be installed when they are needed, we need a way to do that.

This is where the previously mentioned `base` value for `AssetPack` comes in.

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/*" AssetPack="myassets" DeliveryType="FastFollow" />
    <AndroidAsset Update="Assets/myimportantfile.json" AssetPack="base" />
</ItemGroup>
```

In the updated example above, the `myimportantfile.json` will now end up in the main app bundle, rather than the `myassets` asset pack. This is a good way to control which specific assets end up in certain locations.

### Checking the Status of FastFollow Asset Packs

If you are using `FastFollow` based packs, you will need to check it is installed before trying to access its contents.
First add a PackageReference to the `Xamarin.Google.Android.Play.Asset.Delivery` NuGet package.

```xml
<ItemGroup>
<PackageReference Include="Xamarin.Google.Android.Play.Asset.Delivery" Version="2.0.5.0" />
</ItemGroup>
```

This will pull in the required API's to work with Asset Packs.

Next we need to make use of the `Google.Play.Core.Assets.AssetPackManager` to query the location of the `FastFollow` asset pack. We can use the `GetPackLocation` method to do this.
If it returns `null` for the `AssetsPath` method on the returned `AssetPackLocation`, then the pack has not yet been installed. If it returns anything else that is the install location of the pack.

```csharp
var assetPackManager = AssetPackManagerFactory.GetInstance (this);
AssetPackLocation assetPackPath = assetPackManager.GetPackLocation("myfastfollowpack");
string assetsFolderPath = assetPackPath?.AssetsPath() ?? null;
if (assetsFolderPath is null) {
    // FastFollow Pack is not installed.
}
```

### Downloading OnDemand Asset Packs

If you want to use `OnDemand` packs you will need to download these manually. In order to do that you must use the `Google.Play.Core.Assets.IAssetPackManager`. In order to monitor the progress of the download you need to make use of the `AssetPackStateUpdateListener` type. However because this type is a Java Generic you cannot use it directly. To work around this the .NET Binding provides a `AssetPackStateUpdateListenerWrapper` class which can be used to hook up a event to monitor the progress.

First we need to declare the fields we need.

```csharp
// This is the underlying IAssetPackManager
IAssetPackManager assetPackManager;
// This is the wrapper AssetPackStateUpdateListener which allows us
// to provide an event to get updates from the IAssetPackManager.
AssetPackStateUpdateListenerWrapper listener;
```

Next up we declare the delegate we want to use to monitor the download. Note the `AssetPackStateEventArgs` is a nested class of `AssetPackStateUpdateListenerWrapper`. You can find out what `AssetPackStatus` values to you can use over at the [android documentation](https://developer.android.com/reference/com/google/android/play/core/assetpacks/model/AssetPackStatus).

```csharp
void Listener_StateUpdate(object sender, AssetPackStateUpdateListenerWrapper.AssetPackStateEventArgs e)
{
    var status = e.State.Status();
    switch (status)
    {
        case AssetPackStatus.Downloading:
            long downloaded = e.State.BytesDownloaded();
            long totalSize = e.State.TotalBytesToDownload ();
            double percent = 100.0 * downloaded / totalSize;
            Android.Util.Log.Info ("Listener_StateUpdate", $"Downloading {percent}");
            break;
        case AssetPackStatus.Completed:
            break;
        case AssetPackStatus.WaitingForWifi:
            assetPackManager.ShowCellularDataConfirmation (this);
            break;
    }
}
```

Next thing to do is to create an instance of the `IAssetPackManager` via the `AssetPackManagerFactory.GetInstance` method. We then need to create the `AssetPackStateUpdateListenerWrapper` instance and hookup the
`StateUpdate` delegate.

```csharp
assetPackManager = AssetPackManagerFactory.GetInstance (this);
// Create our Wrapper and set up the event handler.
listener = new AssetPackStateUpdateListenerWrapper();
listener.StateUpdate += Listener_StateUpdate;
```

We need to make sure to `RegisterListener` and `UnregisterListener` the listener with the `assetPackManager` when the application resumes or pauses.

```csharp
protected override void OnResume()
{
    // register our Listener Wrapper with the SplitInstallManager so we get feedback.
    assetPackManager.RegisterListener(listener.Listener);
    base.OnResume();
}

protected override void OnPause()
{
    assetPackManager.UnregisterListener(listener.Listener);
    base.OnPause();
}
```

Before we download the `OnDemand` asset pack we need to check to see if it has been installed first. We do this using the `assetPackManager.GetPackLocation` method like we did for `FastFollow` packs. In this case if we get a `null` for `AssetsPath()` the pack is not installed. We can then call `assetPackManager.Fetch` to start the download. You can also use the C# extension method `AsAsync<T>` which returns a `Task<T>` so it can be `awaited` on. This extension method is available in the `Android.Gms.Extensions` namespace.

```csharp
// Try to install the new feature.
var assetPackPath = assetPackManager.GetPackLocation ("myondemandpack");
string assetsFolderPath = assetPackPath?.AssetsPath() ?? null;
if (assetsFolderPath is null) {
    await assetPackManager.Fetch(new string[] { "myondemandpack" }).AsAsync<AssetPackStates>();
}
```

From this point on we can monitor the status via the `AssetPackStateUpdateListenerWrapper` we setup earlier.

## Debugging and Testing

To test your asset packs locally you need to make sure you are using the `aab` `AndroidPackageFormat`. By default .NET Android will use `apk` for debugging. If the `AndroidPackageFormat` is left as `apk` all the assets will be packaged into the `apk` as they normally would be.

The following settings in your csproj will turn on the required settings for debugging your asset packs.

```xml
<PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
<AndroidPackageFormat>aab</AndroidPackageFormat>
<EmbedAssembliesIntoApk>true</EmbedAssembliesIntoApk>
<AndroidBundleToolExtraArgs>--local-testing</AndroidBundleToolExtraArgs>
</PropertyGroup>
```

The `--local-testing` flag is required for testing on your local device. It tells the `bundletool` app that ALL the asset packs should be installed in a cached location on the device. It also sets up the `IAssetPackManager` to use a mock downloader which will use the cache. This allows you to test installing `OnDemand` and `FastFollow` packs in a `Debug` environment.

## Can I use this in my Maui Application?

Yes absolutely! When building your Maui application for Android you are using the .NET Android Sdk. So all the features available for .NET Android users can be used by Maui developers. Maui does have its own way to define what an `Asset` is, this is via the `MauiAsset` build action. Adding the additional `AssetPack` and `DeliveryType` meta data to the `MauiAsset` Items will produce the same results. The additional metadata will be ignored by other platforms.

```xml
<MauiAsset
    Include="Resources\Raw\**"
    LogicalName="%(RecursiveDir)%(Filename)%(Extension)"
    AssetPack="myassetpack"
/>
```

If you have specific items you want to place in an Asset Pack you can use the `Update` method to define the `AssetPack` metadata.

```xml
<MauiAsset Update="Resources\Raw\MyLargeAsset.txt" AssetPack="myassetpack" />
```

## Conclusion

Asset Packs provide a great way to increase the size of your application without compromising on the amount of media or data you ship with it. Moving some or all of your Assets into an Asset Pack will allow you to put more of the base package size towards code and user interface. Be sure to read more about what is new for [.NET MAUI in .NET 9](https://learn.microsoft.com/dotnet/maui/whats-new/dotnet-9).
