namespace UdssCli.Infrastructure
{
  using Microsoft.AspNetCore.Authorization;

  // кастомный атрибут авторизации, для возможности указания прав доступа в виде списка ролей
  public class AuthorizeRolesAttribute : AuthorizeAttribute
  {
    public AuthorizeRolesAttribute(params string[] roles)
    {
      const char sep = ',';
      Roles = string.Join(sep, roles);
    }
  }
}
