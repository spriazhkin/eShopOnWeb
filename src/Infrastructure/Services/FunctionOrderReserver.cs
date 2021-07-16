using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    public class FunctionOrderReserver : IOrderReserver
    {
        private readonly FunctionOrderReserverConfiguration _configuration;
        private readonly RestClient _restClient;

        public FunctionOrderReserver(IOptions<FunctionOrderReserverConfiguration> configuration)
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

            request.AddJsonBody(orderDto);
            await _restClient.ExecuteAsync(request);
        }
    }
}
