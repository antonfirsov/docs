# .NET Support and Versioning

_This post was written by **Lee Coward**._

We’ve travelled many new roads in the process of releasing .NET Core 1.0,
ASP.NET Core 1.0 and Entity Framework Core 1.0. Developing the projects in the
open and delivering to Mac and Linux have been exciting experiences and the
feedback we continue to receive shows that the excitement is shared by many.
Getting 1.0 to you is only the beginning. This post will introduce the ways in
which we will be releasing updates for the .NET Core family. Taking care of the
inevitable things that don’t work quite right, delivering updates, continuing to
innovate and providing a rock solid platform on which enterprises can have
confidence are critical. While our world class developer support team will
continue providing the same great help you’ve come to expect, we will be
managing releases and updates in ways quite different from the .NET Framework.
This will allow us to deliver features as well as enterprise-required stability
and provide you with a way to choose the path of faster innovation or careful
stability.

## LTS and FTS Release Trains

Long Term Support (LTS) and Fast Track Support (FTS) are concepts in use
throughout the software world and should be familiar to many. LTS releases will
be maintained for stability over their lifecycle, receiving fixes for important
issues and security fixes. New feature work and additional bug fixes will take
place in FTS releases. From a support perspective, these release trains have the
following support lifecycle attributes.

LTS releases are -
* Supported for three years after the general availability date of a LTS release
* And one year after the general availability of a subsequent LTS release

FTS releases are -
* Supported within the same three-year window as the parent LTS release
* And three months after the general availability of a subsequent FTS release
* And one year after the general availability of a subsequent LTS release

## Versioning

Talking about versioning for all of the things under the .NET Core umbrella is a
bit tricky. There’s the top level .NET Core version, currently 1.0, the many
NuGet package versions and the binary versions contained within the packages. A
detailed discussion on package and assembly versioning schemes is a bit out of
scope for this document. The takeaway here is that there are multiple and
independent versioned elements of .NET Core. LTS and FTS updates will always
reference the top level and we will provide detailed information on the packages
and binaries included in the update. The
[.NET Core Versioning document](https://docs.microsoft.com/en-us/dotnet/articles/core/versions/index)

does a great job of explaining the different areas and our usage of Semantic
Versioning (`major.minor.patch[-prerelease-buildnumber]`).

LTS and FTS versioning will match the top level .NET Core umbrella version, now
1.0. We’re also talking about adding names to the LTS / FTS pairs to make them
easier to identify. New LTS releases will be marked by an increase in the Major
version number. FTS releases will have the same Major number as the
corresponding LTS train and be marked by an increase in the Minor version
number. Bug fix updates to either train will increment the Patch version.

## Release Cadence

We’re still working through details and are thinking that twice yearly updates
for the FTS train strikes a good balance between speed with which new features
are available to you and time to make good use of them before the next set is
released. Additional scheduled updates could also be made available covering
functional, performance, and other improvements. These would likely release
quarterly.

LTS releases will happen much less frequently as new features and more impacting
innovations are created which warrant introducing a new

## More to Come

This post was an introduction to what we have in mind for .NET Core support and
servicing. There will be two trains which you can choose depending on whether
you need a stable, slow moving platform or want to stay current with the latest
released features. In either case, Microsoft will provide support and servicing
for multiple years so that you can have confidence in whatever train you choose.

Stay tuned for future entries which will go into more detail on topics such as:
how updates are delivered, how updates are consumed, and security updates.
