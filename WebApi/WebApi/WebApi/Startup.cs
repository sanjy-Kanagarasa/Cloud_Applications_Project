using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;
using AutoMapper;
using WebApi.Models.Repositories;
using WebApi.Hubs;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Injection for Automapper
            services.AddAutoMapper();

            //Inject UserRepo interface
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // Database connection
            services.AddDbContext<UserContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<OrderContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddDbContext<ReviewContext>(options =>
            //options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            // Cross origin 
            services.AddCors(o => o.AddPolicy("AllowClient", builder =>
            {
                builder.AllowAnyOrigin()//WithOrigins("http://192.168.0.172:9000", "http://192.168.0.172:80") //
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();
            services.AddMvcCore().AddDataAnnotations().AddJsonFormatters();

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "CA2-UBER API's", Version = "v1" });
            });

            
            // Require all requests to use https, http gets ignored here, see redirect below
            //            Run the following command to create a certificate
            //            cd C:\Program Files(x86)\IIS Express
            //IisExpressAdminCmd.exe setupsslUrl -url:urlToYourSite - UseSelfSigned
            //services.Configure<MvcOptions>(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowClient");
            

            app.UseSignalR(routes =>
            {
                routes.MapHub<OrderHub>("orderhub");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            app.UseMvcWithDefaultRoute();

            // Redirect http to https
            //app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
        }
    }
}
