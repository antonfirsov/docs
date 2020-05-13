# Introducing .NET Multi-platform App UI (.NET MAUI)

You can build anything with .NET. We say this over and over again, and it's one of the main reasons millions of developers choose .NET as the platform for their careers, and companies invest for their businesses. In today's business landscape, your teams and customers use many devices across the most popular platforms, from Android and iOS to Windows and macOS. We feel that building applications that span all of those needs is where .NET really shows its power, so today we are excited to announce your new first-class UI framework for doing just that: .NET Multi-platform App UI, affectionately call MAUI. 

Maui is an evolution of the increasingly popular Xamarin.Forms toolkit that turns 6 years old this month. Enterprises such as UPS, Pepsi, Ernst & Young, and Delta have been leveraging the mobile expertise of Xamarin atop .NET to power their businesses; some since the very beginning. It has also been very successful in helping small businesses maximize their development investment sharing upwards of 95% of their code, and beating their competitors to market. Today we are announcing our plans to extend this success on mobile to embrace the desktop and be the best way to build applications for first party devices such as the new Surface Duo.

## More Platforms, All Native

.NET MAUI simplifies the choices for .NET developers, providing a single stack that supports the best of breed solutions for all modern workloads: Android, iOS, macOS, and Windows. All the features of each platform and UI control are made available in a simple, cross-platform API for you to deliver no-compromise user experiences while sharing even more code than before.

## One Library, One Toolchain

At Build 2019 we said we would unify .NET Core and the Mono/Xamarin implementations into one base class library (BCL) and toolchain (SDK). In the wake of the global health pandemic, we are adapting to the changing needs of our customers in order to provide the support needed to assist with smooth operations. Our efforts continue to be anchored in helping our customers address their most urgent needs. As a result, we expect these features to be available in preview for the .NET 5 release, and the unification to be truly completed with .NET 6, our Long-Term Support (LTS) release.  Our vision has not changed, but our timeline has.

## Single Project Developer Experience

We will be delivering a vastly simplified, yet equally as powerful project experience for Maui. From the very beginning, a Maui solution begins with a single project that focuses on your goals, whether you are targeting one platform or all platforms. Take a look at this prototype that shows the beauty of this approach.

// video

Notice that you only need to worry about platforms when you're ready to run or publish the application. Any platform specific requirements can be contained in a simple, common structure. Assets such as images, fonts, and other resources are handled by .NET Maui for each platform from a single location. It couldn't be easier!

You master one way to build client apps, the Maui way, and all platforms are within your reach.

## Modern App Patterns

### MVVM

MVVM and XAML, the predominant pattern and practice among .NET developers for decades now, are first-class features in Maui. 

### MVU

In addition, we are enabling developers to write fluent C# UI and implement the increasingly popular Model-View-Update pattern. Both styles deliver the same native applications, performance, and platform fidelity. Developers can now choose which style best suits their preference and use case. Let's take a look at MVU in action.

// video and code sample

### Blazor

In addition to MVU, .NET MAUI is also optimized for consumption by Blazor in order to deliver a variety of future possibilities from Blazor WebWindow to Blazor Native, and even a Blazor hosted control within a .NET MAUI XAML application! In a unified .NET universe, the possibilities are intoxicating.

// Blazor demo

## Transitioning from Xamarin.Forms to .NET MAUI

Xamarin.Forms developers will hit the ground running with new projects in .NET MAUI, using all the same controls and APIs they have grown to know and love. In order to help developers make a smooth transition of existing apps to .NET MAUI we will be providing Try-Convert support and migration guides.

### The .NET MAUI Timeline

We will begin shipping .NET MAUI previews on .NET 5 later this year, and target a GA release with .NET 6 in November of 2021. 

### What's Next for Xamarin and Xamarin.Forms

As part of our .NET unification, Xamarin.iOS and Xamarin.Android will become part of .NET 6 as .NET for iOS and .NET for Android. Because these bindings are projections of the SDKs shipped from Apple and Google, nothing changes there, however build tooling, target framework monikers, and runtime framework monikers will be updated to match all other .NET 6 workloads. Our commitment to keeping .NET developers up-to-date with the latest mobile SDKs is foundational to .NET MAUI and remains firm. When .NET 6 ships, we expect to ship a final release of Xamarin SDKs in their current form that will be serviced for a year. All modern work will at that time shift to .NET 6.

Xamarin.Forms will ship a new major version later this year, and continue to ship minor and service releases every 6 weeks through .NET 6 GA in November 2021. The final release of Xamarin.Forms will be serviced for a year after shipping, and all modern work will shift to .NET MAUI. 

## Get Involved Today

CTA to star the new repository and engage on proposals