using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    public class CompositeOrderReserver : IOrderReserver
    {
        private readonly IReadOnlyCollection<IOrderReserver> _orderReservers;

        public CompositeOrderReserver(IReadOnlyCollection<IOrderReserver> reservers)
        {
            _orderReservers = reservers;
        }

        public async Task ReserveOrderAsync(Order order)
        {
            var tasks = _orderReservers.Select(t => t.ReserveOrderAsync(order));
            await Task.WhenAll(tasks);
        }
    }
}
