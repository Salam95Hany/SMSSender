using Microsoft.Extensions.DependencyInjection;
using SMSSender.Messaging.Handlers;
using SMSSender.Messaging.Parsers;
using SMSSender.Messaging.Repositories;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging
{
    public static class MsgStrapping
    {
        public static IServiceCollection Bootstrap(this IServiceCollection services)
        {
            services.AddSingleton<IRegexEngine, RegexEngine>();
            services.AddSingleton<IMessageProviderRegistry, MessageProviderRegistry>();
            services.AddScoped<IMessageProcessingService, MessageProcessingService>();
            services.AddScoped<IMessageLogRepository, MessageLogRepository>();
            services.AddScoped<IFailedSmsLogger, FailedSmsLogger>();

            services.AddScoped<IMessageParser, VodafoneCashParser>();
            services.AddScoped<IMessageParser, VodafoneCashEnParser>();
            services.AddScoped<IMessageParser, InstaPayParser>();

            services.AddScoped<IOperationHandler, DepositHandler>();
            services.AddScoped<IOperationHandler, WithdrawHandler>();
            services.AddScoped<IOperationHandler, CashHandler>();
            services.AddScoped<IOperationHandler, TransferHandler>();
            services.AddScoped<IOperationHandler, BalanceInquiryHandler>();

            return services;
        }
    }
}
