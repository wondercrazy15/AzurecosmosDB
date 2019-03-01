using AzureCosmosDBApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Net;

namespace AzureCosmosDBApi.Controllers
{
    public class CustomerController
    {
        private const string EndpointUrl = "https://wondercrazy15.documents.azure.com:443/";
        private const string PrimaryKey = "JyXXB4sSvv8WRHCJIchXNv98vEUCcY0cY3KU6K80x2hS1bmdY2YdAPLj66ArkgmQ46dFmSRraUlqvNlu7Na1Sw==";
        private DocumentClient client;

        private const string databaseName = "NatrixFamilyDB";
        private const string collectionName = "NatrixFamilyCollection";

        [HttpPost]
        [Route("api/CustomerRegister")]
        public async Task<string> CustomerRegisterAsync([FromBody]CustomerRegModel customerRegModel)
        {
            try
            {
                CoonectCosmosDB();
                await this.CreateDocumentIfNotExists(customerRegModel);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        [HttpGet]
        [Route("api/GetCustomersLists")]
        public async Task<List<CustomerRegModel>> GetCustomersLists()
        {
            List<CustomerRegModel> customerRegModelList = new List<CustomerRegModel>();
            try
            {
                CoonectCosmosDB();
                customerRegModelList = this.GetCustomerList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return customerRegModelList;
        }


        [HttpPost]
        [Route("api/UpdateCustomer")]
        public async Task<string> UpdateCustomer([FromBody]CustomerRegModel customerRegModel)
        {
            try
            {
                CoonectCosmosDB();
                await this.ReplaceCustomerDocument(customerRegModel.id, customerRegModel);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        [HttpDelete]
        [Route("api/DeleteCustomer")]
        public async Task<string> DeleteCustomer(string CustomerId)
        {
            try
            {
                CoonectCosmosDB();
                await this.DeleteCustomerDocument(CustomerId);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "success";
        }

        #region  DB Method

        /// <summary>
        /// Cosmos DB Connection
        /// </summary>
        public async Task<string> CoonectCosmosDB()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });

            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection { Id = collectionName });
            return null;
        }

        /// <summary>
        /// CreateDocumentIfNotExists
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="collectionName"></param>
        /// <param name="customerRegModel"></param>
        /// <returns></returns>
        private async Task CreateDocumentIfNotExists(CustomerRegModel customerRegModel)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, customerRegModel.id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), customerRegModel);
                }
                else
                {
                    throw;
                }
            }
        }

        private List<CustomerRegModel> GetCustomerList()
        {
            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // Here we find the Andersen family via its LastName
            List<CustomerRegModel> customerRegModel = this.client.CreateDocumentQuery<CustomerRegModel>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions).ToList();

            return customerRegModel;
        }


        private async Task ReplaceCustomerDocument(string CustomerId, CustomerRegModel updateCustomer)
        {
            await this.client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, CustomerId), updateCustomer);

        }

        private async Task DeleteCustomerDocument(string CustomerId)
        {
            await this.client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, CustomerId));            
        }

        #endregion
    }
}
