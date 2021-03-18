namespace UdssCli.Infrastructure
{
  using System;
  using System.Threading.Tasks;
  using Microsoft.AspNetCore.Mvc.ModelBinding;

  public class StringTrimModelBinder : IModelBinder
  {
    private readonly IModelBinder m_fallbackBinder;

    public StringTrimModelBinder(IModelBinder fallbackBinder)
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

      string valueAttempted = value.FirstValue;
      if (string.IsNullOrWhiteSpace(valueAttempted))
      {
        bindingContext.Result = ModelBindingResult.Success(null);
        return Task.CompletedTask;
      }

      // устанавливаем результат привязки
      bindingContext.Result = ModelBindingResult.Success(valueAttempted.Trim());
      return Task.CompletedTask;
    }
  }
}