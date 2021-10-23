---
post_title: .NET Hot Reload Support via CLI
username: scott-h@microsoft.com
microsoft_alias: scothu
categories: .NET, .NET Core
summary: Summary of your post, shown on the home page next to the featured image
desired_publication_date: 2021-10-28
---

Last week, our [blog post](https://devblogs.microsoft.com/dotnet/update-on-net-hot-reload-progress-and-visual-studio-2022-highlights/) and the removal of the Hot Reload capability from the .NET SDK repo led to a lot of feedback from the community. 

First and foremost, we want to apologize. We made a mistake in executing on our decision and took longer than expected to respond back to the community. We have approved the [pull request](https://github.com/dotnet/sdk/pull/22262) to re-enable this code path and it will be in the GA build of the .NET 6 SDK.

As a team, we are committed to .NET being an open platform and doing our development in the open. The very fact that we decided to adopt an open posture by default from the start for developing the Hot Reload feature is a testament to that. That said, like any development team, from time to time we have to look at quality, time, resources to make tradeoffs while continuing to make forward progress. Vast majority of the .NET developers are using Visual Studio, and we want to make sure VS delivers the best experience for .NET 6.  

With the runway getting short for the .NET 6 release and Visual Studio 2022,  we chose to focus on bringing Hot Reload to VS2022 first. We made a mistake in executing on this plan in the way it was carried out. In our effort to scope, we inadvertently ended up deleting the source code instead of just not invoking that code path. We underestimated the number of developers that are dependent upon this capability in their environments across scenarios, and how the CLI was being used alongside Visual Studio to drive inner loop productivity by many. 

We are always listening to our customers’ feedback to deliver on their needs. Thank you for making your feedback heard. We are sorry that we made so many community members upset via this change across many parameters including timing and execution. 

Our desire is to create an open and vibrant ecosystem for .NET. As is true with many companies, we are learning to balance the needs of OSS community and being a corporate sponsor for .NET. Sometimes we don’t get it right. When we don’t, the best we can do is learn from our mistakes and be better moving forward.

Thank you for all of your feedback and your [contributions](https://dot.net/thanks) over the years. We are committed to developing .NET in the open and look forward to continuing to work closely with the community.

Thank you! 🙏