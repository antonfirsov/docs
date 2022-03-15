---
post_title: Announcing .NET MAUI Preview 14
username: david.ortinau@microsoft.com
microsoft_alias: daortin
categories: .NET MAUI, .NET
desired_publication_date: 2022-03-15
featured_image: images/menubar.png
summary: .NET MAUI Preview 14 has shipped with...
---

Preview 14 of .NET Multi-platform App UI is now available in Visual Studio 2022 17.2 Preview 2. This release includes a hefty volume of issue resolutions and completed features, and one new feature that will be a welcome addition for desktop developers: the MenuBar. While desktop app navigation and menus are often designed into the content window of many modern applications (think Teams left sidebar or Maps top tabs), there's still a strong need for a traditional menu that resides at the top of the app window on Windows, and in the title bar on macOS.  

Menus may be expressed in XAML or in C# for any `ContentPage`. Begin by adding a `MenuBarItem` to the page's `MenuBarItems` collection, and add `MenuFlyoutItem`s for direct children, or `MenuFlyoutSubItem`s for containers of other `MenuFlyoutItem`s.

```xaml
<ContentPage.MenuBarItems>
    <MenuBarItem Text="File">
        <MenuFlyoutItem Text="Quit" Command="{Binding QuitCommand}"/>
    </MenuBarItem>
    <MenuBarItem Text="Locations">
        <MenuFlyoutSubItem Text="Change Location">
            <MenuFlyoutItem Text="Boston, MA"/>
            <MenuFlyoutItem Text="Redmond, WA"/>
            <MenuFlyoutItem Text="St. Louis, MO"/>
        </MenuFlyoutSubItem>
        <MenuFlyoutItem Text="Add a Location" Command="{Binding AddLocationCommand}"/>
    </MenuBarItem>
    <MenuBarItem Text="View">
        <MenuFlyoutItem Text="Refresh" Command="{Binding RefreshCommand}"/>
        <MenuFlyoutItem Text="Toggle Light/Dark Mode" Command="{Binding ToggleModeCommand}"/>
    </MenuBarItem>
</ContentPage.MenuBarItems>
```

Additional Preview 14 highlights include:  

* Device and Essentials reconciliation, plus interfaces for Essentials APIs
* Shell WinUI https://github.com/dotnet/maui/pull/4501
* Removed Microsoft.Extensions.Hosting https://github.com/dotnet/maui/pull/4505
* CarouselView 
* Image caching https://github.com/dotnet/maui/pull/4515
* Native -> Platform https://github.com/dotnet/maui/pull/4599
* Shapes https://github.com/dotnet/maui/pull/4472
* Use string for StrokeShape https://github.com/dotnet/maui/pull/3256
* WebView cookies https://github.com/dotnet/maui/pull/4419
* MenuBar https://github.com/dotnet/maui/pull/4839
* RTL Windows https://github.com/dotnet/maui/pull/4936

