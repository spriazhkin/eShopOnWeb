using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace OrderItemsReserver
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Blob("orders/{rand-guid}.json", FileAccess.Write, Connection = "AzureWebJobsStorage")] TextWriter jsonStorage,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDto data;
            try
            {
                data = JsonConvert.DeserializeObject<OrderDto>(requestBody);
            }
            catch
            {
                return new BadRequestObjectResult("Unable to parse json");
            }
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(data, new ValidationContext(data), results, true))
            {
                return new BadRequestObjectResult($"Model is invalid: {string.Join(", ", results.Select(s => s.ErrorMessage).ToArray())}");
            }

            await jsonStorage.WriteAsync(requestBody);

            var responseMessage = $"Order for Id {data.ItemId} with Count {data.Count} created successfully";
            return new OkObjectResult(responseMessage);
        }
    }
}
