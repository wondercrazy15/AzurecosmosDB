using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureCosmosDBApplication.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace AzureCosmosDBApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string URL = "http://localhost:59082/api/";

        public async Task<IActionResult> Index()
        {
            CustomerRegModel customerRegModel = new CustomerRegModel();
            customerRegModel.customerRegModelsList = await this.GetCustomersListAsync();
            return View(customerRegModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> RegisterAsync(CustomerRegModel customerRegModel)
        {
            try
            {
                if (string.IsNullOrEmpty(customerRegModel.id))
                {
                    #region Save
                    customerRegModel.id = customerRegModel.FirstName + "_" + DateTime.Now.TimeOfDay.Milliseconds;
                    var customerData = JsonConvert.SerializeObject(customerRegModel);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(customerData);
                    var byteData = new ByteArrayContent(buffer);
                    byteData.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(URL);
                    var response = await client.PostAsync("CustomerRegister", byteData);
                    client.Dispose();
                    #endregion
                }
                else
                {
                    #region Update
                    var customerData = JsonConvert.SerializeObject(customerRegModel);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(customerData);
                    var byteData = new ByteArrayContent(buffer);
                    byteData.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(URL);
                    var response = await client.PostAsync("UpdateCustomer", byteData);
                    client.Dispose();
                    #endregion

                }

                return RedirectToAction("Index", "Home");
                // return View("Index", new CustomerRegModel());
            }
            catch (Exception)
            {
                return View("Index", customerRegModel);
            }

        }

        public async Task<List<CustomerRegModel>> GetCustomersListAsync()
        {
            List<CustomerRegModel> customerRegModels = new List<CustomerRegModel>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);

                var response = client.GetAsync("GetCustomersLists");
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = await result.Content.ReadAsStringAsync();
                    customerRegModels = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CustomerRegModel>>(readTask);
                }
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
            return customerRegModels;
        }

        public JsonResult DeleteCustomerById(string Id)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL + "DeleteCustomer?CustomerId=" + Id);

                var response = client.DeleteAsync("");
                var result = response.Result;

                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
            return Json("");
        }

    }
}
