using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Practices.ObjectBuilder2;
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
    private const string ResourcesIds = "ResourcesIds";

    public async Task CreateResource(CreateResourceRequest request)
    {
      if ((await Redis.HashValuesAsync(ResourcesIds)).Select(x => x.ToString()).Contains(request.Uid))
      {
        throw new FaultException(new FaultReason("UidExist"));
      }

      var resourceId = Guid.NewGuid().ToString();
      await CreateOrUpdateResourceRecord(request, resourceId);
      await UpdateRelationsRecord(CategoriesName, request.CategoryId.ToString(), resourceId);
      foreach (var item in request.ResourceGroups)
      {
        await UpdateRelationsRecord(ResourceGroupsName, item.ToString(), resourceId);
      }

      foreach (var item in request.Tags)
      {
        await UpdateRelationsRecord(TagsName, item, resourceId);
      }

      await Redis.HashSetAsync(Names, resourceId, request.Name);
      await Redis.HashSetAsync(ResourcesIds, resourceId, request.Uid);
    }

    public async Task UpdateResource(UpdateResourceRequest request)
    {
      if ((await Redis.HashGetAllAsync(ResourcesIds)).ToDictionary(x => x.Name.ToString(), x=> x.Value.ToString())
        .Where(x => x.Value == request.Uid && x.Key != request.Id).Count() != 0)
      {
        throw new FaultException(new FaultReason("UidExist"));
      }

      Tracer.TraceMessage($"Updating of resource : {request.Id}");
      Tracer.TraceMessage($"Deletion of relations from table <{CategoriesName}>");
      await DeleteRelationsRecord(CategoriesName, request.Id);

      Tracer.TraceMessage($"Deletion of relations from table <{ResourceGroupsName}>");
      await DeleteRelationsRecord(ResourceGroupsName, request.Id);

      Tracer.TraceMessage($"Deletion of relation from table <{Names}>");
      await Redis.HashDeleteAsync(Names, request.Id);
      await Redis.HashDeleteAsync(ResourcesIds, request.Id);

      Tracer.TraceMessage($"Deletion of relations from table <{TagsName}>");
      await DeleteRelationsRecord(TagsName, request.Id);

      Tracer.TraceMessage("Updating of <Resource> table");
      await CreateOrUpdateResourceRecord(request, request.Id);

      await CreateResourceInternal(request, request.Id);
    }

    private async Task CreateResourceInternal(CreateResourceRequest request, string resourceId)
    {
      await CreateOrUpdateResourceRecord(request, resourceId);
      await UpdateRelationsRecord(CategoriesName, request.CategoryId.ToString(), resourceId);
      foreach (var item in request.ResourceGroups)
      {
        await UpdateRelationsRecord(ResourceGroupsName, item.ToString(), resourceId);
      }

      foreach (var item in request.Tags)
      {
        await UpdateRelationsRecord(TagsName, item, resourceId);
      }

      await Redis.HashSetAsync(Names, resourceId, request.Name);
      await Redis.HashSetAsync(ResourcesIds, resourceId, request.Uid);
    }

    public async Task DeleteResource(string id)
    {
      Tracer.TraceMessage($"Deletion of resource : {id}");
      await Redis.HashDeleteAsync(ResourcesName, id);

      Tracer.TraceMessage($"Deletion of relations from table <{CategoriesName}>");
      await DeleteRelationsRecord(CategoriesName, id);

      Tracer.TraceMessage($"Deletion of relations from table <{ResourceGroupsName}>");
      await DeleteRelationsRecord(ResourceGroupsName, id);

      Tracer.TraceMessage($"Deletion of relation from table <{Names}>");
      await Redis.HashDeleteAsync(Names, id);
      await Redis.HashDeleteAsync(ResourcesIds, id);

      Tracer.TraceMessage($"Deletion of relations from table <{TagsName}>");
      await DeleteRelationsRecord(TagsName, id);
    }

    public async Task<GetResourceDetailsResult> GetResourceDetails(string resourceId)
    {
      var result = new GetResourceDetailsResult();
      var allCategories = await Redis.HashGetAllAsync(CategoriesName);
      for (int i = 0; i < allCategories.Length; i++)
      {
        Resources res = Unpack<Resources>(allCategories[i].Value.ToString());
        if (res.ResourcesIds.Any(r => r == resourceId.ToString()))
        {
          result.Category = Guid.Parse(allCategories[i].Name);
          break;
        }
      }

      var allTags = await Redis.HashGetAllAsync(TagsName);
      var tags = new List<string>();
      for (int i = 0; i < allTags.Length; i++)
      {
        Resources res = Unpack<Resources>(allTags[i].Value.ToString());
        if (res.ResourcesIds.Any(r => r == resourceId.ToString()))
        {
          tags.Add(allTags[i].Name);
        }
      }

      result.Tags = tags.ToArray();

      var allResourceGroups = await Redis.HashGetAllAsync(ResourceGroupsName);
      var resourceGroups = new List<string>();
      for (int i = 0; i < allResourceGroups.Length; i++)
      {
        Resources res = Unpack<Resources>(allResourceGroups[i].Value.ToString());
        if (res.ResourcesIds.Any(r => r == resourceId.ToString()))
        {
          resourceGroups.Add(allResourceGroups[i].Name);
        }
      }

      result.ResourceGroups = resourceGroups.Select(Guid.Parse).ToArray();

      result.Uid = (await Redis.HashGetAsync(ResourcesIds, resourceId)).ToString();

      return result;
    }

    private async Task CreateOrUpdateResourceRecord(CreateResourceRequest request, string resourceId)
    {
      var newResource = new Resource
      {
        Description = request.Description,
        FileName = request.FileName,
        Name = request.Name,
        Owner = request.OwnerLogin,
        Url = request.Url,
        SaveDate = request.SaveDate
      };

      string serializedResource = Pack(newResource);
      Tracer.TraceMessage($"Create/Update of resource : {resourceId} - {serializedResource}");

      await Redis.HashSetAsync(
        ResourcesName,
        resourceId,
        serializedResource);

      Tracer.TraceMessage("Resource has been created");
    }

    private async Task DeleteRelationsRecord(string table, string resourceId)
    {
      Dictionary<string, Resources> data = (await Redis.HashGetAllAsync(table))
        .ToDictionary(x => x.Name.ToString(), x => Unpack<Resources>(x.Value))
        .Where(x => x.Value.ResourcesIds.Contains(resourceId))
        .ToDictionary(x => x.Key, x => x.Value);

      foreach (var rel in data)
      {
        rel.Value.ResourcesIds.Remove(resourceId);
        await Redis.HashSetAsync(table, rel.Key, Pack(rel.Value));
      }
    }

    private async Task UpdateRelationsRecord(string table, string key, string resourceId)
    {
      RedisValue res = await Redis.HashGetAsync(table, key);
      Resources resources;
      if (res.HasValue)
      {
        resources = Unpack<Resources>(res.ToString());
        resources.ResourcesIds.Add(resourceId);
      }
      else
      {
        resources = new Resources
        {
          ResourcesIds = new List<string> { resourceId }
        };
      }

      var serializedResource = Pack(resources);
      Tracer.TraceInfo($"Added relation to table <{table}> : {key} - {serializedResource}");

      await Redis.HashSetAsync(
          table,
          key,
          serializedResource);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources(int skip, int take)
    {
      var resources = (await Redis.HashGetAllAsync(ResourcesName))
        .ToDictionary(f => f.Name.ToString(), f => Unpack<Resource>(f.Value.ToString()))
        .OrderBy(f  => f.Value.SaveDate)
        .Skip(skip)
        .Take(take)
        .Select(f => new GetAllResourcesResult
        {
          Id = f.Key,
          FileName = f.Value.FileName,
          Url = f.Value.Url,
          Name = f.Value.Name,
          SaveDate = f.Value.SaveDate,
          Description = f.Value.Description
        });

      return resources.ToArray();
    }

    public async Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter)
    {
      var resources = (await Redis.HashKeysAsync(ResourcesName)).Select(k => k.ToString()).ToList();
      RedisValue response;
      if (!string.IsNullOrEmpty(filter.Id))
      {
        var allResourcesIds = (await Redis.HashGetAllAsync(ResourcesIds))
          .ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

        if (allResourcesIds.ContainsValue(filter.Id))
        {
          string resId = allResourcesIds.First(x => x.Value == filter.Id).Key;
          var resp = await Redis.HashGetAsync(ResourcesName, resId);
          GetAllResourcesResult result = Unpack<GetAllResourcesResult>(resp.ToString());
          result.Id = resId;
          return new[] { result };
        }

        return new GetAllResourcesResult[0];
      }

      if (!string.IsNullOrEmpty(filter.Name))
      {
        var filterByName = (await Redis.HashGetAllAsync(Names))
          .ToDictionary(f => f.Name.ToString(), f=> f.Value.ToString())
          .Where(f => f.Value.ToString().ToLowerInvariant().Contains(filter.Name.ToLowerInvariant()))
          .ToDictionary(x => x.Key, x => x.Value);

        resources = resources.Where(r => filterByName.Values.Contains(r)).ToList();
      }

      if (filter.CategoryId.HasValue)
      {
        response = await Redis.HashGetAsync(CategoriesName, filter.CategoryId.Value.ToString());
        if (response.HasValue)
        {
          var resourcesInCategory = Unpack<Resources>(response.ToString()).ResourcesIds;
          resources = resources.Where(r => resourcesInCategory.Contains(r)).ToList();
        }
        else
        {
          resources.Clear();
        }
      }

      if (filter.ResourceGroupId.HasValue)
      {
        response = await Redis.HashGetAsync(ResourceGroupsName, filter.ResourceGroupId.Value.ToString());
        if (response.HasValue)
        {
          var resourcesInGroup = Unpack<Resources>(response.ToString()).ResourcesIds;
          resources = resources.Where(r => resourcesInGroup.Contains(r)).ToList();
        }
        else
        {
          resources.Clear();
        }
      }

      if (filter.Tags != null && filter.Tags.Length != 0)
      {
        var allKeys = await Redis.HashKeysAsync(TagsName);
        var keys = new List<string>();
        foreach (string tag in filter.Tags)
        {
          keys.AddRange(allKeys.Where(k => k.ToString().ToLowerInvariant().Contains(tag.ToLowerInvariant())).Select(k => k.ToString()));
        }

        var filterByTags = new Dictionary<string, string[]>();
        foreach (var key in keys)
        {
          response = await Redis.HashGetAsync(TagsName, key);
          filterByTags.Add(key, Unpack<Resources>(response.ToString()).ResourcesIds.ToArray());
        }

        var resourcesByTags = filterByTags.Select(f => f.Value).ToArray();
        for (int i = 0; i < resourcesByTags.GetLength(0) - 1; i++)
        {
          resourcesByTags[i + 1] = resourcesByTags[i].Union(resourcesByTags[i + 1]).ToArray();
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
        response = await Redis.HashGetAsync(ResourcesName, resources[i]);
        var res = Unpack<GetAllResourcesResult>(response.ToString());
        res.Id = resources[i];
        allResourcesResults.Add(res);
      }

      return allResourcesResults.ToArray();
    }

    public string GetCloudBlobConnectionString()
    {
      return ConfigurationManager.ConnectionStrings["AzureStorageConnection"].ConnectionString;
    }

    private string Pack<T>(T obj)
    {
      return JsonConvert.SerializeObject(obj);
    }

    private T Unpack<T>(string obj)
    {
      return JsonConvert.DeserializeObject<T>(obj);
    }

    private IDatabase Redis => RegistryCommon.RedisDb;
  }
}
