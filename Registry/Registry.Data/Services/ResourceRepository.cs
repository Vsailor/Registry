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
    private const string Names = "ReourcesNames";

    public async Task CreateResource(CreateResourceRequest request)
    {
      Console.WriteLine("\n##################################################################\n");
      var resourceId = request.SaveDate;
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

      await RegistryCommon.RedisDb.HashSetAsync(Names, request.Name, resourceId);
    }

    private async Task CreateResourceRecord(CreateResourceRequest request, string resourceId)
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
        resourceId,
        serializedResource);
    }

    private async Task UpdateRelationsRecord(string table, string key, string resourceId)
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
          ResourcesIds = new List<string> { resourceId }
        };
      }

      var serializedResource = JsonConvert.SerializeObject(resources);
      Console.WriteLine("Update relation : {0} - {1} - {2}", table, key, serializedResource);

      await RegistryCommon.RedisDb.HashSetAsync(
          table,
          key,
          serializedResource);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources(int count, int endId)
    {
      RedisValue[] resourcesKeys = await RegistryCommon.RedisDb.HashKeysAsync(ResourcesName);
      var query = resourcesKeys.OrderByDescending(item => int.Parse(item));
      if (endId != -1)
      {
        resourcesKeys = query.Where(key => int.Parse(key) < endId).Take(count).ToArray();
      }
      else
      {
        resourcesKeys = query.Take(count).ToArray();
      }

      RedisValue[] response = await RegistryCommon.RedisDb.HashGetAsync(ResourcesName, resourcesKeys);

      var result = new List<GetAllResourcesResult>();
      for (int i=0; i<response.Length; i++)
      {
        var res = JsonConvert.DeserializeObject<GetAllResourcesResult>(response[i].ToString());
        res.Id = resourcesKeys[i];
        result.Add(res);
      }

      return result.ToArray();
    }

    public async Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter, int count, int endId)
    {
      var resources = (await RegistryCommon.RedisDb.HashKeysAsync(ResourcesName)).Select(k => k.ToString()).ToList();
      RedisValue response;
      if (!string.IsNullOrEmpty(filter.Name))
      {
        var filterByName = (await RegistryCommon.RedisDb.HashKeysAsync(Names)).Select(f => f.ToString())
          .Where(f => f.ToString().ToLowerInvariant().Contains(filter.Name.ToLowerInvariant()));
        var filterByNameValues = (await RegistryCommon.RedisDb.HashGetAsync(Names, filterByName.Select(f => (RedisValue)f).ToArray())).Select(f => f.ToString()).ToArray();
        if (!filterByNameValues.Any())
        {
          resources.Clear();
        }

        resources =  resources.Where(r => filterByNameValues.Contains(r)).ToList();
      }

      if (filter.CategoryId.HasValue)
      {
        response = await RegistryCommon.RedisDb.HashGetAsync(CategoriesName, filter.CategoryId.Value.ToString());
        if (response.HasValue)
        {
          var resourcesInCategory = JsonConvert.DeserializeObject<Resources>(response.ToString()).ResourcesIds;
          resources = resources.Where(r => resourcesInCategory.Contains(r)).ToList();
        }
        else
        {
          resources.Clear();
        }
      }

      if (filter.ResourceGroupId.HasValue)
      {
        response = await RegistryCommon.RedisDb.HashGetAsync(ResourceGroupsName, filter.ResourceGroupId.Value.ToString());
        if (response.HasValue)
        {
          var resourcesInGroup = JsonConvert.DeserializeObject<Resources>(response.ToString()).ResourcesIds;
          resources = resources.Where(r => resourcesInGroup.Contains(r)).ToList();
        }
        else
        {
          resources.Clear();
        }
      }

      if (filter.Tags != null && filter.Tags.Length != 0)
      {
        var allKeys = await RegistryCommon.RedisDb.HashKeysAsync(TagsName);
        var keys = new List<string>();
        foreach (string tag in filter.Tags)
        {
          keys.AddRange(allKeys.Where(k => k.ToString().ToLowerInvariant().Contains(tag.ToLowerInvariant())).Select(k => k.ToString()));
        }

        var filterByTags = new Dictionary<string, string[]>();
        foreach (var key in keys)
        {
          response = await RegistryCommon.RedisDb.HashGetAsync(TagsName, key);
          filterByTags.Add(key, JsonConvert.DeserializeObject<Resources>(response.ToString()).ResourcesIds.ToArray());
        }

        var resourcesByTags = filterByTags.Select(f => f.Value).ToArray();
        for (int i = 0; i < resourcesByTags.GetLength(0)-1; i++)
        {
          resourcesByTags[i+1] = resourcesByTags[i].Union(resourcesByTags[i + 1]).ToArray();
        }

        if (!resourcesByTags.Any())
        {
          resources.Clear();
        }

        resources = resources.Where(r => resourcesByTags.Last().Contains(r.ToString())).ToList();
      }

      var allResourcesResults = new List<GetAllResourcesResult>();
      for (int i = 0; i < resources.Count; i++)
      {
        response = await RegistryCommon.RedisDb.HashGetAsync(ResourcesName, resources[i]);
        var res = JsonConvert.DeserializeObject<GetAllResourcesResult>(response.ToString());
        res.Id = resources[i];
        allResourcesResults.Add(res);
      }

      return allResourcesResults.ToArray();
    }

    public string GetCloudBlobConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString;
    }
  }
}
