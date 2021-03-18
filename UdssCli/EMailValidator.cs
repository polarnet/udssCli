namespace UdssCli
{
  using System;
  using System.Net.Mail;

  public static class EMailValidator
  {
    /// <summary>
    /// Проверка корректности адреса электропочты. Не строим свой regex, а используем готовый класс net
    /// MailAddress использует внутри себя MailAddressParser
    /// https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/Net/Mail/MailAddressParser.cs
    /// </summary>
    /// <param name="emailaddress">адрес e-mail для проверки</param>
    /// <returns></returns>
    public static bool IsValid(string emailaddress)
    {
      try
      {
        var m = new MailAddress(emailaddress);
        return true;
      }
      catch (FormatException)
      {
        return false;
      }
    }
  }
}
