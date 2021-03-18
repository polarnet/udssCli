using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using UdssCli.Models;

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
    private const string viewRegisterProduct = "~/Views/Clients/RegisterProduct.cshtml";

    public IActionResult RegisterProduct()
    {
      ViewBag.ServerError = false;
      return View(viewRegisterProduct);
    }

    [HttpPost]
    public IActionResult RegisterProduct_Submit(RegisterProductData data, List<IFormFile> files)
    {
      if (!ModelState.IsValid)
      {
        ViewBag.ServerError = true;
        return View(viewRegisterProduct, data);
      }

      if (data == null)
      {
        ViewBag.ServerError = true;
        ModelState.AddModelError(string.Empty, "Register data not found");
        return View(viewRegisterProduct);
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