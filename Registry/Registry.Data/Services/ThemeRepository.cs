using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;

namespace Registry.Data.Services
{
  public class ThemeRepository : BaseRepository, IThemeRepository
  {
    public async Task<GetAllThemesResult[]> GetAllThemes()
    {
      IEnumerable<GetAllThemesResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetAllThemesResult>(
          StoredProcedures.GetAllThemes,
          commandType: CommandType.StoredProcedure);
      }

      if (!result.Any())
      {
        return new GetAllThemesResult[0];
      }

      return result.ToArray();
    }

    public async Task UpdateTheme(Guid id, string name, string ownerLogin)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);
      parameters.Add("@name", name);
      parameters.Add("@leader_login", ownerLogin);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.UpdateTheme,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task CreateTheme(string ownerLogin, string name)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@name", name);
      parameters.Add("@leader_login", ownerLogin);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.CreateTheme,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task DeleteTheme(Guid id)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.DeleteTheme,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }
  }
}