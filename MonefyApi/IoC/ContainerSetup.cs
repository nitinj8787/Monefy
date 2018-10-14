using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monefy.Data.Access.DAL;
using Monefy.Data.Access.Interfaces;
using Monefy.Queries.Interfaces;
using Monefy.Queries.Queries;
using Monefy.Security.Auth;
using MonefyApi.Filters;
using MonefyApi.Helpers.ActionTransaction;
using MonefyApi.Mapper;
using MonefyApi.Security;
using Monify.Security;

namespace MonefyApi.IoC
{
    public static class ContainerSetup
    {
        public static void Setup(IServiceCollection services, IConfigurationRoot configuration)
        {
            AddUow(services, configuration);
            AddQueries(services);
            ConfigureAutoMapper(services);
            ConfigureAuth(services);
        }

        private static void ConfigureAuth(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();
            services.AddScoped<ISecurityContext, SecurityContext>();
        }

        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            var mapperConfig = AutoMapperConfigurator.Configure();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(m => mapper);
            services.AddTransient<IAutoMapper, AutoMapperAdapter>();
        }

        private static void AddQueries(IServiceCollection services)
        {
            //services.AddScoped<IExpenseQueryProcessor, ExpenseQueryProcessor>();

            var queryProcesserType = typeof(UsersQueryProcessor);

            var types = queryProcesserType.GetTypeInfo().Assembly.GetTypes()
                                            .Where(x => x.Namespace == queryProcesserType.Namespace
                                                    && x.GetTypeInfo().IsClass
                                                    && x.GetTypeInfo().GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                                            .ToArray();

            foreach (var type in types)
            {
                var interfaceQ = type.GetTypeInfo().GetInterfaces().First();

                services.AddScoped(interfaceQ, type);
            }
        }

        private static void AddUow(IServiceCollection services, IConfigurationRoot configuration)
        {
            var connectionString = configuration["Data:main"];

            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork>(ctx => new EFUnitOfWork(ctx.GetRequiredService<MainDbContext>()));

            services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();

            services.AddScoped<UnitOfWorkFilterAttribute>();

        }
    }
}
