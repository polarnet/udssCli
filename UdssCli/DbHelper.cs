namespace UdssCli
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlTypes;
  using Microsoft.Data.SqlClient;

  public static class DbHelper
  {
    public const string DbName = "udssClientDb";
    public const string SetOfGuidTypeName = "dbo.SetOfGuid";

    private static string m_connStr;

    static DbHelper()
    {
      /*
      var configurationBuilder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
      var configurationRoot = configurationBuilder.Build();
      m_connStr = configurationRoot.GetConnectionString("DefaultConnection");
      */
    }

    public static void SetConnectionString(string connStr)
    {
      m_connStr = connStr;
    }

    public static string ConnectionString()
    {
      return m_connStr;
    }

    public static SqlConnection DbConnection()
    {
      string cs = ConnectionString();
      return !string.IsNullOrEmpty(cs) ? new SqlConnection(cs) : null;
    }

    public static DataTable SetOfGuid(IEnumerable<Guid> data)
    {
      const string fldValueId = "ValueId";

      var dt = new DataTable();
      dt.Columns.Add(fldValueId, typeof(SqlGuid));

      if (data == null)
      {
        return dt;
      }

      foreach (var item in data)
      {
        dt.Rows.Add(item);
      }

      return dt;
    }
  }
}