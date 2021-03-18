using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace UdssCli.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.Rendering;
  using UdssCli.Models;

  public class DataHelperController : Controller
  {
    private readonly IDataHelperRepo m_repo;
    private readonly Guid m_userId;

    public DataHelperController(IHttpContextAccessor httpContextAccessor, IDataHelperRepo repo)
    {
      if (httpContextAccessor == null)
      {
        throw new ArgumentNullException(nameof(httpContextAccessor));
      }

      //m_userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
      m_repo = repo;
    }

    public JsonResult BrandList()
    {
      var list = m_repo.BrandList();
      var data = list.Select(f => new { f.BrandName, BrandId = f.BrandId.ToString() });
      return Json(data);
    }

    public JsonResult EquipmentTypesList4Brand(int? brandId)
    {
      var list = m_repo.EquipmentList(brandId);
      var data = list.Select(f => new SelectListItem { Text = f.EquipmentTypeName, Value = f.EquipmentTypeId.ToString() });
      return Json(data);
    }

    public JsonResult CountriesList(string countryCode)
    {
      var list = m_repo.CountriesList();
      var data = list.Select(f => new SelectListItem { Text = f.CountryName, Value = f.CountryCode });
      return Json(data);
    }

    public JsonResult SerialDataSingle(string serial)
    {
      var data = m_repo.SerialDataSingle(serial);
      if (data != null)
      {
        data.Error = 0;
        data.ErrorMessage = string.Empty;
      }
      else
      {
        data = new SerialData
        {
          Error = 1,
          ErrorMessage = $"Serial number {serial} not found.",
          BrandName = string.Empty,
          Model = string.Empty,
          EquipmentTypeName = string.Empty,
          EquipmentSubtypeName = string.Empty,
        };
      }

      return Json(data);
    }

    public IActionResult SerialDataMulty([DataSourceRequest] DataSourceRequest request, string serial)
    {
      var data = m_repo.SerialDataMulty(serial);
      return Json(data.ToDataSourceResult(request));
    }

    public IActionResult SerialQtyGet(string serial)
    {
      var qty = m_repo.SerialQtyGet(serial);
      return Json(new { qty });
    }
  }
}
