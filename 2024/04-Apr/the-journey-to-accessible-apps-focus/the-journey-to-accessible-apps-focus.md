---
post_title: 'The Journey to Accessible Apps: Keyboard Accessibility and .NET MAUI'
author1: rachelkang
post_slug: the-journey-to-accessible-apps-keyboard-accessible
microsoft_alias: rachelkang
featured_image: keyboardaccessibility.jpg
categories: .NET, .NET MAUI
tags: xamarin, xamarin.forms, accessibility, maui
summary: Are your apps keyboard accessible? Learn more about keyboard traps and find out how you can ensure your .NET MAUI apps are keyboard accessible.
post_date: 2024-04-29 12:00:00
---

Are your apps keyboard accessible? Let's take a look:
1. Launch one of your apps.
2. Attach a physical keyboard if one is not already attached to your device.
3. Navigate your app as you typically would, and use only your keyboard to do so.

How did that go? Was it easy? Did it match your typical experience navigating your app?

Ensuring your app experiences are just as awesome when  navigated exclusively via keyboard is essential to building an app experience that is inclusive and accessible to all.

## Web Content Accessibility Guidelines (WCAG)

To understand what exactly constitutes keyboard accessibility, the Web Content Accessibility Guidelines (WCAG) is a great place to start.

WCAG is a set of technical standards on web accessibility that is widely referenced and extended to various applications and platforms beyond web. It has become a global standard and legal benchmark and continues to evolve with the evolving landscape of technology.

