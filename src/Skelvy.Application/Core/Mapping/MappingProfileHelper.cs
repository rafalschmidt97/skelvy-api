using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skelvy.Application.Core.Mapping
{
  public static class MappingProfileHelper
  {
    public static IEnumerable<ICustomMapping> LoadCustomMappings(Assembly rootAssembly)
    {
      var types = rootAssembly.GetExportedTypes();

      var mapsFrom = (
        from type in types
        from instance in type.GetInterfaces()
        where
          typeof(ICustomMapping).IsAssignableFrom(type) &&
          !type.IsAbstract &&
          !type.IsInterface
        select (ICustomMapping)Activator.CreateInstance(type)).ToList();

      return mapsFrom;
    }
  }
}
