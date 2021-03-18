namespace UdssCli
{
  using System;

  public static class Defines
  {
    public const string ReportCompanyName = "Union Dsitribution";
    public const string ReportAuthorName = "UDSS web system";

    // путь к папке с файлами, относительно wwwroot
    public const string PathStorageFiles = "storage\\files";

    // кол-во лет глубины для просмотра инвойсов
    public const int InvoicePeriodYear = 5;

    // маска номера телефона. Проверка  делается средствами jquery.maskedinput
    // https://github.com/digitalBush/jquery.maskedinput
    // номер телефона должен состоять только из цифр и иметь длину от 10 до 16 цифр
    public const string PhoneNumberRegex = @"^\d{10,16}$";
    public const string PhoneNumberMaskJs = @"0000000000999999";
    public const string PhoneNumberTooltip = @"Phone number must be just digits and be from 10 to 16 digits length";

    // разделитель для списков в виде строки
    public const char ListSep = ';';

    public const int SRID = 4326;

    // идентификатор, используемый как признак создания нового пользователя
    public static readonly Guid UserIdNew = new Guid("69FF80F3-BD16-4713-9D63-9A0ADFED83B1");

    // идентификатор нового адреса
    public static readonly Guid AddressIdNew = new Guid("6DBC942B-D57C-4D12-9DB9-0A2D6E5AC6BE");

    public const int LCID_Polish = 21;
    public const string ShortDatePattern_Polish = "dd.MM.yyyy";

    public static string UiCulture()
    {
      var cultureName = System.Globalization.CultureInfo.CurrentCulture.ToString();
      return cultureName;
    }
  }
}
