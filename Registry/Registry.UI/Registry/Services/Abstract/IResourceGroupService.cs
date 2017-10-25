using System;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Services.Abstract
{
  public interface IResourceGroupService
  {
    Task<GetAllGroupsResult[]> GetAllThemes();

    Task UpdateTheme(Guid id, string name);

    Task DeleteTheme(Guid id);

    Task CreateTheme(string name, CreateGroupResourceRequest[] request);

    Task<string[]> GetUserThemes(string login);
  }
}