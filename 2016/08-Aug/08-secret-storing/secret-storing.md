Storing and using secrets in Azure
==================================

Most applications need access to secret information in order to function: it could be an API key, database credentials, or something else. In this post, we'll create a simple service that will compare the temperatures in Seattle and Paris using the [OpenWeatherMap API](http://openweathermap.org/api), for which we'll need a secret API key. I'll walk you through the usage of [Azure's Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) for storing the key, then I'll show how to retrieve and use it in a simple [Azure function](https://azure.microsoft.com/en-us/documentation/articles/functions-reference-csharp/).

Prerequisites
-------------

In order to be able to follow along, you'll need an [Azure subscription](https://azure.microsoft.com/pricing/free-trial/). It's very easy to get a trial subscription, and get started for free.

Where to store secrets?
-----------------------

There are of course many different places where people store such secrets. From worst to best, one could think of the following: in your source code repository on GitHub (of course, nobody should ever do that), in config files ([encrypted](http://docs.asp.net/en/latest/security/app-secrets.html) or not), in environment variables, or in specialized secret vaults.

Which one you choose depends on the level of security your application requires. Oftentimes, storing an API key in an environment variable will be adequate (what is never adequate is hard-coded values in code or config files checked into source control). If you require a higher level of security, however, you'll need a specialized vault such as Azure Key Vault. 

Azure Key Vault is a service that stores and retrieves secrets in a secure fashion. Once stored, your secrets can only be accessed by applications you authorize, and only on an encrypted channel. Each secret can be managed in a single secure place, while multiple applications can use it. 

Setting up Key Vault
-------------------

First, we're going to set-up Key Vault. This requires a few steps, but only steps 4 and 5 have to be repeated for new secrets, the others being the one-time building of the vault.

1. Open the [Azure portal](https://portal.azure.com) and click on **Resource groups**. Choose an existing group, or create a new one. This is a matter of preference, but is otherwise inconsequential from a security standpoint. For this tutorial, we'll create a new one called "sample-weather-group". After clicking the **Create** button, you may have to wait a few seconds and refresh the list of resource groups.

   ![Creating a new resource group](rg-01-create.png)

2. Select the newly created group, then click the **Add** button over its property page. Enter "Key Vault" in the search box, select the Key Vault service, then click **Create**.

   ![Adding the Key Vault service to the resource group](rg-02-add-key-vault.png)

3. Enter "sample-weather-vault" as the name of the new vault. Select the right subscription and location, and leave the "sample-weather-group" resource group selected. Click **Create**.

   ![Creating a new vault](rg-03-create-key-vault.png)

4. If you refresh the resource group property page, you'll see the new vault appear. We're now ready to add a key to it. Get an API key from [OpenWeatherMaps](http://openweathermap.org/api). Select the vault in the list of resources under the resource group, then select **Secrets**. You can now click **Add** to add a new secret. Under **Upload options**, select **Manual**. Enter "open-weather-map-key" as the name of the secret, and paste the API key from OpenWeatherMaps into the value field. Click **Create**.

   ![Storing the secret in Key Vault](rg-04-create-secret.png)

5. We will later need the URL for the secret we just created. This can be found under **Secret Identifier** on the property page of the current version of the secret, which can be reached by navigating to **Secrets** under the key vault, then clicking the secret, and then its latest version.

   ![Getting the URL for the secret](rg-05-secret-url.png)

And this is it for now for Key Vault: we now have a vault, containing our secret. Next, we'll need to setup access so that we can securely retrieve the key from our application.

Preparing Active Directory authentication
-----------------------------------------

The application will need to securely connect to the vault, for which it will have to prove its identity, using some form of master secret. This is similar to the master password that a password vault uses, and makes sure the identity of our application can be managed independently from the secret, which may be shared with more than one application. We'll use Active Directory for this.

In this post, we'll authenticate using a secret key, but it's important to note that it is possible to use a certificate instead for added security. Please refer to [Authenticate with a Certificate instead of a Client Secret](https://azure.microsoft.com/en-us/documentation/articles/key-vault-use-from-web-application/#_authenticate-with-a-certificate-instead-of-a-client-secret) for more information.

1. To access Active Directory, in [the Azure portal](https://portal.azure.com), select **More Services** and choose **Azure Active Directory** (currently in preview). In the next menu that will appear, click **App registrations**. Click the **Add** button above the list of applications. You'll be asked for a name for the application. We'll choose "sample-weather-ad". For the application type, leave the default **Web app / API** selected. We also have to provide a sign-on URL. For our purposes, this doesn't need to actually exist, but only to be unique. Click the **Create** button.

   ![Naming the AD application](ad-03-new-app-name.png)

2. Now that the application has been created, select it in the application list, so that you can see your application's [security principal](https://en.wikipedia.org/wiki/Principal_(computer_security)) ID, which can be found under **Managed Application In Local Directory** in the application's property page. This principal ID will represent the authenticated identity of our application. You'll need that and a key.

   ![Viewing the application's principal ID](ad-04-app-id.png)

   Currently, this principal ID does not get automatically generated until the Active Directory application is logged into for the first time. This is a temporary issue that is being looked into. In the meantime, it can be worked around by browsing to `<oauth 2.0 authorization endpoint URL>?client_id=<application_id>`, where `<oauth 2.0 authorization endpoint URL>` can be found by clicking the **Endpoints** button above the list of registered apps, and `<application_id>` can be found under the property page for the application.

   ![Getting the authorization URL](ad-05-get-auth-url.png)

   Navigating to the URL we composed will require you to authenticate with the credentials of a user that has admin rights on the subscription, and then it will yield an error page that can be safely ignored. If all went well, you should now be able to see your application's principal ID in the property page.

   ![Getting the AD application's principal ID](ad-05b-get-principal.png)

3. We can now proceed to create credentials that our Azure Function will be able to use to authenticate to Active Directory as the application we created earlier. Click on **All settings**, then select **Keys**. We can add a new key by entering a description, selecting an expiration, and hitting the **Save** button. If you do choose to have the key expire, you should also take the time to create a reminder on the schedule of the team in charge of managing this application to renew it.

   ![Adding a new key](ad-06-add-key.png)

   Note that using a different key and id for each application that will use the secrets makes it possible to revoke access to the whole vault for a specific application in one operation.

4. Once you've saved, the key can be viewed and copied to a safe place. Do it now, because this is the last time the Azure portal is going to show it.

   ![The generated key](ad-07-the-key.png)

5. We'll also need the URL of the Active Directory endpoint. This is not the URL that we manually entered earlier when we created the AD application. It can be obtained by clicking the **Endpoints** button above the list of applications.

   ![The "Endpoints" button](ad-08-getting-the-URL.png)

   The URL we want to copy for later use is the one under **OAuth 2.0 Token Endpoint**.

6. We're now ready to authorize the application to access the vault and get values out of it. Navigate back to the key vault's property page, and select the vault we created earlier, then **Access policies**. Click **Add new**, then **Select principal**. Paste the principal ID into the text box. After a few seconds, the principal should appear checked. Click the **Select** button. Then click on **Secret permissions** and check **Get**, then click **OK**.

   ![Adding permissions for AD](rg-06-add-access.png)

We now have a set of Active Directory credentials that our Azure Function will be able to use, that will enable read access to the secrets in the key vault. Our last remaining step is to create the actual code.

Creating the Azure function
---------------------------

We're going to use Azure Functions to implement the actual service, because it's the easiest way to write code on Azure, but roughly the same steps would apply to any other kind of application.

1. From the resource group's property page, click **Add**, and type "Function App" in the filter box. Select **Function App**, then click **Create**. Name your new function app "sample-weather". Select the relevant subscription, resource group, plan, and location.

   ![Setting up the new function app](04-setting-up-the-function.png)

2. Refresh the resource group's property page, then select the new "sample-weather" app service. Create a new function under the function app. Name it "ParisSeattleWeatherComparison" and choose the empty C# template:

   ![Creating the function](05-creating-the-function.png)

   Now we have an empty function. Let's add some code.

3. Click **View Files** under the code editor. This shows the list of files in the function's directory. Click the **+** icon to add a new file, and name it "weather.csx". Enter the following code in that file:

    ```csharp
    using System.Collections.Generic;

    public class WeatherList
    {
        public IEnumerable<Weather> List { get; set; }
    }

    public class Weather
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public WeatherData Main { get; set; }
    }

    public class WeatherData
    {
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
    ```

   Those classes will help deserialize the response from the weather server. The structure of the types `WeatherList`, `Weather`, and `WeatherData` reflects [the schema of the JSON documents that the weather service will return](http://openweathermap.org/current). It does not, nor needs to reflect the entirety of the schema returned by the API: the `Microsoft.AspNet.WebApi.Client` library will figure out which parts to deserialize and how to map them onto the provided object model.

4. Enter the following code as the body of the function:

    ```csharp
    // Bring some references in
    // (this won't be a requirement in future versions of Azure Functions)
    #r "System.Runtime"
    #r "System.Threading.Tasks"

    // Include the data model classes
    #load "weather.csx"

    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Azure.KeyVault;

    public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
    {
        // City codes from http://bulk.openweathermap.org/sample/city.list.json.gz
        const int parisId = 6455259;
        const int seattleId = 5809844;

        // Fetch configuration values
        var adUrl = ConfigurationManager.AppSettings["WeatherADUrl"];
        var adClientId = ConfigurationManager.AppSettings["WeatherADClientID"];
        var adKey = ConfigurationManager.AppSettings["WeatherADKey"];
        var keyUrl = ConfigurationManager.AppSettings["WeatherKeyUrl"];
        
        // Create a Key Vault client with an Active Directory authentication callback
        var keyVault = new KeyVaultClient(async (string authority, string resource, string scope) => {
            var authContext = new AuthenticationContext(authority);
            var credential = new ClientCredential(adClientId, adKey);
            var token = await authContext.AcquireTokenAsync(resource, credential);

            return token.AccessToken;
        });

        // Get the API key out of the vault
        var apiKey = keyVault.GetSecretAsync(keyUrl).Result.Value;

        // Send a request to the remote API for weather data for Seattle and
        // Paris, using the API key we got out of the vault.
        using (var weatherClient = new HttpClient())
        {
            weatherClient.BaseAddress = new Uri("http://api.openweathermap.org/");
            var weatherResponse = await weatherClient.GetAsync($"data/2.5/group?id={parisId},{seattleId}&units=metric&APPID={apiKey}");

            if (weatherResponse.IsSuccessStatusCode)
            {
                // Deserialize the API response into a data model
                var weather = await weatherResponse.Content.ReadAsAsync<WeatherList>();
                // Extract the Paris temperature from the data model
                var parisTemperature = weather.List.Where(city => city.Id == parisId).FirstOrDefault()?.Main.Temp;
                // Extract the Seattle temperature from the data model
                var seattleTemperature = weather.List.Where(city => city.Id == seattleId).FirstOrDefault()?.Main.Temp;
                if (parisTemperature != null && seattleTemperature != null)
                {
                    // Compare the temperatures and write out the result
                    if (parisTemperature > seattleTemperature)
                    {
                        return req.CreateResponse(HttpStatusCode.OK, 
                            $"It's nicer in Paris ({parisTemperature}°C) than in Seattle ({seattleTemperature}°C) right now.");
                    }
                    else
                    {
                        return req.CreateResponse(HttpStatusCode.OK, 
                            $"It's nicer in Seattle ({seattleTemperature}°C) than in Paris ({parisTemperature}°C) right now.");
                    }
                }
            }
        }
        return req.CreateResponse(HttpStatusCode.InternalServerError, $"Something went wrong.");
    }
    ```

   We also need to define the bindings for the function. Open `function.json` and enter the following as its content.

   ```json
    {
    "bindings": [
        {
            "authLevel": "function",
            "name": "req",
            "type": "httpTrigger",
            "direction": "in"
        },
        {
            "name": "res",
            "type": "http",
            "direction": "out"
        }
    ],
        "disabled": false
    }
   ```

   This tells Azure Functions what objects to inject into the function.

5. In the code above, you'll notice that we're reading the AD URL, client ID and key from configuration, because of course we haven't done all this to end up storing secrets in code. For the code to function, we'll have to enter that information into the function's Azure configuration. This can be done by clicking **Function app settings** on the bottom-left of the function editing screen.

    ![The function app settings button](fun-01-config.png)

    In the screen this brings up, you'll want to select the **Configure app settings** option. Once there, you'll see a few general settings, but what we're interested in is the table of custom **App settings**. We'll add new key-value pairs in there with the names we used in the code:
    
    * "WeatherADURL" with the Active Directory OAuth 2.0 Token Endpoint URL.
    * "WeatherADClientID" with the Active Directory application ID we got in the previous section.
    * "WeatherADKey" with the Active Directory application key.
    * "WeatherKeyUrl" with the URL for the secret API key we stored in the vault earlier.
    
    Don't forget to hit **Save** on top of the panel.

    ![Setting the Active Directory URL, master id and key in the app settings](fun-02-settings.png)

6. Go back to the function editor and add a `project.json` file to import the NuGet packages we need.

    ![Adding a project.json file](fun-04-adding-project-json.png)

   Enter the following code into the file.

    ```json
    {
        "frameworks": {
            "net46": {
                "dependencies": {
                    "Microsoft.IdentityModel.Clients.ActiveDirectory": "3.13.4",
                    "Microsoft.Azure.KeyVault": "2.0.1-preview",
                    "Microsoft.AspNet.WebApi.Client": "5.2.3"
                }
            }
        }
    }
    ```

And this is it. Once you've saved all the files, you should see the trace of the package restoration and compilation of the function in the logs window. After that's completed, you should be able to click the **Run** button and figure out where the weather is nicest, between Paris and Seattle.

![Where is the weather the nicest?](fun-05-result.png)

Summary
-------

We now have a function that connects to an Azure Key Vault using Azure Active Directory authentication, and then uses a secret stored in the vault to query a remote service.

Wait, what about .NET Core?
---------------------------

Before I conclude, and now that we've made this work in Azure Functions, how about re-using the knowledge we've gained in a different kind of application, such as a .NET Core console application? We actually wouldn't have to change much. First, we'd need to check our dependencies to make sure that everything we need is available on .NET Core, then we'd have to modify the code so that it reads configuration using the new [`ConfigurationBuilder`](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Configuration/ConfigurationBuilder/). Finally, we'd just change `req.CreateResponse(HttpStatusCode.OK, $"...");` into simple `Console.WriteLine` calls.

Here's what the code looks like once ported to a .NET Core console app.

```csharp
public static void Main(params string[] args)
{
    MainAsync(args).Wait();
}

public static async Task MainAsync(params string[] args)
{
    // City codes from http://bulk.openweathermap.org/sample/city.list.json.gz
    const int parisId = 6455259;
    const int seattleId = 5809844;

    var configurationBuilder = new ConfigurationBuilder();
    var config = configurationBuilder
        .AddEnvironmentVariables("WeatherSample")
        .AddCommandLine(args)
        .Build();

    var adUrl = config["WeatherADUrl"];
    var adClientId = config["WeatherADClientID"];
    var adKey = config["WeatherADKey"];
    var keyUrl = config["WeatherKeyUrl"];

    var keyVault = new KeyVaultClient(async (string authority, string resource, string scope) => {
        var authContext = new AuthenticationContext(authority);
        var credential = new ClientCredential(adClientId, adKey);
        var token = await authContext.AcquireTokenAsync(resource, credential);

        return token.AccessToken;
    });

    var apiKey = keyVault.GetSecretAsync(keyUrl).Result.Value;

    using (var weatherClient = new HttpClient())
    {
        weatherClient.BaseAddress = new Uri("http://api.openweathermap.org/");
        var weatherResponse = await weatherClient.GetAsync($"data/2.5/group?id={parisId},{seattleId}&units=metric&APPID={apiKey}");
        if (weatherResponse.IsSuccessStatusCode)
        {
            var weather = await weatherResponse.Content.ReadAsAsync<WeatherList>();
            var parisTemperature = weather.List.Where(city => city.Id == parisId).FirstOrDefault()?.Main.Temp;
            var seattleTemperature = weather.List.Where(city => city.Id == seattleId).FirstOrDefault()?.Main.Temp;
            if (parisTemperature != null && seattleTemperature != null)
            {
                if (parisTemperature > seattleTemperature)
                {
                    Console.WriteLine($"It's nicer in Paris ({parisTemperature}C) than in Seattle ({seattleTemperature}C) right now.");
                }
                else
                {
                    Console.WriteLine($"It's nicer in Seattle ({seattleTemperature}C) than in Paris ({parisTemperature}C) right now.");
                }
            }
        }
    }

    Console.ReadKey();
}
```

And here's the `project.json` that enables it to restore the right packages.

```json
{
  "version": "1.0.0-*",
  "buildOptions": {
    "emitEntryPoint": true
  },

  "dependencies": {
    "Microsoft.AspNet.WebApi.Client": "5.2.3",
    "Microsoft.Azure.KeyVault": "2.0.1-preview",
    "Microsoft.Extensions.Configuration": "1.0.0",
    "Microsoft.Extensions.Configuration.CommandLine": "1.0.0",
    "Microsoft.Extensions.Configuration.EnvironmentVariables": "1.0.0",
    "Microsoft.IdentityModel.Clients.ActiveDirectory": "3.13.4",
    "Microsoft.NETCore.App": {
      "type": "platform",
      "version": "1.0.0"
    },
    "System.Runtime.Serialization.Xml": "4.3.0-preview1-24528-02"
  },

  "frameworks": {
    "netcoreapp1.0": {
      "imports": [ "portable-net451+win8" ]
    }
  }
}
```

The weather data model remains unchanged from the Azure Functions version.

And that's it, this console application will, like the Azure Function, authenticate to Active Directory, get the API key from Key Vault, and then query the API and tell you about the weather.

Please let us know if tutorials like these are helpful. Happy programming!

References
----------

1. [Manage Key Vault using CLI](https://azure.microsoft.com/en-us/documentation/articles/key-vault-manage-with-cli/)
2. [Azure Key Vault .NET Samples](https://github.com/Azure/azure-sdk-for-net/tree/AutoRest/src/KeyVault/Microsoft.Azure.KeyVault.Samples)
3. [Azure Functions C# Reference](https://azure.microsoft.com/en-us/documentation/articles/functions-reference-csharp/)
4. [Safe storage of app secrets during development](http://docs.asp.net/en/latest/security/app-secrets.html)