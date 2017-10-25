using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  public interface IResourceGroupRepository
  {
    Task<GetAllGroupsResult[]> GetAllResourceGroups();

    Task UpdateResourceGroup(Guid id, string name);

    Task DeleteGroup(Guid id);

    Task<string[]> GetGroups(string login);

    Task CreateGroup(string name, CreateGroupResourceRequest[] createGroupResourceRequest);
  }
}