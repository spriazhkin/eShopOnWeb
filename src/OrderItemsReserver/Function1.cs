using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OrderItemsReserver
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async void Run(
            [ServiceBusTrigger("orders", Connection = "ServiceBusConnection")] string orderItem,
            [Blob("orders/{rand-guid}.json", FileAccess.Write, Connection = "AzureWebJobsStorage")] TextWriter jsonStorage,
            ILogger log)
        {
            log.LogInformation("C# ServiceBus trigger function start processing a message");

            var data = JsonConvert.DeserializeObject<OrderListDto>(orderItem);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(data, new ValidationContext(data), results, true))
            {
                throw new ApplicationException($"Model is invalid: {string.Join(", ", results.Select(s => s.ErrorMessage))}");
            }

            await jsonStorage.WriteAsync(orderItem);
        }
    }
}
