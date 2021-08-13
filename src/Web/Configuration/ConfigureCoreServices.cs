﻿using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Logging;
using Microsoft.eShopWeb.Infrastructure.Services;
using Microsoft.eShopWeb.Infrastructure.Services.DeliveryOrderProcessorClient;
using Microsoft.eShopWeb.Infrastructure.Services.ServiceBusOrderSender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopWeb.Web.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddSingleton<IUriComposer>(new UriComposer(configuration.Get<CatalogSettings>()));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<ServiceBusOrderSenderConfiguration>(configuration.GetSection(nameof(ServiceBusOrderSenderConfiguration)));
            services.Configure<DeliveryOrderProcessorClientConfiguration>(configuration.GetSection(nameof(DeliveryOrderProcessorClientConfiguration)));
            services.AddSingleton<ServiceBusOrderSender>();
            services.AddSingleton<DeliveryOrderProcessorClient>();
            services.AddSingleton<IOrderReserver>(provider =>
            {
                var serviceBusSender = provider.GetRequiredService<ServiceBusOrderSender>();
                var deiveryClient = provider.GetRequiredService<DeliveryOrderProcessorClient>();

                return new CompositeOrderReserver(new IOrderReserver[]
                {
                    serviceBusSender,
                    deiveryClient
                });
            });

            return services;
        }
    }
}
