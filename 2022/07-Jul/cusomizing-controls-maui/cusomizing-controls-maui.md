---
post_title: Customizing Controls in .NET MAUI
author1: pedro-jesus
post_slug: customizing-controls-maui
username: pedro-jesus
microsoft_alias: bramin
featured_image: custom-controls.png
categories: .NET MAUI
summary: Let's look at how to customize .NET MAUI controls.
desired_publication_date: 2022-07-09
---

> **Note:** This is a Guest Blog Post by Microsoft MVP, [Pedro Jesus](https://twitter.com/pj_souz). Pedro works as a Software Engineer at [ArcTouch](https://www.linkedin.com/company/arctouch/) and is a core maintainer of the [.NET MAUI Community Toolkit](https://github.com/CommunityToolkit/Maui)

Before looking at .NET MAUI let's move back a couple years, back to the [Xamarin.Forms](https://docs.microsoft.com/xamarin/xamarin-forms/?WT.mc_id=dotnet-0000-bramin) era. Back then, we had a couple of ways to customize controls: We had [`Behaviors`](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/behaviors/?WT.mc_id=dotnet-0000-bramin) that are used when you don't need to access the platform-specific APIs in order to customize controls; and we had [`Effects`](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/effects/introduction?WT.mc_id=dotnet-0000-bramin) if you need to access the platform-specific APIs.

Let's focus a little bit on the [`Effects`](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/effects/introduction?WT.mc_id=dotnet-0000-bramin) API. It was created due to Xamarin's lack of multi-target architecture. That means we can't access platform-specific code at the shared level (in the .NET Standard `csproj`). It worked pretty well and can save you from creating [Custom Renderers](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/custom-renderer/?WT.mc_id=dotnet-0000-bramin).

Today, in .NET MAUI, we can leverage the power of the multi-target architecture and access the platform-specific APIs in our shared project. So do we still need `Effects`? No, because we have access to all code and APIs from all platforms that we target.

So let's talk about all the possibilities to customize a control in .NET MAUI and some dragons that you may found in the way. For this, we'll be customizing the `Image` control adding the ability to tint the image presented.

> **Note:** .NET MAUI still supports `Effects` if you want to use it, however it is not recommended

## Customizing an Existing Control

To add additional features to an existing control, we extend it and add the features that we need.

Let's create a new control, `class ImageTintColor : Image` and add a new `BindableProperty` that we will leverage to change the tint color of the `Image`.

```csharp
public class ImageTintColor : Image
{
    public static readonly BindableProperty TintColorProperty =
        BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintColorBehavior), propertyChanged: OnTintColorChanged);

    public Color? TintColor
    {
        get => (Color?)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // ...
    }
}
```

Folks familiar with Xamarin.Forms will recognize this; it's pretty much the same code that you will write in a Xamarin.Forms application.

The .NET MAUI platform-specific API work will happen on the `OnTintColorChanged` delegate. Let's take a look at it.

```csharp
public class ImageTintColor : Image
{
    public static readonly BindableProperty TintColorProperty =
        BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintColorBehavior), propertyChanged: OnTintColorChanged);

    public Color? TintColor
    {
        get => (Color?)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }

    static void OnTintColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ImageTintColor)bindable;
        var tintColor = control.TintColor;

        if (control.Handler is null || control.Handler.PlatformView is null)
        {
            // Workaround for when this executes the Handler and PlatformView is null
            control.HandlerChanged += OnHandlerChanged;
            return;
        }

        if (tintColor is not null)
        {
#if ANDROID
            // Note the use of Android.Widget.ImageView which is an Android-specific API
            // You can find the Android implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L9-L12
            ImageExtensions.ApplyColor((Android.Widget.ImageView)control.Handler.PlatformView, tintColor);
#elif IOS
            // Note the use of UIKit.UIImage which is an iOS-specific API
            // You can find the iOS implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L7-L11
            ImageExtensions.ApplyColor((UIKit.UIImageView)control.Handler.PlatformView, tintColor);
#endif
        }
        else
        {
#if ANDROID
            // Note the use of Android.Widget.ImageView which is an Android-specific API
            // You can find the Android implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L14-L17
            ImageExtensions.ClearColor((Android.Widget.ImageView)control.Handler.PlatformView);
#elif IOS
            // Note the use of UIKit.UIImage which is an iOS-specific API
            // You can find the iOS implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L13-L16
            ImageExtensions.ClearColor((UIKit.UIImageView)control.Handler.PlatformView);
#endif
        }

        void OnHandlerChanged(object s, EventArgs e)
        {
            OnTintColorChanged(control, oldValue, newValue);
            control.HandlerChanged -= OnHandlerChanged;
        }
    }
}
```

Because .NET MAUI uses multi-targeting, we can access the platform specifics and customize the control the way that we want. The `ImageExtensions.ApplyColor` and `ImageExtensions.ClearColor` methods are helper methods that will add or remove the tint from the image.

One thing that you maybe noticed is the `null` check for `Handler` and `PlatformView`. This is the first dragon that you may find on your way. When the `Image` control is created and instantiated and the `PropertyChanged` delegate of the `BindableProperty` is called, the `Handler` can be `null`. So, without that null check, the code will throw a `NullReferenceException`. This may sound like a bug, but it's actually a feature! This allows the .NET MAUI engineering team to keep the same lifecycle that controls have on Xamarin.Forms, avoiding some breaking changes for applications that will migrate from Forms to .NET MAUI.

Now that we have everything set up, we can use our control in our `ContentPage`. In the snippet below you can see how to use it in XAML:

```xml
<ContentPage x:Class="MyMauiApp.ImageControl"
             xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp"
             Title="ImageControl"
             BackgroundColor="White">

            <local:ImageTintColor x:Name="ImageTintColorControl"
                                  Source="shield.png"
                                  TintColor="Orange" />
</ContentPage>
```

## Using Attached Property and PropertyMapper

Another way to customize a control is using `AttachedProperties`, it's a flavor of `BindableProperty` when you don't need to have it tied to a specific custom control.

Here's how we can create an AttachedProperty for TintColor:

```csharp
public static class TintColorMapper
{
    public static readonly BindableProperty TintColorProperty = BindableProperty.CreateAttached("TintColor", typeof(Color), typeof(Image), null);

    public static Color GetTintColor(BindableObject view) => (Color)view.GetValue(TintColorProperty);

    public static void SetTintColor(BindableObject view, Color? value) => view.SetValue(TintColorProperty, value);

    public static void ApplyTintColor()
    {
        // ...
    }
}
```

Again we have the boilerplate that we have on Xamarin.Forms for the `AttachedProperty`, but as you can see we don't have the `PropertyChanged` delegate. In order to handle the property change, we will use the `Mapper` in the `ImageHandler`. You add the Mapper at any level, since the members are `static`. I choose to do it inside the `TintColorMapper` class, as you can see below.

```csharp
public static class TintColorMapper
{
     public static readonly BindableProperty TintColorProperty = BindableProperty.CreateAttached("TintColor", typeof(Color), typeof(Image), null);

    public static Color GetTintColor(BindableObject view) => (Color)view.GetValue(TintColorProperty);

    public static void SetTintColor(BindableObject view, Color? value) => view.SetValue(TintColorProperty, value);
    
    public static void ApplyTintColor()
    {
        ImageHandler.Mapper.Add("TintColor", (handler, view) =>
        {
            var tintColor = GetTintColor((Image)handler.VirtualView);

            if (tintColor is not null)
            {
#if ANDROID
                // Note the use of Android.Widget.ImageView which is an Android-specific API
                // You can find the Android implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L9-L12
                ImageExtensions.ApplyColor((Android.Widget.ImageView)control.Handler.PlatformView, tintColor);
#elif IOS
                // Note the use of UIKit.UIImage which is an iOS-specific API
                // You can find the iOS implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L7-L11
                ImageExtensions.ApplyColor((UIKit.UIImageView)handler.PlatformView, tintColor);
#endif
            }
            else
            {
#if ANDROID
                // Note the use of Android.Widget.ImageView which is an Android-specific API
                // You can find the Android implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L14-L17
                ImageExtensions.ClearColor((Android.Widget.ImageView)handler.PlatformView);
#elif IOS
                // Note the use of UIKit.UIImage which is an iOS-specific API
                // You can find the iOS implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L13-L16
                ImageExtensions.ClearColor((UIKit.UIImageView)handler.PlatformView);
#endif
            }
        });
    }
}

```

The code is pretty much the same as showed before, just implemented using another API, in this case the `AppendToMapping` method. If you don't want this behavior, use the `CommandMapper` instead, it will be triggered just when a property changed or an action happens.

Be aware that when we handle with `Mapper` and `CommandMapper`, we're adding this behavior for all controls that use that handler in the project. In this case all `Image` controls will trigger this code. In some cases this isn't what you want, if you something more specific the next way, using `PlatformBehavior` will fit perfectly.

So, now that we have everything set up, we can use our control in our page, at the snippet below you can see how to use it in XAML.

```xml
<ContentPage x:Class="MyMauiApp.ImageControl"
             xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp"
             Title="ImageControl"
             BackgroundColor="White">

            <Image x:Name="Image"
                   local:TintColorMapper.TintColor="Fuchsia"
                   Source="shield.png" />
</ContentPage>
```

## Using PlatformBehavior

`PlatformBehavior` is a new API created on .NET MAUI to make easier the task to customize controls when you need to access the platform-specifics APIs in safe way (safe because it ensures that the `Handler` and `PlatformView` aren't `null`). It has two methods to `override`: `OnAttachedTo` and `OnDetachedFrom`. This API exists to replace the `Effect` API from Xamarin.Forms and to take advantage of the multi-target architecture. 

In this example, we will use `partial class` to implement the platform-specific APIs:

```csharp
//FileName : ImageTintColorBehavior.cs

public partial class ImageTintColorBehavior
{
    public static readonly BindableProperty TintColorProperty =
        BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(TintColorBehavior), propertyChanged: OnTintColorChanged);

    public Color? TintColor
    {
        get => (Color?)GetValue(TintColorProperty);
        set => SetValue(TintColorProperty, value);
    }
}
```

The above code will be compiled by all platforms that we target.

Now let's see the code for the `Android` platform:

```csharp
//FileName: ImageTintColorBehavior.android.cs

public partial class IconTintColorBehavior : PlatformBehavior<Image, ImageView> // Note the use of ImageView which is an Android-specific API
{
    protected override void OnAttachedTo(Image bindable, ImageView platformView) =>
        ImageExtensions.ApplyColor(bindable, platformView); // You can find the Android implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L9-L12

    protected override void OnDetachedFrom(Image bindable, ImageView platformView) =>
        ImageExtensions.ClearColor(platformView); // You can find the Android implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.android.cs#L14-L17
}
```

And here's the code for the `iOS` platform:

```csharp
//FileName: ImageTintColorBehavior.ios.cs

public partial class IconTintColorBehavior : PlatformBehavior<Image, UIImageView> // Note the use of UIImageView which is an iOS-specific API
{
    protected override void OnAttachedTo(Image bindable, UIImageView platformView) => 
        ImageExtensions.ApplyColor(bindable, platformView); // You can find the iOS implementation of `ApplyColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L7-L11

    protected override void OnDetachedFrom(Image bindable, UIImageView platformView) => 
        ImageExtensions.ClearColor(platformView); // You can find the iOS implementation of `ClearColor` here: https://github.com/pictos/MFCC/blob/1ef490e507385e050b0cfb6e4f5d68f0cb0b2f60/MFCC/TintColorExtension.ios.cs#L13-L16
}
```

As you can see, we don't need to care about if the `Handler` is `null`, because that's handled for us by `PlatformBehavior<T, U>`.

We can specify the type of platform-specific API that this Behavior covers. If you want to apply the control for more than one type, you don't need to specify the type of the platform view (e.g. use `PlatformBehavior<T>`); you probably want to apply your `Behavior` in more than one control, in that case the platformView will be an `PlatformBehavior<View>` on `Android` and an `PlatformBehavior<UIView>` on `iOS`.

And the usage is even better, you just need to call the `Behavior`:

```XML
<ContentPage x:Class="MyMauiApp.ImageControl"
             xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MyMauiApp"
             Title="ImageControl"
             BackgroundColor="White">

            <Image x:Name="Image"
                   Source="shield.png">
                <Image.Behaviors>
                    <local:IconTintColorBehavior TintColor="Fuchsia">
                </Image.Behaviors>
            </Image>
</ContentPage>
```

> **Note:** The `PlatformBehavior` will call the `OnDetachedFrom` when the `Handler` disconnect from the `VirtualView`, in other words, when the `Unloaded` event is fired. The `Behavior` API doesn't call the `OnDetachedFrom` method automatically, you as a developer needs to handle it by yourself.

## Conclusion

In this blog post we discussed various ways to customize your controls and interact with the platform-specific APIs. There's no `right` or `wrong` way, all those are valids solutions, you just need to see which will suit better to your case. I would say that for most cases you want to use the `PlatformBehavior` since it's designed to work with the multi-target approach and makes sure to clean-up the resources when the control is not used anymore.
