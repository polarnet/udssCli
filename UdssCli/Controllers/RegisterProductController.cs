using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using UdssCli.Models;
using Westwind.Globalization;

namespace UdssCli.Controllers
{
  using Microsoft.AspNetCore.Http;
  using Microsoft.AspNetCore.Mvc;

  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  [Authorize(Roles = "User")]
  public class RegisterProductController : Controller
  {
    private const string ResourceSet = "cli_RegisterProduct";
    private const string ViewRegisterProduct = "~/Views/Clients/RegisterProduct.cshtml";

    public IActionResult RegisterProduct()
    {
      ViewBag.ServerError = false;
      return View(ViewRegisterProduct);
    }

    [HttpPost]
    public IActionResult RegisterProduct_Submit(RegisterProductData data, List<IFormFile> files)
    {
      if (!ModelState.IsValid)
      {
        return View(ViewRegisterProduct, data);
      }

      if (data == null)
      {
        ModelState.AddModelError(string.Empty, @DbRes.T("DeviceDataNotFound_Msg", ResourceSet));
        return View(ViewRegisterProduct);
      }



      return Ok();
    }

    [HttpPost]
    public JsonResult Products_Read([DataSourceRequest] DataSourceRequest request, string serialNo)
    {
      //var news = m_repo.GetArticles4User(m_userId);
      //var result = news.ToDataSourceResult(request);
      var result = new RegisterProductDeviceInfo()
      {
        BrandId = 1,
        BrandName = "toshiba",
        EquipmentId = 1,
        EquipmentTypeName = "tv",
        Model = "TV32",
      };

      var lst = new List<RegisterProductDeviceInfo>();
      lst.Add(result);

      var r1 = lst.ToDataSourceResult(request);
      return Json(r1);
    }
  }
}