using Dapper;
using UdssCli.Infrastructure;

namespace UdssCli
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlTypes;
  using Microsoft.Data.SqlClient;

  public static class DbHelper
  {
    public const string DbName = "UdssCliDb";
    public const string DbNameIdentity = "UdssCliDbUsers";
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

    public static bool ContactsEMailRegister(
      string authorName,
      string authorEmail,
      string emailSubject,
      string emailMessage,
      out string errorMessage)
    {
      const string cmdText = "cli.ContactsEmailAdd";

      var db = DbConnection();
      var cmd = db.CreateCommand();
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.CommandText = cmdText;
      cmd.Parameters.AddWithValue("@AuthorName", authorName);
      cmd.Parameters.AddWithValue("AuthorEmail", authorEmail);
      cmd.Parameters.AddWithValue("EmailSubject", emailSubject);
      cmd.Parameters.AddWithValue("EmailMessage", emailMessage);
      var prmErrMsg = cmd.AddErrorMessageParam();
      var prmRc = cmd.AddRetvalParam();
      db.Open();
      cmd.ExecuteNonQuery();
      var rc = (int)prmRc.Value;
      if (rc != 0)
      {
        var errMsg = (SqlString)prmErrMsg.Value;
        errorMessage = $"Send e-mail error: {(!errMsg.IsNull ? errMsg.Value : string.Empty)}";
        return false;
      }

      errorMessage = null;
      return true;
    }
  }
}