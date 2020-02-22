using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Skelvy.Common.Extensions;

namespace Skelvy.Application.Core.Mappers
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
      var types = assembly.GetExportedTypes()
        .Where(t => t.GetInterfaces().Any(i =>
          i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapping<>)))
        .ToList();

      foreach (var type in types)
      {
        var instance = Activator.CreateInstance(type);
        var methodInfo = type.GetMethod("Mapping");

        if (methodInfo == null)
        {
          type.GetInterfaces()
            .Where(x => x.Name.Contains("IMapping", StringComparison.Ordinal))
            .ForEach(interfaceInstance =>
            {
              var interfaceMethodName = interfaceInstance.GetMethod("Mapping");
              interfaceMethodName?.Invoke(instance, new object[] { this });
            });
        }
        else
        {
          methodInfo.Invoke(instance, new object[] { this });
        }
      }
    }
  }
}
