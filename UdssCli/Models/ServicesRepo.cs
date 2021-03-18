using System.Data;
using Dapper;

namespace UdssCli.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading.Tasks;

  public interface IServicesRepo
  {
    IEnumerable<ServiceData> ServicesList(string countryCode);
  }

  public class ServicesRepo : IServicesRepo
  {
    private readonly string m_connStr;

    public ServicesRepo(string connStr)
    {
      m_connStr = connStr;
    }

    public IEnumerable<ServiceData> ServicesList(string countryCode)
    {
      const string cmdName = "cli.ServicesList";
      using var db = DbHelper.DbConnection();
      var list = db.Query<ServiceData>(cmdName, new {CountryCode = countryCode}, commandType: CommandType.StoredProcedure);
      return list;
    }
  }
}
