.NET Core Image Processing
==========================

Image processing, and in particular image resizing, is a common requirement for web applications. As such, I wanted to paint a panorama of the options that exist for .NET Core to process images. For each option, I'll give a code sample for image resizing, and I'll outline interesting features. I'll conclude with a comparison of the performance of the libraries, in terms of speed and size of the output.

CoreCompat.System.Drawing
-------------------------

The easiest way to process images with .NET Framework is to use [the built-in System.Drawing APIs](https://msdn.microsoft.com/en-us/library/mt481535(v=vs.110).aspx). The implementation in .NET Framework is however relying on the GDI+ features in Windows, and was not included in .NET Core 1.x, which needs to be cross-platform. A new, cross-platform implementation will be part of [.NET Standard 2.0](), but in the meantime, the APIs can still be used, thanks to CoreCompat.System.Drawing.

This library is a .NET Core port of [the Mono implementation of System.Drawing](https://github.com/mono/mono/tree/master/mcs/class/System.Drawing).

If you have existing code relying on System.Drawing, using this library is clearly your fastest path to .NET Core and cross-platform bliss: the performance and quality are fine, and the API is exactly the same. Be careful however when using the library cross-platform, to include the [runtime.osx.10.10-x64.CoreCompat.System.Drawing](https://www.nuget.org/packages/runtime.osx.10.10-x64.CoreCompat.System.Drawing) and / or [runtime.linux-x64.CoreCompat.System.Drawing](https://www.nuget.org/packages/runtime.linux-x64.CoreCompat.System.Drawing/1.0.0-beta009) packages.

```csharp
using System.Drawing;

using (var image = new Bitmap(System.Drawing.Image.FromFile(inputPath)))
{
    int width, height;
    if (image.Width > image.Height)
    {
        width = size;
        height = Convert.ToInt32(image.Height * size / (double)image.Width);
    }
    else
    {
        width = Convert.ToInt32(image.Width * size / (double)image.Height);
        height = size;
    }
    var resized = new Bitmap(width, height);
    using (var graphics = Graphics.FromImage(resized))
    {
        graphics.CompositingQuality = CompositingQuality.HighSpeed;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.DrawImage(image, 0, 0, width, height);
        using (var output = File.OpenWrite(outputPath))
        {
            resized.Save(output, ImageFormat.Jpeg);
        }
    }
}
```

* Nuget: [CoreCompat.System.Drawing](https://www.nuget.org/packages/CoreCompat.System.Drawing/), [runtime.osx.10.10-x64.CoreCompat.System.Drawing](https://www.nuget.org/packages/runtime.osx.10.10-x64.CoreCompat.System.Drawing), and [runtime.linux-x64.CoreCompat.System.Drawing](https://www.nuget.org/packages/runtime.linux-x64.CoreCompat.System.Drawing/1.0.0-beta009)
* GitHub: [CoreCompat / CoreCompat](https://github.com/CoreCompat/CoreCompat)

ImageSharp
----------

ImageSharp is a brand new, pure managed code, and cross-platform image processing library. Because it's 100% managed code, its performance is not as good as that of libraries relying on native OS-specific dependencies, but it remains very reasonable. Its only dependency is .NET itself, which makes it extremely portable: no additional package to install, just reference ImageSharp itself, and you're done.

Be aware that the version of ImageSharp that shows in NuGet is a placeholder, and it's necessary for now to get the actual bits from a [MyGet](https://www.myget.org) feed. This can be done by adding the following `NuGet.config` to the root directory of the project:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="ImageSharp Nightly" value="https://www.myget.org/F/imagesharp/api/v3/index.json" />
  </packageSources>
</configuration>
```

```csharp
using ImageSharp;

Configuration.Default.AddImageFormat(new JpegFormat());

using (var input = File.OpenRead(inputPath))
{
    using (var output = File.OpenWrite(outputPath))
    {
        var image = new Image(input)
            .Resize(new ResizeOptions
            {
                Size = new Size(size, size),
                Mode = ResizeMode.Max
            });
        image.Save(output);
    }
}
```

For a new codebase, the library is surprisingly complete. It includes all the filters you'd expect to treat images, and even includes very comprehensive support for reading and writing [EXIF tags](https://en.wikipedia.org/wiki/Exif):

```csharp
var exif = image.ExifProfile;
var description = exif.GetValue(ImageSharpExifTag.ImageDescription);
var yearTaken = DateTime.ParseExact(
    (string)exif.GetValue(ImageSharpExifTag.DateTimeOriginal).Value,
    "yyyy:MM:dd HH:mm:ss",
    CultureInfo.InvariantCulture)
    .Year;
var author = exif.GetValue(ImageSharpExifTag.Artist);
var copyright = $"{description} (c) {yearTaken} {author}";
exif.SetValue(ImageSharpExifTag.Copyright, copyright);
```

* MyGet: [ImageSharp](https://www.myget.org/feed/imagesharp/package/nuget/ImageSharp)
* GitHub: [JimBobSquarePants / ImageSharp](https://github.com/JimBobSquarePants/ImageSharp)

Magick.NET
----------

```csharp
using ImageMagick;

using (var image = new MagickImage(inputPath))
{
    image.Resize(size, size);
    image.Strip();
    image.Write(outputPath);
}
```

SkiaSharp
---------

```csharp
using SkiaSharp;

using (var input = File.OpenRead(inputPath))
{
    using (var inputStream = new SKManagedStream(input))
    {
        var original = SKBitmap.Decode(inputStream);
        int width, height;
        if (original.Width > original.Height)
        {
            width = size;
            height = original.Height * size / original.Width;
        }
        else
        {
            width = original.Width * size / original.Height;
            height = size;
        }
        var surface = SKSurface.Create(width, height, original.ColorType, original.AlphaType);
        var canvas = surface.Canvas;
        var scale = (float)width / original.Width;
        canvas.Scale(scale);
        var paint = new SKPaint();
        paint.FilterQuality = SKFilterQuality.High;
        canvas.DrawBitmap(original, 0, 0, paint);
        canvas.Flush();

        using (var output = File.OpenWrite(outputPath))
        {
            surface.Snapshot()
                   .Encode(SKImageEncodeFormat.Jpeg, 85)
                   .SaveTo(output);
        }
    }
}
```

Performance
-----------

A word on methodology: I used 12 images with a good variety of subjects, and details that are not too easy to resize, so that defects are easy to spot. The images are roughly one megapixel JPEGs, except for one of the images that is a little smaller. Your mileage may vary, depending on what type of image you need to work with. I'd recommend you try to reproduce these results with a sample of images that corresponds to your own use case.

|         Library | Time (ms) | Size (kB) |
|-----------------|----------:|----------:|
|      ImageSharp |   133 ± 8 |      16.5 |
|   SystemDrawing |    44 ± 7 |       3.9 |
|       MagickNET |    42 ± 5 |       8.1 |
|       SkiaSharp |    17 ± 1 |       4.0 |

Gallery
-------

Here are the resized images. As you can see, the quality varies a lot from one image to the next, and between libraries. You should make a choice based on the constraints of your project, and on the performance vs. quality trade-offs you're willing to make.

Sample code
-----------

My sample code [can be found on GitHub](https://github.com/bleroy/core-imaging-playground). The repository includes [the sample images](https://github.com/bleroy/core-imaging-playground/tree/master/images) I've been using in this post.

