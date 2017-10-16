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
  public class CategoryRepository : BaseRepository, ICategoryRepository
  {
    public async Task<GetAllCategoriesResult[]> GetAllCategories()
    {
      IEnumerable<GetAllCategoriesResult> result;
      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        result = await conn.QueryAsync<GetAllCategoriesResult>(
          StoredProcedures.GetAllCategories,
          commandType: CommandType.StoredProcedure);
      }

      if (result.Count() == 0)
      {
        return new GetAllCategoriesResult[0];
      }

      return result.ToArray();
    }

    public async Task UpdateCategory(Guid id, string name)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);
      parameters.Add("@name", name);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.UpdateCategory,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task CreateCategory(Guid parentId, string name)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@ParentId", parentId);
      parameters.Add("@Name", name);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.CreateCategory,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }

    public async Task DeleteCategory(Guid id)
    {
      var parameters = new DynamicParameters();
      parameters.Add("@id", id);

      using (IDbConnection conn = new SqlConnection(ConnectionString))
      {
        await conn.ExecuteAsync(
          StoredProcedures.DeleteCategory,
          parameters,
          commandType: CommandType.StoredProcedure);
      }
    }
  }
}