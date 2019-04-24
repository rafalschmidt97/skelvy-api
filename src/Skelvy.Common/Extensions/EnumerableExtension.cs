using System;
using System.Collections.Generic;

namespace Skelvy.Common.Extensions
{
  public static class EnumerableExtension
  {
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
      if (list == null)
      {
        throw new ArgumentNullException(nameof(list));
      }

      if (action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      foreach (var t in list)
      {
        action(t);
      }
    }
  }
}
