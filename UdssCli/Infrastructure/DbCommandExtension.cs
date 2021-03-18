namespace UdssCli.Infrastructure
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Data.SqlTypes;
  using Microsoft.Data.SqlClient;

  public static class DbCommandExtension
  {
    public static IDbDataParameter AddErrorMessageParam(this IDbCommand cmd)
    {
      const string prmName = "@ErrorMessage";
      const int prmSize = 512;

      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      var idx = cmd.Parameters.IndexOf(prmName);
      if (idx >= 0)
      {
        return (IDbDataParameter)cmd.Parameters[idx];
      }

      var prm = cmd.CreateParameter();
      prm.ParameterName = prmName;
      prm.DbType = DbType.String;
      prm.Size = prmSize;
      prm.Direction = ParameterDirection.InputOutput;
      prm.Value = SqlString.Null;
      cmd.Parameters.Add(prm);
      return prm;
    }

    public static IDbDataParameter AddRetvalParam(this IDbCommand cmd)
    {
      const string prmName = "@RETURN_VALUE";

      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      var idx = cmd.Parameters.IndexOf(prmName);
      if (idx >= 0)
      {
        return (IDbDataParameter)cmd.Parameters[idx];
      }

      var prm = cmd.CreateParameter();
      prm.ParameterName = prmName;
      prm.DbType = DbType.Int32;
      prm.Direction = ParameterDirection.ReturnValue;
      cmd.Parameters.Add(prm);
      return prm;
    }

    public static IDbDataParameter AddUserIdParam(this IDbCommand cmd, Guid userId)
    {
      const string prmName = "@UserId";

      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      var idx = cmd.Parameters.IndexOf(prmName);
      if (idx >= 0)
      {
        return (IDbDataParameter)cmd.Parameters[idx];
      }

      var prm = cmd.CreateParameter();
      prm.ParameterName = prmName;
      prm.DbType = DbType.Guid;
      prm.Direction = ParameterDirection.Input;
      cmd.Parameters.Add(prm);
      prm.Value = userId;
      return prm;
    }

    public static IDbDataParameter AddSetOfGuid(this SqlCommand cmd, string prmName, IEnumerable<Guid> list)
    {
      const string fldValueId = "ValueId";

      if (cmd == null)
      {
        throw new ArgumentNullException(nameof(cmd));
      }

      var idx = cmd.Parameters.IndexOf(prmName);
      if (idx >= 0)
      {
        return cmd.Parameters[idx];
      }

      // загрузка в табличный параметр
      using var dt = new DataTable();
      dt.Columns.Add(fldValueId, typeof(SqlGuid));
      foreach (var itemId in list)
      {
        dt.Rows.Add(itemId);
      }

      var prm = cmd.CreateParameter();
      prm.ParameterName = prmName;
      prm.SqlDbType = SqlDbType.Structured;
      prm.Direction = ParameterDirection.Input;
      prm.Value = dt;
      cmd.Parameters.Add(prm);
      return prm;
    }
  }
}