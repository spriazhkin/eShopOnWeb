using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryOrderProcessor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
             [CosmosDB(
                databaseName: "EShop",
                collectionName: "Orders",
                CreateIfNotExists = true,
                ConnectionStringSetting = "CosmosDBConnection")]out dynamic document,
            ILogger log)
        {
            document = null;
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            OrderListDto data;
            try
            {
                data = JsonConvert.DeserializeObject<OrderListDto>(requestBody);
            }
            catch
            {
                return new BadRequestObjectResult("Unable to parse json");
            }
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(data, new ValidationContext(data), results, true))
            {
                return new BadRequestObjectResult($"Model is invalid: {string.Join(", ", results.Select(s => s.ErrorMessage))}");
            }

            document = data;

            var responseMessage = $"Order for {data.FinalPrice} shipped to {data.ShippingAddress}: \"{string.Join("; ", data.Items.Select(d => $"Product id: {d.ItemId}, count: {d.Count}"))}\" created successfully";
            return new OkObjectResult(responseMessage);
        }
    }
}
