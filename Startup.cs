using LineDC.CEK;
using LineDC.Messaging;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using KatsuzetsuApp.Settings;

[assembly: FunctionsStartup(typeof(KatsuzetsuApp.Startup))]
namespace KatsuzetsuApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddClova<ILineMessageableClova, KatsuzetsuClova>()
                .AddSingleton<ILineMessagingClient, LineMessagingClient>(_ => LineMessagingClient.Create(Keys.token));
        }
    }
}
