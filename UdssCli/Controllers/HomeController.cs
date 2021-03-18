using Ganss.XSS;

namespace UdssCli.Controllers
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.Extensions.Logging;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using System.Threading.Tasks;
  using UdssCli.Models;
  using Westwind.Globalization;

  public class HomeController : Controller
  {
    private const string ResourceSet = "cli:Home/Contacts";
    private const string ViewNameSuccess = "~/Views/Home/Success.cshtml";

    private readonly ILogger<HomeController> m_logger;

    public HomeController(ILogger<HomeController> logger)
    {
      m_logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult AboutUs()
    {
      return View();
    }

    // отправка формы клиенту
    [HttpGet]
    public IActionResult Contacts()
    {
      return View();
    }

    /// <summary>
    ///  получение заполненной формы от клиента
    /// </summary>
    /// <param name="name">имя</param>
    /// <param name="email">адрес электропочты</param>
    /// <param name="subject">тема</param>
    /// <param name="message">сообщение</param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Contacts_Submit(
      string name,
      string email,
      string subject,
      string message)
    {
      // санитайзер
      var sanitizer = new HtmlSanitizer();
      var nameSanitized = sanitizer.Sanitize(name);
      var emailSanitized = sanitizer.Sanitize(email);
      var subjectSanitized = sanitizer.Sanitize(subject);
      var messageSanitized = sanitizer.Sanitize(message);

      if (string.IsNullOrWhiteSpace(nameSanitized))
      {
        var msg = @DbRes.T("CustomerName_Text", ResourceSet);
        return Content(msg);
      }

      if (string.IsNullOrWhiteSpace(emailSanitized))
      {
        var msg = @DbRes.T("CustomerEmail_Hint", ResourceSet);
        return Content(msg);
      }

      if (!EMailValidator.IsValid(emailSanitized))
      {
        var msg = @DbRes.T("CustomerEmail_Invalid", ResourceSet);
        return Content(msg);
      }

      if (string.IsNullOrWhiteSpace(subjectSanitized))
      {
        var msg = @DbRes.T("Subject_Hint", ResourceSet);
        return Content(msg);
      }

      if (string.IsNullOrWhiteSpace(messageSanitized))
      {
        var msg = @DbRes.T("Message_Text", ResourceSet);
        return Content(msg);
      }

      var rc = DbHelper.ContactsEMailRegister(nameSanitized, emailSanitized, subjectSanitized, messageSanitized, out string errMsg);
      if (!rc)
      {
        return Content(errMsg);
      }

      // такого ответа требует скрипт авторов шаблона, см. логику в assets\vendor\php-email-form\validate.js
      return Content("OK");
    }

    public IActionResult Success()
    {
      return View(ViewNameSuccess);
    }
  }
}