Of the various guidelines is [Guideline 2.1](https://www.w3.org/TR/WCAG22/#keyboard-accessible), a guideline that is often overlooked, which says that developers should "Make all functionality available from a keyboard".

This includes four success criteria:

> **Success Criterion 2.1.1 Keyboard**
> 
> All functionality of the content is operable through a keyboard interface without requiring specific timings for individual keystrokes, except where the underlying function requires input that depends on the path of the user's movement and not just the endpoints.
> 
> **Success Criterion 2.1.2 No Keyboard Trap**
>
> If keyboard focus can be moved to a component of the page using a keyboard interface, then focus can be moved away from that component using only a keyboard interface, and, if it requires more than unmodified arrow or tab keys or other standard exit methods, the user is advised of the method for moving focus away.
> 
> **Success Criterion 2.1.3 Keyboard (No Exception)**
>
> All functionality of the content is operable through a keyboard interface without requiring specific timings for individual keystrokes.
> 
> **Success Criterion 2.1.4 Character Key Shortcuts**
>
> If a keyboard shortcut is implemented in content using only letter (including upper- and lower-case letters), punctuation, number, or symbol characters, then at least one of the following is true:
> - **Turn off** A mechanism is available to turn the shortcut off;
> - **Remap** A mechanism is available to remap the shortcut to include one or more non-printable keyboard keys (e.g., Ctrl, Alt);
> - **Active only on focus** The keyboard shortcut for a user interface component is only active when that component has focus.

A fundamental understanding of these criteria will help you get started in developing apps that are keyboard accessible.

## Keyboard Accessibility and .NET MAUI

On top of various other considerations, .NET MAUI was designed with the intention of enabling easier development of keyboard accessible experiences. Consequently, developers familiar with Xamarin.Forms keyboard behaviors noticed some changes that were made to improve keyboard accessibility in their apps.

For all functionality to be operable through a keyboard interface, it is essential that all interactive controls are keyboard focusable (can receive keyboard focus) and keyboard navigable (can be navigated to and from using the keyboard). This also includes avoiding making invisible content keyboard accessible. Just as we should expect visible controls to be keyboard focusable and navigable, we should expect invisible/nonexistent controls to _not_ be keyboard accessible or present whatsoever.

To avoid keyboard traps, we ensure keyboard navigation is possible into, within, and out of all relevant controls within the current view. For example, if you navigate a screen with multiple CollectionViews, .NET MAUI aligns with standard keyboard accessibility expectations, enabling you to easily navigate into, though, and out of any of the CollectionViews via standard keyboard navigation patterns.

So how exactly does .NET MAUI enable you to create keyboard accessible experiences more easily? Here are just 3 examples:

### 1. Keyboard Navigation on Modal Pages

One area in which .NET MAUI intentionally accounts for keyboard accessibility is with modal pages. When a modal page appears, as with all other pages, it is important to ensure that everything on the page is accessible. With modal pages in particular, however, it is also especially important to ensure that anything on the underlying page is _not_ keyboard accessible and is _not_ surfacing up to the modal page.

When a modal page appears, the first keyboard focusable control on the page should receive focus. Then, all the content on the modal page should be accessible, and all the interactive controls, which should include an exit option (commonly "Save" or "Close") from the modal page, should be keyboard focusable. Once and only once the modal page is exited, the focus should be returned to the underlying page, and the first keyboard focusable control on the underlying page should receive focus once more.

This complexity is handled by the .NET MAUI framework, so your modal pages navigate accessibly right out of the box!

### 2. Keyboard Focus/Unfocus on Android

In developing .NET MAUI, another thing we learned is that it is not possible to "unfocus" an entry on earlier versions of Android. Some control _must_ always be focused. In Xamarin.Forms, "unfocusing" an entry was made "possible" by setting focus on the page layout; unfortunately, this approach created major accessibility issues. For these reasons, .NET MAUI does not allow for this inaccessible behavior by default and highly recommends using a different approach.
 
The motivation behind utilizing "focus" and "unfocus" in the first place is often tied to showing and hiding the soft input keyboard. Instead of manipulating the focus to achieve this, manage keyboard behavior by using the new [`SoftInputExtensions` APIs](https://learn.microsoft.com/dotnet/maui/user-interface/controls/entry?view=net-maui-8.0#hide-and-show-the-soft-input-keyboard)!

For example:

```cs
if (entry.IsSoftInputShowing())
    await entry.HideSoftInputAsync(System.Threading.CancellationToken.None);
```

If the `SoftInputExtensions` or other alternative solutions do not work for your keyboard focus needs, the .NET MAUI team would love to learn more about your scenario. Please share with us so that we can better understand your development needs!

That being said, the optional `HideSoftInputOnTapped` property was [introduced in .NET 8](https://github.com/dotnet/maui/pull/16530). Applying this property enables users to tap on a page to hide the soft input keyboard, and we recommend it only be used in exceptional scenarios.

### 3. Keyboard Accelerators

As with all awesome, accessible solutions, designing with accessibility in mind means designing for all. This is especially the case for keyboard accessibility, where enabling nifty keyboard behaviors benefits all keyboard users, from those who leverage keyboard as their primary input mode, to power users who are partial to using keyboard shortcuts, also known as keyboard accelerators.

In .NET MAUI, we built out a solution for [keyboard accelerators](https://learn.microsoft.com/dotnet/maui/user-interface/keyboard-accelerators?view=net-maui-8.0). With keyboard accelerators, all keyboard and desktop users can leverage keyboard shortcuts to activate menu item commands!

![Screenshot of menu bar item keyboard accelerators](./screenshot-menu-bar-item-keyboard-accelerators.png) ![Screenshot of context menu item keyboard accelerators](./screenshot-context-menu-item-keyboard-accelerators.png)

As captured in .NET MAUI documentation, here is how you can get started with attaching keyboard accelerators to a `MenuFlyoutItem` in XAML or C#:

```xaml
<MenuFlyoutItem Text="Cut"
                Clicked="OnCutMenuFlyoutItemClicked">
    <MenuFlyoutItem.KeyboardAccelerators>
        <KeyboardAccelerator Modifiers="Ctrl"
                             Key="X" />
    </MenuFlyoutItem.KeyboardAccelerators>
</MenuFlyoutItem>
```

```cs
cutMenuFlyoutItem.KeyboardAccelerators.Add(new KeyboardAccelerator
{
    Modifiers = KeyboardAcceleratorModifiers.Ctrl,
    Key = "X"
});
```

Be sure to include the keyboard accelerators in your .NET MAUI app if you aren't already, and apply your new knowledge from WCAG Success Criterion 2.1.4!

## The Journey to Accessible Apps

With .NET MAUI, you have the power to build your apps to be fully keyboard accessible and void of keyboard traps, and to do so more easily than ever before.

If you are new to The Journey to Accessible Apps, welcome! Be sure to check out my [previous blog posts](https://devblogs.microsoft.com/xamarin/author/rachelkang/) to learn more about building accessible apps, and how .NET MAUI makes it easy.

You can gain more context about other keyboard accessibility improvements made in .NET MAUI, by checking out the last blog post on [meaningful content ordering and the decision to remove tab index](https://devblogs.microsoft.com/xamarin/the-journey-to-accessible-apps-meaningful-content-ordering/).

.NET MAUI helps you to build accessible apps more easily than ever before. As always, let us know how we can make it even easier for you!
