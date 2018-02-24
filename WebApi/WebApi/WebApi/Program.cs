using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Data;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var UserContext = services.GetRequiredService<UserContext>();
                    UserDbInitializer.Initialize(UserContext);

                    var OrderContext = services.GetRequiredService<OrderContext>();
                    OrderDbInitializer.Initialize(OrderContext, UserContext);

                    //var ReviewContext = services.GetRequiredService<ReviewContext>();
                    //ReviewDbInitializer.Initialize(ReviewContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                
                .UseStartup<Startup>()
                //.UseUrls("http://*:9000")
                .UseUrls("http://*:8000")
                //.UseUrls("https://*:9000")
                .Build();
    }
}