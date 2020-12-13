using KafkaBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Repository;

namespace Stock
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
            services.AddDbContext<StockDbContext>(opt =>
                    opt.UseInMemoryDatabase("Stock"))
                .AddControllers();

            services.AddSingleton<IKafkaMessageProducer, KafkaMessageProducer>(x =>
                new KafkaMessageProducer(new KafkaProducerConfiguration
                {
                    KafkaHost = Configuration.GetValue<string>("KafkaHost"),
                }));
            services.AddTestProducts();

            services.AddConsumers()
                .AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger().UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock");
                x.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}