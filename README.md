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

[Rick van den Bosch - Blog](https://www.rickvandenbosch.net/blog)
