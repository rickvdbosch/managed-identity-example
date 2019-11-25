# Connecting to Azure Resources using Managed Identity

This repo contains some short and quite simple example implementations of using a Managed Identity to connect to Azure resources.

## Azure Resources currently implemented

* Key Vault
* Azure SQL
* Service Bus
* Azure Storage

## Gotcha's

There are a few gotcha's in the implementations. Looking at the Function implementations, connecting to three different types of Azure Resources requires three different types of implementation. For now, they will be documented/explained in comments. 

For more information, have a look at my blog, where I'm currently posting a series on using Managed Identity:

### [Managed Identity - Part I](https://www.rickvandenbosch.net/blog/managed-identity-part-i/)  

This blog post shows you the basics of [Managed Identities for Azure Resources](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/), how to create them and how to give them access rights.

### [Managed Identity - Part II](https://www.rickvandenbosch.net/blog/managed-identity-part-ii/)  

This blog post shows you how to connect your application to different types of Azure resources using Managed Identity.

### [Managed Identity - Part III](https://www.rickvandenbosch.net/blog/managed-identity-part-iii/)  

Normally debugging something when running it locally shouldn’t deserve its own blog post. With Managed Identity there are some things to take into account when debugging since it heavily depends on the Azure platform. This blog post explains it all.
