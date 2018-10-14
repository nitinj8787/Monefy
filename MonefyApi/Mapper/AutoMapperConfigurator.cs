using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MonefyApi.Mapper
{
    public static class AutoMapperConfigurator
    {
        private static readonly object _lock = new object();
        private static MapperConfiguration _mapperConfiguration;

        public static MapperConfiguration Configure()
        {
            lock(_lock)
            {
                if (_mapperConfiguration != null) return _mapperConfiguration;

                var thisType = typeof(AutoMapperConfigurator);

                var configInterfaceType = typeof(IAutoMapperTypeConfigurator);

                var configurators = thisType.GetTypeInfo().Assembly.GetTypes()
                    .Where(x => !string.IsNullOrWhiteSpace(x.Namespace))
                    .Where(x => x.Namespace.Contains(thisType.Namespace))
                    .Where(x => x.GetTypeInfo().GetInterface(configInterfaceType.Name) != null)
                    .Select(x => (IAutoMapperTypeConfigurator)Activator.CreateInstance(x))
                    .ToArray();

                void AggregatedConfigurator(IMapperConfigurationExpression config)
                {
                    foreach (var configurator in configurators)
                    {
                        configurator.Configure(config);
                    }
                }

                _mapperConfiguration = new MapperConfiguration(AggregatedConfigurator);
                return _mapperConfiguration;
            }
        }

    }
}
