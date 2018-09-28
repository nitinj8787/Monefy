using Monefy.Data.Access.Interfaces;
using Monefy.Data.Access.Maps.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Monefy.Data.Access.Maps
{
    public static class MappingsHelper
    {
        public static IEnumerable<IMap> GetMappings()
        {
            var assemblyTypes = typeof(UserMap).GetTypeInfo().Assembly.DefinedTypes;

            var mappings = assemblyTypes
                            .Where(t => t.Namespace != null && t.Namespace.Contains(typeof(UserMap).Namespace))
                            .Where(t => typeof(IMap).GetTypeInfo().IsAssignableFrom(t));

            mappings = mappings.Where(x => !x.IsAbstract);

            return mappings.Select(m => (IMap)Activator.CreateInstance(m.AsType())).ToArray();
        }
    }
}
