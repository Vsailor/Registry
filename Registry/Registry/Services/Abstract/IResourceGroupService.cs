using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Services.Abstract
{
  public interface IResourceGroupService
  {
    Task<GetAllGroupsResult[]> GetAllResourceGroups();

    Task UpdateResourceGroup(Guid id, string name);

    Task DeleteResourceGroup(Guid id);

    Task CreateResourceGroup(string name, string login);
  }
}