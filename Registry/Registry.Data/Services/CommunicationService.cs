using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage.Blob;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;

namespace Registry.Data.Services
{
  public class CommunicationService : ICommunication
  {
    private IResourceRepository _resourceRepository = RegistryCommon.Container.Resolve<IResourceRepository>();
    private IUserRepository _userRepository = RegistryCommon.Container.Resolve<IUserRepository>();
    private IResourceGroupRepository _resourceGroupRepository = RegistryCommon.Container.Resolve<IResourceGroupRepository>();
    private ICategoryRepository _categoryRepository = RegistryCommon.Container.Resolve<ICategoryRepository>();

    public async Task<GetAllCategoriesResult[]> GetAllCategories()
    {
      return await _categoryRepository.GetAllCategories();
    }

    public async Task UpdateCategory(Guid id, string name)
    {
      await _categoryRepository.UpdateCategory(id, name);
    }

    public async Task CreateCategory(Guid parentId, string name)
    {
      await _categoryRepository.CreateCategory(parentId, name);
    }

    public async Task DeleteCategory(Guid id)
    {
      await _categoryRepository.DeleteCategory(id);
    }

    public async Task<GetAllGroupsResult[]> GetAllResourceGroups()
    {
      return await _resourceGroupRepository.GetAllResourceGroups();
    }

    public async Task UpdateResourceGroup(Guid id, string name)
    {
      await _resourceGroupRepository.UpdateResourceGroup(id, name);
    }

    public async Task DeleteGroup(Guid id)
    {
      await _resourceGroupRepository.DeleteGroup(id);
    }

    public async Task CreateGroup(string name, string login)
    {
      await _resourceGroupRepository.CreateGroup(name, login);
    }

    public async Task CreateResource(CreateResourceRequest request)
    {
      await _resourceRepository.CreateResource(request);
    }

    public async Task<GetAllResourcesResult[]> GetAllResources(int count, int endId)
    {
      return await _resourceRepository.GetAllResources(count, endId);
    }

    public string GetCloudBlobConnectionString()
    {
      return _resourceRepository.GetCloudBlobConnectionString();
    }

    public async Task CreateUser(CreateUserRequest request)
    {
      await _userRepository.CreateUser(request);
    }

    public async Task UpdateUser(UpdateUserRequest request)
    {
      await _userRepository.UpdateUser(request);
    }

    public async Task DeleteUser(string login)
    {
      await _userRepository.DeleteUser(login);
    }

    public async Task<GetAllUsersResult[]> GetAllUsers()
    {
      return await _userRepository.GetAllUsers();
    }

    public async Task<GetUserByLoginResult> GetUserByLogin(string login)
    {
      return await _userRepository.GetUserByLogin(login);
    }

    public async Task CreateUserGroup(string name)
    {
      await _userRepository.CreateUserGroup(name);
    }

    public async Task UpdateUserGroup(UpdateUserGroupRequest request)
    {
      await _userRepository.UpdateUserGroup(request);
    }

    public async Task<GetAllUserGroupsResult[]> GetUserGroups()
    {
      return await _userRepository.GetUserGroups();
    }

    public async Task DeleteUserGroup(int id)
    {
      await _userRepository.DeleteUserGroup(id);
    }
  }
}
