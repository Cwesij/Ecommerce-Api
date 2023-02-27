using System;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // store the host in a variable
            var host = CreateHostBuilder(args).Build();

            // create a scope variable
            var scope = host.Services.CreateScope();

            // add the store context to the service provider
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

            // log the errors using the logger service
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            // try to seed data
            try
            {
                // update database
                context.Database.Migrate();

                // seed the data
                DbInitializer.Initialize(context);
            }
            catch(Exception ex)
            {
                //log error messages
                logger.LogError(ex, "An error occured during migration");
            }
            finally
            {
                // dispose of the resources been used
                scope.Dispose();
            }

            // run the server
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
