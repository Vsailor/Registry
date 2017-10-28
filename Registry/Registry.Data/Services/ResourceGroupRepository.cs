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
          StoredProcedures.GetAllResourceGroups,
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
          StoredProcedures.UpdateResourceGroup,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task CreateGroup(string name, string login)
    {
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        var createThemeParam = new DynamicParameters();
        createThemeParam.Add("@name", name);
        createThemeParam.Add("@login", login);

        await conn.ExecuteAsync(
          StoredProcedures.CreateResourceGroup,
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
  }
}