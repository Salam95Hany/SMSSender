using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMSSender.Entities.Models;
using SMSSender.Interfaces.Auth;
using SMSSender.Interfaces.Common;
using SMSSender.Interfaces.Repositories;
using SMSSender.Messaging.Handlers;
using SMSSender.Messaging.Parsers;
using SMSSender.Messaging.Providers;
using SMSSender.Messaging.Repositories;
using SMSSender.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging
{
    public static class MsgStrapping
    {
        public static IServiceCollection Bootstrap(this IServiceCollection services)
        {
            services.AddScoped<IMessageProcessingService, MessageProcessingService>();
            services.AddScoped<IMessageLogRepository, MessageLogRepository>();

            services.AddScoped<IProviderMessageParser, VodafoneCashParser>();
            services.AddScoped<IProviderMessageParser, InstaPayParser>();

            services.AddScoped<IMessageProviderDetector, VodafoneCashDetector>();
            services.AddScoped<IMessageProviderDetector, InstaPayDetector>();

            services.AddScoped<IOperationHandler, DepositHandler>();
            services.AddScoped<IOperationHandler, WithdrawHandler>();
            services.AddScoped<IOperationHandler, CashHandler>();

            return services;
        }
    }
}
