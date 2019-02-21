Implementation Microsoft Azure Cosmos DB Solutions

Here we have created two project 
1. AzureCosmosDBApplication
2. AzureCosmosDBApi

1. AzureCosmosDBApplication:
     AzureCosmosDB Application is a simple web application in.Net Core. In this Application, we have simply implemented crud operation and pass the data to AzureCosmosDBApi throw the API call.

2. AzureCosmosDBApi:
    AzureCosmosDB API is a simple web API project in.Net Core. Before starting implementation of Azure Cosmos DB API. First, we have to install Microsoft.Azure.DocumentDB from the NuGet and its reference to our controller.

In this API project declare your EndpointUrl and PrimaryKey. This key you will get from your Azure Cosmos DB account.

For Crud operation we have implemented four methods:
1. CustomerRegister
2. GetCustomersLists
3. UpdateCustomer
4. DeleteCustomer

