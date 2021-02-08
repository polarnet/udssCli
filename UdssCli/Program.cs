namespace UdssCli
{
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Serilog;
  using System;

  public class Program
  {
    public static void Main(string[] args)
    {
      // инициализация логгера вынесена в Startup
      try
      {
        var builder = CreateHostBuilder(args);
        builder.Build().Run();
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
      {
        Log.Error(e, "catch exception in main");
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureLogging((hostingContext, config) => { config.ClearProviders(); })
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
        .UseSerilog();
  }
}
