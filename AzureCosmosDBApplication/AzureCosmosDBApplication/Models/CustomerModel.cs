using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosDBApplication.Models
{
    public class CustomerModel
    {
    }

    public class CustomerRegModel
    {
        public string id { get; set; }

        [Required(ErrorMessage = "FirstName is required."), MaxLength(256)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "OrderDate is required.")]
        public string OrderDate { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        public List<CustomerRegModel> customerRegModelsList { get; set; } = new List<CustomerRegModel>();
        
    }

}
