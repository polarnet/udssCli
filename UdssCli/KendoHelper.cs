namespace UdssCli
{
  public static class KendoHelper
  {
    // общие параметры для сеток KENDO
    public const int GridButtonCount = 5;
    public const int GridPageSize = 20;

    // именования контролов для метода data
    public const string DataGrid = "kendoGrid";
    public const string DataWindow = "kendoWindow";
    public const string DropDownList = "kendoDropDownList";
    public const string ListView = "kendoListView";
    public const string MultiColumnComboBox = "kendoMultiColumnComboBox";
    public const string FileUpload = "kendoUpload";
    public const string TextBox = "kendoTextBox";

    // режимы select для смены на клиенте
    public const string SelectableMultipleRow = "Multiple, Row";
    public const string SelectableSingleCell = "Cell";

    public const string SelectableMultipleRowText = "To cell select";
    public const string SelectableSingleCellText = "To rows select";
  }
}
