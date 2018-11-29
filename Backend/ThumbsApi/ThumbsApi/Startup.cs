using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using ThumbsApi.Contexts;
using ThumbsApi.Services;
using ThumbsApi.Services.Interfaces;

namespace ThumbsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(opts =>
                    opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection") +
                            Environment.GetEnvironmentVariable("Connection", EnvironmentVariableTarget.Machine)));

            services.AddScoped<IThumbsRepository, ThumbsRepository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = true;
                options.AutomaticAuthentication = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Thumbs",
                    Description = "Thumbs API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "David Kinghan",
                        Email = string.Empty,
                        Url = "to be confirmed"
                    },
                    License = new License
                    {
                        Name = "to be confirmed",
                        Url = "comming soon"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                // c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
                //c.RoutePrefix = string.Empty;
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
