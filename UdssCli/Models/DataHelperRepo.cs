namespace UdssCli.Models
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using Dapper;
  using Microsoft.Data.SqlClient;

  public interface IDataHelperRepo
  {
    IEnumerable<BrandData> BrandList();

    IEnumerable<EquipmentTypeData> EquipmentList(int? brandId);

    IEnumerable<CountryData> CountriesList();

    SerialData SerialDataSingle(string serial);

    IEnumerable<SerialData> SerialDataMulty(string serial);
    
    int SerialQtyGet(string serial);
  }

  public class DataHelperRepo : IDataHelperRepo
  {
    private const string PrmUserId = "@UserId";
    private const string PrmSerial = "@Serial";
    private const string PrmRC = "@RC";

    private readonly string m_connStr;

    public DataHelperRepo(string connStr)
    {
      m_connStr = connStr;
    }

    public IEnumerable<BrandData> BrandList()
    {
      const string cmdText = "cli.BrandList";
      using var db = new SqlConnection(m_connStr);
      var prm = new { UserId = Guid.Empty };
      db.Open();
      var data = db.Query<BrandData>(cmdText, prm, commandType: CommandType.StoredProcedure);
      return data;
    }

    public IEnumerable<CountryData> CountriesList()
    {
      const string cmdText = "cli.CountriesList";
      using var db = new SqlConnection(m_connStr);
      db.Open();
      var data = db.Query<CountryData>(cmdText, commandType: CommandType.StoredProcedure);
      return data;
    }

    public IEnumerable<EquipmentTypeData> EquipmentList(int? brandId)
    {
      const string cmdText = "cli.EquipmentTypesList";
      using var db = new SqlConnection(m_connStr);
      var prm = new { UserId = Guid.Empty, BrandId = brandId };
      db.Open();
      var data = db.Query<EquipmentTypeData>(cmdText, prm, commandType: CommandType.StoredProcedure);
      return data;
    }

    public SerialData SerialDataSingle(string serial)
    {
      const string cmdText = "cli.ProductSerialDataGet";
      using var db = new SqlConnection(m_connStr);
      var prm = new { Serial = serial, Multy = false };
      db.Open();
      var data = db.QuerySingleOrDefault<SerialData>(cmdText, prm, commandType: CommandType.StoredProcedure);
      return data;
    }

    public IEnumerable<SerialData> SerialDataMulty(string serial)
    {
      const string cmdText = "cli.ProductSerialDataGet";
      using var db = new SqlConnection(m_connStr);
      var prm = new { Serial = serial, Multy = true };
      db.Open();
      var data = db.Query<SerialData>(cmdText, prm, commandType: CommandType.StoredProcedure);
      return data;
    }

    public int SerialQtyGet(string serial)
    {
      const string cmdText = "cli.ProductSerialQtyGet";

      var prm = new DynamicParameters();
      prm.Add(PrmSerial, serial);
      prm.Add(PrmRC, dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
      using IDbConnection db = new SqlConnection(m_connStr);
      db.Execute(cmdText, prm, commandType: CommandType.StoredProcedure);
      var qty = prm.Get<int>(PrmRC);
      return qty;
    }
  }
}