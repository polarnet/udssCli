using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UdssCli.Infrastructure
{
  public class CustomNullableDateTimeModelBinderProvider : IModelBinderProvider
  {
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      // Для объекта SimpleTypeModelBinder необходим сервис ILoggerFactory, получаем его из сервисов
      var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
      IModelBinder binder = new CustomNullableDateTimeModelBinder(new SimpleTypeModelBinder(typeof(DateTime?), loggerFactory));
      return context.Metadata.ModelType == typeof(DateTime?) ? binder : null;
    }
  }
}
