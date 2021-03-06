using Core;
using Core.Exceptions;
using Core.Serialization.Newtonsoft;
using Core.WebApi.ExceptionHandling;
using Microsoft.OpenApi.Models;
using System.Net;

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
        services.AddTicketsModule(config);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandlingMiddleware(exception => exception switch
        {
            AggregateNotFoundException _ => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        });

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tickets V1");
            c.RoutePrefix = string.Empty;
        });
    }
}