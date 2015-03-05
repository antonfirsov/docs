### .NET Portability Analyzer

In following Microsoft’s OSS initiative, the .NET Framework team has capitalized on that idea by open-sourcing their [.NET Portability website](https://github.com/Microsoft/dotnet-apiweb) on GitHub. This collects and displays the top used APIs, allows users to search for supported APIs on each platform and displays porting recommendations.

Open sourcing this software shows users a real-world example the ease to migrate code from classic ASP.NET to the new ASP.NET vNext. It’s nice to have a real example, the ones in [ASP.NET Sample](https://github.com/aspnet/home#samples) seem sort of contrived, so with this OSS push, we’ll show them how to make an MVC app and a library.  They’ll be able to see how the new .kproj format can be used to create a library that easily works on multiple platforms (ASP.NET, .NET 4.5, ASP.NET Core 5.0).

In addition, we've also open-sourced [Microsoft.Fx.Portability](https://github.com/Microsoft/dotnet-apiport), a vital component used by all the .NET Portability analyzers to communicate with our back-end services. We've begun to publish nightly builds of our dotnet-apiport repository to [myget](https://www.myget.org/gallery/dotnet-apiport) for external users to consume.  This will allow developers to call the service using the Microsoft.Fx.Portability library and do things like search through the FX catalog for APIs.

Just add a reference to our MyGet feed, https://www.myget.org/F/dotnet-apiport then add a reference to the package: `Install-Package Microsoft.Fx.Portability -IncludePrerelease`.  The code sample below shows how to find matching APIs using our Portability Service.

	public static void Main(string[] args)
	{
    var analysisService = new ApiPortService("https://portability.cloudapp.net", new ProductInformation("MyAPIQueryProgram"));

    Console.WriteLine("Enter API you want to search for:");
    var api = Console.ReadLine();

    var matchingApis = FindMatchingApis(analysisService, api).Result;

    Console.WriteLine("Enter the number of the API you want to get more information about.");

    for (int i = 0; i < matchingApis.Count; i++)
    {
        Console.WriteLine("[" + i + "] " + matchingApis[i].FullName);
    }

    var index = int.Parse(Console.ReadLine());

    var apiToSearchFor = matchingApis[index];
    var apiInformation = GetApi(analysisService, apiToSearchFor.DocId).Result;

    Console.WriteLine("These are the platforms this API is supported on: ");

    foreach (var platform in apiInformation.Supported.Select(x => x.FullName))
    {
        Console.WriteLine(platform);
    }

    Console.WriteLine("Enter any key to quit...");
    Console.ReadKey();
	}
	
	private static async Task<IReadOnlyList<ApiDefinition>> FindMatchingApis(IApiPortService service, string api)
	{
	    var response = await service.SearchFxApiAsync(api, top: 20);
	    return response.Response.ToList();
	}
	
	private static async Task<ApiInformation> GetApi(IApiPortService service, string apiDocId)
	{
	    var response = await service.GetApiInformationAsync(apiDocId);
	    return response.Response;
	}


The .NET Framework team is currently working on making more components/projects available in the dotnet-apiport repository. Watch dotnet-apiport for updates and when we open-source [.NET Portability console tool](https://www.microsoft.com/en-us/download/details.aspx?id=42678) and [VSIX Extension](https://visualstudiogallery.msdn.microsoft.com/1177943e-cfb7-4822-a8a6-e56c7905292b).

#### Sources
* [Scott Hanselman: Getting ready for the future with the Microsoft .NET Portability Analyzer](http://www.hanselman.com/blog/GettingReadyForTheFutureWithTheMicrosoftNETPortabilityAnalyzer.aspx)
* [.NET Framework Blog: Leveraging existing code across .NET platforms](http://blogs.msdn.com/b/dotnet/archive/2014/08/06/leveraging-existing-code-across-net-platforms.aspx)
