using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Monefy.Data.Access.DAL;
using Monefy.Security.Auth;
using MonefyApi.Filters;
using MonefyApi.IoC;
using Swashbuckle.AspNetCore.Swagger;

namespace MonefyApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings{env.EnvironmentName}.json", true)
                            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ContainerSetup.Setup(services, Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (o) =>
                                        {
                                            o.TokenValidationParameters = new TokenValidationParameters()
                                            {
                                                ValidAudience = TokenAuthOption.Audience,
                                                IssuerSigningKey = TokenAuthOption.key,
                                                ValidIssuer = TokenAuthOption.Issuer,
                                                ValidateLifetime = true,
                                                ValidateAudience = true,
                                                ValidateIssuerSigningKey = true,
                                                ValidateIssuer = true,
                                                ClockSkew = TimeSpan.FromMinutes(0)
                                            };
                                        });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            //services.AddMvc();
            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(new ApiExceptionFilter());
            })
            .AddJsonOptions(o =>
            {
                o.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });

            services.AddNodeServices();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Monefy", Version = "v1" });
                c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
                
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true
                //});

                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(s => s.SwaggerEndpoint("swagger/v1/swagger.json", "My API V1"));

            }


            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            InitDatabase(app);

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();

            app.UseExceptionHandler();


        }

        private void InitDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MainDbContext>();
                context.Database.Migrate();
            }
        }
    }
}
