using HelpDesk.Configurations;
using HelpDesk.Models;
using HelpDesk.Models.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDesk.Services.Extenstions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.Configure<HelpDeskSettings>(configuration.GetSection(nameof(HelpDeskSettings)));
            services.AddSingleton<IDataContext, DataContext>();
            return services;
        }
    }
}
