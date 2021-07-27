using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Services.DeliveryOrderProcessorClient
{
    public class DeliveryOrderProcessorClient : IOrderReserver
    {
        private readonly DeliveryOrderProcessorClientConfiguration _configuration;
        private readonly RestClient _restClient;

        public DeliveryOrderProcessorClient(IOptions<DeliveryOrderProcessorClientConfiguration> configuration)
        {
            _configuration = configuration.Value;
            _restClient = new RestClient(_configuration.AzureFunctionUrl);
        }

        public async Task ReserveOrderAsync(Order order)
        {
            var request = new RestRequest("Function1", Method.POST);
            request.AddHeader("x-functions-key", _configuration.AzureFunctionKey);

            var orderDto = new OrderListDto();
            foreach (var item in order.OrderItems)
            {
                orderDto.Items.Add(new OrderItemDto { ItemId = item.Id.ToString(), Count = item.Units });
            }
            orderDto.FinalPrice = order.Total();
            orderDto.ShippingAddress = order.ShipToAddress.ToString();

            request.AddJsonBody(orderDto);
            await _restClient.ExecuteAsync(request);
        }
    }
}
