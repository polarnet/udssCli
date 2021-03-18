using UdssCli.Areas.Identity.Data;
using UdssCli.Data;
using UdssCli.Models;

namespace UdssCli
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.HttpsPolicy;
  using Microsoft.AspNetCore.Localization;
  using Microsoft.AspNetCore.Authentication.Cookies;
  using Microsoft.AspNetCore.Mvc.Localization;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Localization;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using System.Threading.Tasks;
  using Newtonsoft.Json.Serialization;
  using Westwind.Globalization;
  using Westwind.Globalization.AspnetCore;
  using Microsoft.Extensions.Configuration;
  using Serilog;
  using System.Diagnostics;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.AspNetCore.Http;
  using UdssCli.Infrastructure;

  public class Startup
  {
    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
      this.Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{webHostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)

        // ������������ ��� Serilog, � ��������� �����, ����� �� ��������
        .AddJsonFile("appsettings.logs.json", optional: true)
        .Build();

      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(this.Configuration)
        .CreateLogger();

      // ��� �������� ������ Serilog
      Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var connStr = this.Configuration.GetConnectionString(DbHelper.DbName);
      var connStrIdentity = this.Configuration.GetConnectionString(DbHelper.DbNameIdentity);
      DbHelper.SetConnectionString(connStr);

      // ��������� NewtonSoft JSON formatter �������� � ���������� �����, � �������������� �� ��������� ������� �����������
      // ������� ��������� ������������� ����������� �����/������
      // https://stackoverflow.com/questions/47735133/asp-net-core-synchronous-operations-are-disallowed-call-writeasync-or-set-all
      services.Configure<IISServerOptions>(options =>
      {
        options.AllowSynchronousIO = true;
      });

      services.AddDbContext<UdssCliDbContext>(options => options.UseSqlServer(connStrIdentity));

      services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = true; })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UdssCliDbContext>();

      // ��� DI � �����������
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      // ����������� Identity
      services.Configure<IdentityOptions>(options =>
      {
        // ����������� �������� ���������� ������� ��� ������� � ������
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 3;
        options.Lockout.AllowedForNewUsers = true;

        // ���������� � �������
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.Password.RequiredUniqueChars = 5;

        // ���������� ������������� ��� ������ ������������
        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;

        // ���������� � ������
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789- ._@";
        options.User.RequireUniqueEmail = true;
      });

      // ��������� ������� ������������
      services.ConfigureApplicationCookie(options =>
      {
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.Cookie.Name = "udssTrackingApp";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.LoginPath = "/Identity/Account/Login";
        options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
        options.SlidingExpiration = true;
      });

      // Standard ASP.NET Localization features are recommended
      // Make sure this is done FIRST!
      services.AddLocalization(options =>
      {
        options.ResourcesPath = "Resources";
      });

      // Replace StringLocalizers with Db Resource Implementation
      services.AddSingleton(typeof(IStringLocalizerFactory), typeof(DbResStringLocalizerFactory));
      services.AddSingleton(typeof(IHtmlLocalizerFactory), typeof(DbResHtmlLocalizerFactory));

      // Required: Enable Westwind.Globalization (opt parm is optional)
      // shown here with optional manual configuration code
      services.AddWestwindGlobalization();

      services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
          options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });

      services.AddMvc()
        // required for the Administration interface for dynamic serialization
        .AddNewtonsoftJson(opt =>
        {
          opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
        })
        // for MVC/RazorPages localization
        .AddViewLocalization()
        // for ViewModel Error Annotation Localization
        .AddDataAnnotationsLocalization();

      // this *has to go here* after view localization has been initialized
      // so that Pages can localize - note required even if you're not using
      // the DbResource manager.
      services.AddTransient<IViewLocalizer, DbResViewLocalizer>();

      // ������� ������ ��� ��������������� �������� ���������� � ���������� �������� � ��������� �����
      // ��������� ������� ��� ��������� �������� �������� ����/�������
      services.AddControllersWithViews(opts =>
      {
        opts.ModelBinderProviders.Insert(0, new CustomDateTimeModelBinderProvider());
        opts.ModelBinderProviders.Insert(0, new CustomNullableDateTimeModelBinderProvider());
        opts.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider());
      });

      services.AddRazorPages();
      services.AddKendo();

      // ����������� ���������� ��� DI
      services.AddTransient<IDataHelperRepo, DataHelperRepo>(provider => new DataHelperRepo(connStr));
      services.AddTransient<IServicesRepo, ServicesRepo>(provider => new ServicesRepo(connStr));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // �������� ������. ��� ������������ (�� ��������� �������) ���������� ��� � production
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      // ��������� ������ HTTP
      // ������� �������, ����� ������ � �����
      // app.UseStatusCodePages(HttpDefines.ContentText, "Error. Status code : {0}");
      app.UseStatusCodePagesWithRedirects("~/Error?code={0}");

      // app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
      });

      app.UseMyMiddleware();

      // �������������� �����
      const string defaultCultureValue = "ru-RU";
      string[] supportedCultures =
      {
        "ru", defaultCultureValue,    // �������
        "en", "en-US",                // ����������
        "uk", "uk-UA",                // ����������
        "pl", "pl-PL",                // ��������
        "hy", "hy-AM",                // ���������
        "cs", "cs-CZ",                // �������
        "ka", "ka-GE",                // ����������
        "be", "be-BY",                // �����������
      };

      // ����������� �����������
      var localizationOptions = new RequestLocalizationOptions()
        .SetDefaultCulture(defaultCultureValue)
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);

      app.UseRequestLocalization(localizationOptions);
    }
  }
}
