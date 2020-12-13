using KafkaBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Repository;

namespace Wallet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WalletDbContext>(opt =>
                    opt.UseInMemoryDatabase("Wallet"))
                .AddControllers();

            services.AddSingleton<IKafkaMessageProducer, KafkaMessageProducer>(x =>
                new KafkaMessageProducer(new KafkaProducerConfiguration
                {
                    KafkaHost = Configuration.GetValue<string>("KafkaHost"),
                }));

            services.AddTestWallet()
                .AddConsumers()
                .AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger().UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet");
                x.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}