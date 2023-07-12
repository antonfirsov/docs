---
post_title: What does Azure AD renamed Microsoft Entra ID mean for .NET developers?
author1: jeremy-likness
post_slug: azure-ad-microsoft-entra
username: jeremy-likness
microsoft_alias: jeliknes
featured_image: ./entra.png
categories: .NET Core, ASP.NET Core, Azure, Security
tags: security, identity, auth, authentication, authorization
summary: Azure Active Directory (Azure AD) is being renamed as part of unification with the expanded Microsoft Entra identity and network access product family. This is a name change only and does not require any direct action from .NET developers.
desired_publication_date: 2023-07-12
post_date: 2023-07-12 10:05:00
---

You may have heard that one of the key announcements at [Reimagine secure access with Microsoft Entra](https://aka.ms/MSFTEntra) was that Azure Active Directory (Azure AD) is being renamed to Microsoft Entra ID as part of the ongoing commitment to simplify secure access experiences for everyone. If you haven't already, be sure to read the [official announcement](https://devblogs.microsoft.com/identity/aad-rebrand) by the Microsoft Entra team. They explain why the name is being changed along with some longer term plans. You might wonder what the impact will be on .NET developers.

As mentioned in the other blog post, _there is no action needed from you and your existing identity experiences remain the same_. Quoting the Microsoft Entra team:

> To make the transition seamless, we are not changing any code that would impact functionality or your work. For example, existing login URLs, APIs, PowerShell cmdlets, and libraries within the Microsoft identity platform such as Microsoft Authentication Library (MSAL) are not changing.

 To reiterate, there are _no changes_ to your existing code, including apps that rely on Azure B2C, the Microsoft Identity Platform, or MSAL.

The ASP.NET Core team committed to improving the identity management experience for .NET developers in the .NET 8 timeline. Much of that effort has focused on support for self-hosted identity management and Single Page Applications (SPA). We also recognize the popularity and importance of Azure-managed identities and have partnered with the Microsoft Entra team to address those scenarios. Our teams work together to address feedback and improve the process to discover, learn, and securely implement identity in .NET web applications. In addition to the resources shared by the Microsoft Entra team, I encourage you to take a look at the new developer-centric platform [Microsoft Entra External ID](https://www.microsoft.com/security/business/identity-access/microsoft-entra-external-id) that is in preview today. It provides a hands-on experience of what to expect from the future.

Thank you!

Jeremy Likness and the ASP.NET Core team
