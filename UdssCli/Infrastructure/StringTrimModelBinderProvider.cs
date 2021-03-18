// <copyright file="StringTrimModelBinderProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace UdssCli.Infrastructure
{
  using System;
  using Microsoft.AspNetCore.Mvc.ModelBinding;
  using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Logging;

  public class StringTrimModelBinderProvider : IModelBinderProvider
  {
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
      // Для объекта SimpleTypeModelBinder необходим сервис ILoggerFactory, получаем его из сервисов
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      if (context.Metadata.ModelType != typeof(string))
      {
        return null;
      }

      var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
      IModelBinder binder = new StringTrimModelBinder(new SimpleTypeModelBinder(typeof(string), loggerFactory));
      return binder;
    }
  }
}
