---
post_title: Speech Recognition in .NET MAUI with CommunityToolkit
author1: vlad.antonyuk@outlook.com
post_slug: speech-recognition-in-dotnet-maui-with-community-toolkit
username: vlad.antonyuk@outlook.com
microsoft_alias: kyphi
featured_image: speechbot.png
categories: .NET, .NET MAUI
tags: ios, .net maui, android, windows, macos, tizen, community toolkit, .net maui community toolkit
summary: Learn how you can incorporate Speech Recognition into your .Net MAUI applications
desired_publication_date: 2023-05-31
post_date: 2023-05-31 10:05:00
---

> Note: This is a guest blog post by Vladislav Antonyuk, who is a senior software engineer at DataArt and a core contributor of the .NET MAUI Community Toolkit.

The .NET MAUI Community Toolkit is a collection of extensions and components that can be used to extend the functionality of .NET MAUI apps. The toolkit is open-source and community-driven, and it is constantly being updated with new features and improvements.

One of the features that the .NET MAUI Community Toolkit offers is `Speech To Text`. This allows converting spoken words into text, which can be used in a variety of ways. For example, users could use speech-to-text to create a voice-activated assistant or to transcribe audio recordings.

Here's an example of how to use `SpeechToText` in C#:

```csharp
var isGranted = await SpeechToText.Default.RequestPermissions(cancellationToken);
if (!isGranted)
{
    await Toast.Make("Permission not granted").Show(CancellationToken.None);
    return;
}
var recognitionResult = await SpeechToText.Default.ListenAsync(
                                    CultureInfo.GetCultureInfo("uk-ua"),
                                    new Progress(partialText =>
                                    {
                                        RecognitionText += partialText + " ";
                                    }), cancellationToken);
if (recognitionResult.IsSuccessful)
{
    RecognitionText = recognitionResult.Text;
}
else
{
    await Toast.Make(recognitionResult.Exception?.Message ?? "Unable to recognize speech").Show(CancellationToken.None);
}
```

This code requests microphone and speech recognition permissions, then starts listening for speech input, in a result it will set any recognised text to the `RecognitionText` variable.

<center>

[video width="540" height="540" mp4="https://devblogs.microsoft.com/dotnet/wp-content/uploads/sites/10/2023/05/SpeechToTextWindows-1.mp4"][/video]

</center>

When using `SpeechToText`, it captures and handles all exceptions while returning the result of the operation. However, if you prefer to specifically handle certain exceptions, such as when the user cancels the operation, you can enclose your code within a try/catch block and utilize the `EnsureSuccess` method:

```csharp
var isGranted = await SpeechToText.Default.RequestPermissions(cancellationToken);
if (!isGranted)
{
    await Toast.Make("Permission not granted").Show(CancellationToken.None);
    return;
}
var recognitionResult = await SpeechToText.Default.ListenAsync(
                                    CultureInfo.GetCultureInfo("uk-ua"),
                                    new Progress(), cancellationToken);
recognitionResult.EnsureSuccess();
await Toast.Make($"RecognizedText: {recognitionResult.Text}").Show(cancellationToken);
```

> Note: `SpeechToText` requires additional permissions for the app.
>
> Please read the [documentation](https://learn.microsoft.com/dotnet/communitytoolkit/maui/essentials/speech-to-text) to correctly set up the application.

## Summary

`SpeechToText` is a powerful new feature that can be found as part of the [CommunityToolkit.Maui](https://github.com/CommunityToolkit/Maui) library. It can be used to create a variety of more accurate, more responsive, and more engaging speech-enabled applications.

When utilizing speech-to-text, there are several additional factors to take into account:

- The availability of an Internet connection may be necessary depending on the chosen recognition language. On Windows, the speech recognition system automatically adjusts between online and offline modes based on Internet accessibility.
- The accuracy of speech-to-text is influenced by factors such as microphone quality and the surrounding environment.
- Enhancing the accuracy of speech-to-text can be achieved by using a noise-canceling microphone and speaking clearly and at a slower pace.
- Training the speech recognizer with your own voice is another method to improve the accuracy of speech-to-text.

Finally, be sure to check out the full [release notes for version 5.2.0](https://github.com/CommunityToolkit/Maui/releases/tag/5.2.0) for even more great resources for .NET MAUI developers.
