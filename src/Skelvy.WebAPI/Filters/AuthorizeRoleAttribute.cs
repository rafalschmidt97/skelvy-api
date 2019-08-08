using System;
using Microsoft.AspNetCore.Authorization;

namespace Skelvy.WebAPI.Filters
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
  public sealed class AuthorizeRoleAttribute : AuthorizeAttribute
  {
    public AuthorizeRoleAttribute(params string[] roles)
    {
      Roles = string.Join(",", roles);
    }
  }
}
