using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosDBApi.Models
{
    public class CustomerModel
    {
    }

    public class CustomerRegModel
    {
        //[JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

    }
}
