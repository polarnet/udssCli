using System.Security.Claims;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using UdssCli.Models;

namespace UdssCli.Controllers
{
  using Microsoft.AspNetCore.Mvc;
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public class ServicesController : Controller
  {
    private const string ViewNameIndex = "~/Views/Services/Index.cshtml";

    private readonly IServicesRepo m_repo;
    private readonly Guid m_userId;

    public ServicesController(IHttpContextAccessor httpContextAccessor, IServicesRepo repo)
    {
      if (httpContextAccessor == null)
      {
        throw new ArgumentNullException(nameof(httpContextAccessor));
      }

      //m_userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
      m_repo = repo;
    }

    public IActionResult Index()
    {
      return View(ViewNameIndex);
    }

    public JsonResult ServiceListRead([DataSourceRequest] DataSourceRequest request, string countryCode)
    {
      var list = m_repo.ServicesList(countryCode);
      var ds = list.ToDataSourceResult(request);
      return Json(ds);
    }
  }
}
