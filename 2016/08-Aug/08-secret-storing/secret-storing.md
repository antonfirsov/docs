Storing secrets in an Azure application
=======================================

Most applications need access to secret information in order to function: it could be an API key, database credentials, or many other things. There are or course many different places where people store such secrets. From worse to best, one could think of the following: in your source code repository on GitHub (of course, nobody should ever do that), in config files, in environment variables, or in specialized secret vaults.

In this post, we'll create a simple service that will calculate the monthly payments on a house using data from [the Zillow API](http://www.zillow.com/howto/api/GetMonthlyPayments.htm), for which we'll need a secret API key. I'll walk you through the usage of [Azure's KeyVault](https://azure.microsoft.com/en-us/services/key-vault/) for storing the key, then I'll show how to retrieve and use it in a simple Azure function.

Prerequisites
-------------

In order to be able to follow along, you'll need an [Azure account](https://azure.microsoft.com/pricing/free-trial/), and [the Azure command-line](https://github.com/azure/azure-xplat-cli).

Setting up KeyVault
-------------------

1. Log-in your PowerShell Azure console using `azure login`:
   ![Log-in from PowerShell](01-AzureLogin.png)
   Follow the instructions on the screen, which will likely include using a browser to enter a validation code.

2. Select the subscription you want to use, if you have more than one, using `azure account set "name of the subscription"`:
   ![Pick a subscription](02-PickSubscription.png)

3. 