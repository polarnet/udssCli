﻿namespace UdssCli.Infrastructure
{
  using System;
  using System.Globalization;
  using System.Threading;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc.ModelBinding;

  public class CustomDateTimeModelBinder : IModelBinder
  {
    private readonly IModelBinder m_fallbackBinder;

    public CustomDateTimeModelBinder(IModelBinder fallbackBinder)
    {
      m_fallbackBinder = fallbackBinder;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
      // в случае ошибки возвращаем исключение
      if (bindingContext == null)
      {
        throw new ArgumentNullException(nameof(bindingContext));
      }

      // с помощью поставщика значений получаем данные из запроса
      ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

      // если не найдено значений с данными ключами, вызываем привязчик модели по умолчанию
      if (value == ValueProviderResult.None)
      {
        return m_fallbackBinder.BindModelAsync(bindingContext);
      }

      // получаем значения
      string testValue = value.FirstValue;

      // 2021-02-08 olegk: коррекция для Polska. В Польше используется формат ДД.ММ.ГГГГ в повседневном общении
      // и формат ГГГГ-ММ-ДД электронном обороте. Net для польской культуры ожидает ГГГГ-ММ-ДД, а из Telerik приходит ДД.ММ.ГГГГ
      // поэтому делаем изменение ShortDatePattern для польской культуры
      CultureInfo culture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.LCID);
      if (culture.LCID == Defines.LCID_Polska)
      {
        culture.DateTimeFormat.ShortDatePattern = Defines.ShortDatePattern_Polska;
      }

      bool isDate = DateTime.TryParse(testValue, culture, DateTimeStyles.None, out DateTime result);
      if (!isDate)
      {
        bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid date/time value");
        return Task.CompletedTask;
      }

      // устанавливаем результат привязки
      bindingContext.Result = ModelBindingResult.Success(result);
      return Task.CompletedTask;
    }
  }
}