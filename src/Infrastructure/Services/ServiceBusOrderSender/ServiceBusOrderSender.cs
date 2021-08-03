using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Infrastructure.Services.ServiceBusOrderSender
{
    public class ServiceBusOrderSender : IOrderReserver
    {
        private readonly ServiceBusOrderSenderConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;

        public ServiceBusOrderSender(IOptions<ServiceBusOrderSenderConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _client = new ServiceBusClient(_configuration.ServiceBusConnectionString);
            _sender = _client.CreateSender(_configuration.QueueName);
        }

        public async Task ReserveOrderAsync(Order order)
        {
            var orderDto = new OrderListDto();
            foreach (var item in order.OrderItems)
            {
                orderDto.Items.Add(new OrderItemDto { ItemId = item.Id.ToString(), Count = item.Units });
            }
            orderDto.FinalPrice = order.Total();
            orderDto.ShippingAddress = order.ShipToAddress.ToString();

            var body = JsonConvert.SerializeObject(orderDto);

            var message = new ServiceBusMessage(body)
            {
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(message);
        }
    }
}
