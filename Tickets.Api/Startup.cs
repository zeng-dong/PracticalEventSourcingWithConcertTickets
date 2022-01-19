using Core;
using Core.Serialization.Newtonsoft;
using Microsoft.OpenApi.Models;

namespace Tickets.Api;

public class Startup
{
    private readonly IConfiguration config;

    public Startup(IConfiguration config)
    {
        this.config = config;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
          .AddNewtonsoftJson(opt => opt.SerializerSettings.WithDefaults());

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tickets", Version = "v1" });
        });

        services.AddCoreService();
        //services.AddTicketsModule(config);
    }
}