Find more details in our [release notes](https://github.com/dotnet/maui/releases/tag/6.0.200-preview.14).

While combing through your feedback in previous .NET MAUI releases we have noticed a theme of questions such as "how do I add a FilePicker", "how do I get check the connectivity of my app", and other such "essential" application tasks that aren't specifically UI.   

## Beyond UI: Focus on Essentials

Within .NET MAUI is a set of APIs located in the Microsoft.Maui.Essentials namespace that unlock common features to bring that same efficiency to non-UI demands as to creating beautiful UI quickly. Originally a library in the Xamarin ecosystem, Essentials is now baked into .NET MAUI and is hosted in the very same [dotnet/maui](https://github.com/dotnet/maui) repository (in case you're wondering where to log your valuable feedback). With it you can access such features as:

|  |  |  |
| :-- | :-- | :-- |
| Accelerometer | App Actions | App Information |
| App Theme | Barometer | Battery |
| Clipboard | Color Converters | Compass |
| Connectivity | Contacts | Detect Shake |
| Display Info | Device Info | Email |
| File Picker | File System Helpers | Flashlight |
| Geocoding | Geolocation | Gyroscope |
| Haptic Feedback | Launcher | Magnetometer |
| MainThread | Maps | Media Picker |
| Open Browser | Orientation Sensor | Permissions |
| Phone Dialer | Platform Extensions | Preferences |
| Screenshot | Secure Storage | Share |
| SMS | Text-to-Speech | Unit Converters |
| Version Tracking | Vibrate | Web Authenticator |

That's a lot! Each API uses common pattern, so let's focus on a few by way of introduction.

## File Picker

Desktop platforms may often have a UI control named FilePicker or similar, but not all platforms do. Mobile platforms do not, but it's still possible to perform the action from any UI element that takes an action such as a simple `Button`. 

```xaml
<Button Text="Select a File" Clicked="OnClicked" />
```

Now we can use the Maui.Essentials API to start the file picking process and handle the callback.

```csharp
async void OnClicked(object sender, EventArgs args)
{
    var result = await PickAndShow(PickOptions.Default);
}

async Task<FileResult> PickAndShow(PickOptions options)
{
    try
    {
        var result = await FilePicker.PickAsync(options);
        if (result != null)
        {
            Text = $"File Name: {result.FileName}";
            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
            {
                var stream = await result.OpenReadAsync();
                Image = ImageSource.FromStream(() => stream);
            }
        }
        
        return result;
    }
    catch (Exception ex)
    {
        // The user canceled or something went wrong
    }
    
    return null;
}
```

The [`PickOptions`](https://docs.microsoft.com/dotnet/api/xamarin.essentials.pickoptions?view=xamarin-essentials) conveniently provides options for configuring your file selection criteria such as file types with [`FilePickerFileType`](https://docs.microsoft.com/dotnet/api/xamarin.essentials.filepickerfiletype?view=xamarin-essentials):

* FilePickerFileType.Images
* FilePickerFileType.Jpeg
* FilePickerFileType.Pdf
* FilePickerFileType.Png
* FilePickerFileType.Videos

## Connectivity

This is an important feature for mobile, but equally useful for desktop in order to handle both offline and online scenarios. In fact, if you have ever attempted to publish an app to the Apple App Store, you may have encountered this common rejection for not detection connectivity status prior to attempting a network call. 

```csharp
var current = Connectivity.NetworkAccess;

if (current == NetworkAccess.Internet)
{
    // able to connect, do API call
}else{
    // unable to connect, alert user
}
```

Some services require a bit of configuration per platform. In this case iOS, macOS, and Windows don't require anything, but Android needs a simple permission added to the "AndroidManifest.xml" which you can find in the Platforms/Android path of your .NET MAUI solution. 

```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

[Read the docs](https://docs.microsoft.com/xamarin/essentials/connectivity) for additional information.


## Get Started Today

.NET MAUI Preview 14 is bundled with Visual Studio 17.2 Preview 2 which is also available today with the [latest productivity improvements for .NET MAUI development](link to vs release notes or blog). If you are using Visual Studio 2022 17.1 Preview 2 or newer, you can upgrade to 17.2 Preview 2.

> If you are upgrading from .NET MAUI preview 10 or earlier, or have been using `maui-check` we recommend starting from a clean slate by [uninstalling all .NET 6](https://docs.microsoft.com/dotnet/core/install/remove-runtime-sdk-versions?pivots=os-windows#uninstall-net) previews and [Visual Studio 2022 previews](https://docs.microsoft.com/visualstudio/install/uninstall-visual-studio?view=vs-2022).

Starting from scratch? Install this [Visual Studio 2022 Preview](https://aka.ms/vs2022preview) (17.2 Preview 2) and confirm .NET MAUI (preview) is checked under the "Mobile Development with .NET workload".

Ready? Open Visual Studio 2022 and create a new project. Search for and select .NET MAUI.

Preview 14 [release notes are on GitHub](https://github.com/dotnet/maui/releases/tag/6.0.200-preview.14). For additional information about getting started with .NET MAUI, refer to our [documentation](https://docs.microsoft.com/dotnet/maui/get-started/installation), and the [migration tip sheet](https://github.com/dotnet/maui/wiki/Migration-to-Preview-14) for a list of changes to adopt when upgrading projects.

For a look at what is coming in future .NET 6 releases, visit our [product roadmap](https://github.com/dotnet/maui/wiki/roadmap), and for a status of feature completeness visit our [status wiki](https://github.com/dotnet/maui/wiki/status).

## Feedback Welcome

We'd love to hear from you! Please let us know about your experiences using .NET MAUI by completing a [short survey](https://www.surveymonkey.com/r/7ZXM69K).