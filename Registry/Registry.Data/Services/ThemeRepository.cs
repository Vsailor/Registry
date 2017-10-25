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

    public async Task UpdateTheme(Guid id, string name)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);
      parameters.Add("@name", name);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.UpdateTheme,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task CreateTheme(string name, CreateThemeUserRequest[] createThemeUserRequest)
    {
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        var createThemeParam = new DynamicParameters();
        createThemeParam.Add("@name", name);
        var dt = new DataTable();
        dt.Columns.Add("UserLogin", typeof (string));
        dt.Columns.Add("Role", typeof (int));

        foreach (var item in createThemeUserRequest)
        {
          dt.Rows.Add(item.UserLogin, item.Role);
        }

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        SqlParameter dtparam = cmd.Parameters.AddWithValue("@request", dt);
        dtparam.SqlDbType = SqlDbType.Structured;

        createThemeParam.Add("@");

        await conn.ExecuteAsync(
          StoredProcedures.CreateTheme,
          createThemeParam,
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

    public async Task<string[]> GetThemes(string login)
    {
      IEnumerable<string> result;
      var parameters = new DynamicParameters();
      parameters.Add("@login", login);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<string>(
          StoredProcedures.GetUserThemes,
          parameters,
          commandType: CommandType.StoredProcedure);
      }

      if (!result.Any())
      {
        return new string[0];
      }

      return result.ToArray();
    }
  }
}