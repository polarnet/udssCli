namespace UdssCli
{
  using System;
  using Microsoft.Data.SqlClient;

  public static class SqlParameterCollectionExtension
  {
    public static SqlParameter AddWithNullableValue(this SqlParameterCollection collection, string parameterName, object value)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      return collection.AddWithValue(parameterName, value ?? DBNull.Value);
    }

    /*
    public static SqlParameter AddGeographyNullableValue(this SqlParameterCollection collection, string parameterName, SqlGeography value)
    {
      if (collection == null)
      {
        throw new ArgumentNullException(nameof(collection));
      }

      SqlParameter p = new SqlParameter();
      p.ParameterName = parameterName;
      p.SqlDbType = SqlDbType.Udt;
      p.UdtTypeName = "geography";
      if (value != null)
      {
        p.Value = value;
      }
      else
      {
        p.Value = DBNull.Value;
      }

      return collection.Add(p);
    }
    */
  }
}