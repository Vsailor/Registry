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
  public class ResourceGroupRepository : BaseRepository, IResourceGroupRepository
  {
    public async Task<GetAllGroupsResult[]> GetAllResourceGroups()
    {
      IEnumerable<GetAllGroupsResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetAllGroupsResult>(
          StoredProcedures.GetAllThemes,
          commandType: CommandType.StoredProcedure);
      }

      if (!result.Any())
      {
        return new GetAllGroupsResult[0];
      }

      return result.ToArray();
    }

    public async Task UpdateResourceGroup(Guid id, string name)
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

    public async Task CreateGroup(string name, CreateGroupResourceRequest[] createGroupResourceRequest)
    {
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        var createThemeParam = new DynamicParameters();
        createThemeParam.Add("@name", name);
        var dt = new DataTable();
        dt.Columns.Add("UserLogin", typeof (string));
        dt.Columns.Add("Role", typeof (int));

        foreach (var item in createGroupResourceRequest)
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

    public async Task DeleteGroup(Guid id)
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

    public async Task<string[]> GetGroups(string login)
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