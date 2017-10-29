using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using Registry.Data.Models;
using Registry.Data.RedisModels;
using Registry.Data.Services.Abstract;
using StackExchange.Redis;

namespace Registry.Data.Services
{
  public class ResourceRepository : BaseRepository, IResourceRepository
  {
    private const string ResourcesName = "Resources";
    private const string CategoriesName = "Categories";
    private const string ResourceGroupsName = "ResourceGroups";
    private const string TagsName = "Tags";

    public async Task CreateResource(CreateResourceRequest request)
    {
      Console.WriteLine("\n##################################################################\n");
      var resourceId = Guid.NewGuid();
      await CreateResourceRecord(request, resourceId);
      await UpdateRelationsRecord(CategoriesName, request.CategoryId.ToString(), resourceId);
      foreach (var item in request.ResourceGroups)
      {
        await UpdateRelationsRecord(ResourceGroupsName, item.ToString(), resourceId);
      }

      foreach (var item in request.Tags)
      {
        await UpdateRelationsRecord(TagsName, item, resourceId);
      }
    }

    private async Task CreateResourceRecord(CreateResourceRequest request, Guid resourceId)
    {
      var resource = new Resource
      {
        Description = request.Description,
        FileName = request.FileName,
        Name = request.Name,
        Owner = request.OwnerLogin,
        Url = request.Url,
        SaveDate = request.SaveDate
      };

      string serializedResource = JsonConvert.SerializeObject(resource);
      Console.WriteLine("Creation of resource : {0} - {1}", resourceId, serializedResource);

      await RegistryCommon.RedisDb.HashSetAsync(
        ResourcesName,
        resourceId.ToString(),
        serializedResource);
    }

    private async Task UpdateRelationsRecord(string table, string key, Guid resourceId)
    {
      RedisValue res = await RegistryCommon.RedisDb.HashGetAsync(table, key);
      Resources resources;
      if (res.HasValue)
      {
        resources = JsonConvert.DeserializeObject<Resources>(res.ToString());
        resources.ResourcesIds.Add(resourceId);
      }
      else
      {
        resources = new Resources
        {
          ResourcesIds = new List<Guid> { resourceId }
        };
      }

      var serializedResource = JsonConvert.SerializeObject(resources);
      Console.WriteLine("Update relation : {0} - {1} - {2}", table, key, serializedResource);

      await RegistryCommon.RedisDb.HashSetAsync(
          table,
          key,
          serializedResource);
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

    public string GetCloudBlobConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString;
    }
  }
}
