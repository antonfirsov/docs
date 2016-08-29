Storing secrets in an Azure application
=======================================

Most applications need access to secret information in order to function: it could be an API key, database credentials, or many other things. There are or course many different places where people store such secrets. From worse to best, one could think of the following: in your source code repository on GitHub (of course, nobody should ever do that), in config files, in environment variables, or in specialized secret vaults.

KeyVault's unique responsibility is to store your secrets securely. Once stored, your secrets can only be accessed by applications you authorize, and only on an encrypted channel.

In this post, we'll create a simple service that will compare the temperatures in Seattle and Paris using the [OpenWeatherMap API](http://openweathermap.org/api), for which we'll need a secret API key. I'll walk you through the usage of [Azure's KeyVault](https://azure.microsoft.com/en-us/services/key-vault/) for storing the key, then I'll show how to retrieve and use it in a simple Azure function.

Prerequisites
-------------

In order to be able to follow along, you'll need an [Azure account](https://azure.microsoft.com/pricing/free-trial/), and [the Azure command-line](https://github.com/azure/azure-xplat-cli).

Setting up KeyVault
-------------------

First, we're going to set-up KeyVault. There are quite a few steps involved, but only steps 6-8 have to be repeated for new secrets, the others being the one-time building of the vault.

1. Log your PowerShell Azure console in using `azure login`:
   ![Log-in from PowerShell](01-AzureLogin.png)
   Follow the instructions on the screen, which will likely include using a browser to enter a validation code.

2. Select the subscription you want to use, if you have more than one, using `azure account set "name of the subscription"`:
   ![Pick a subscription](02-PickSubscription.png)

3. Create a new resource group (change the location as needed):
   `azure group create sample-weather-group -l WestUS`.
   You can skip this if you already have a resource group that you want to use.

4. Register KeyVault with your subscription:
   `azure provider register Microsoft.KeyVault`.
   This only needs to be done once per subscription.

5. Create a new vault under the group we created above (change the location as needed):
   `azure keyvault create sample-weather-vault -g sample-weather-group -l WestUS`

6. Get an API key from [OpenWeatherMaps](http://openweathermap.org/api).

7. Create the key in the vault:
   `azure keyvault key create sample-weather-vault open-weather-map-key -d software`
   
8. Store the API key on KeyVault (replace the 'x's with your actual key):
   `azure keyvault secret set sample-weather-vault  open-weather-map-key -w xxxxxxxxxxxxxxxxxxxxxxxxxxxx`

Creating the Azure function
---------------------------

We're going to use Azure Functions to implement the actual service, because it's the easiest way to write code on Azure, but roughly the same steps would apply to any other kind of application.

1. From the [the Azure portal](https://portal.azure.com/), click "New", and pick "Function App" under "Web + Mobile",

   ![Creating a new Azure Function App](03-new-azure-function.png)

   then name your new function app "sample-weather".

   ![Setting up the new function app](04-setting-up-the-function.png)

2. Navigate to the new function app under your subscription, and create a new function. Name it "ParisSeattleWeatherComparison" and choose the empty C# template:

   !Creating the function[](05-creating-the-function.png)

References
----------

1. [Manage Key Vault using CLI](https://azure.microsoft.com/en-us/documentation/articles/key-vault-manage-with-cli/)