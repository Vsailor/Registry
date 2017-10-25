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
  public class ResourceRepository : BaseRepository, IResourceRepository
  {
    public async Task CreateResource(CreateResourceRequest request)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@owner_login", request.OwnerLogin);
      parameters.Add("@name", request.Name);
      parameters.Add("@description", request.Description);
      parameters.Add("@url", request.Url);
      parameters.Add("@tags", request.Tags);
      parameters.Add("@fileName", request.FileName);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.CreateResource,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task<GetAllResourcesResult[]> GetAllResources()
    {
      IEnumerable<GetAllResourcesResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetAllResourcesResult>(
          StoredProcedures.GetAllResources,
          commandType: CommandType.StoredProcedure);
      }

      if (!result.Any())
      {
        return new GetAllResourcesResult[0];
      }

      return result.ToArray();
    }
  }
}
