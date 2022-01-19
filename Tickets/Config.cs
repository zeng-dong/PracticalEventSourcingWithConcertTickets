using Core.Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickets;

public static class Config
{
    public static void AddTicketsModule(this IServiceCollection services, IConfiguration config)
    {
        //services.AddMarten(config, options =>
        //{
        //    options.ConfigureReservations();
        //});
        //services.AddReservations();
        //services.AddMaintainance();
    }
}