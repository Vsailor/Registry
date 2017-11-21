using System;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;

namespace Registry.Data.Services
{
  public class CommunicationService : ICommunication
  {
    private IResourceRepository _resourceRepository;
    private IUserRepository _userRepository;
    private IResourceGroupRepository _resourceGroupRepository;
    private ICategoryRepository _categoryRepository;

    public CommunicationService()
    {
      RegistryCommon.Container = new UnityContainer();
      RegistryDataRegistration.Register(RegistryCommon.Container);
      _resourceRepository = RegistryCommon.Container.Resolve<IResourceRepository>();
      _userRepository = RegistryCommon.Container.Resolve<IUserRepository>();
      _resourceGroupRepository = RegistryCommon.Container.Resolve<IResourceGroupRepository>();
      _categoryRepository = RegistryCommon.Container.Resolve<ICategoryRepository>();
    }

    public async Task<GetAllCategoriesResult[]> GetAllCategories()
    {
      Tracer.TraceInfo();
      return await _categoryRepository.GetAllCategories();
    }

    public async Task UpdateCategory(Guid id, string name)
    {
      Tracer.TraceEnter($"id = {id}, name = {name}");
      await _categoryRepository.UpdateCategory(id, name);
      Tracer.TraceExit();
    }

    public async Task CreateCategory(Guid parentId, string name)
    {
      Tracer.TraceEnter($"parentid = {parentId}, name = {name}");
      await _categoryRepository.CreateCategory(parentId, name);
      Tracer.TraceExit();
    }

    public async Task DeleteCategory(Guid id)
    {
      Tracer.TraceEnter($"id = {id}");
      await _categoryRepository.DeleteCategory(id);
      Tracer.TraceExit();
    }

    public async Task<GetAllGroupsResult[]> GetAllResourceGroups()
    {
      Tracer.TraceInfo();
      return await _resourceGroupRepository.GetAllResourceGroups();
    }

    public async Task UpdateResourceGroup(Guid id, string name)
    {
      Tracer.TraceEnter($"id = {id}, name = {name}");
      await _resourceGroupRepository.UpdateResourceGroup(id, name);
      Tracer.TraceExit();
    }

    public async Task DeleteGroup(Guid id)
    {
      Tracer.TraceEnter($"id = {id}");
      await _resourceGroupRepository.DeleteGroup(id);
      Tracer.TraceExit();
    }

    public async Task CreateGroup(string name, string login)
    {
      Tracer.TraceEnter($"login = {login}, name = {name}");
      await _resourceGroupRepository.CreateGroup(name, login);
      Tracer.TraceExit();
    }

    public async Task CreateResource(CreateResourceRequest request)
    {
      Tracer.TraceEnter($"request = <{JsonConvert.SerializeObject(request)}>");
      await _resourceRepository.CreateResource(request);
      Tracer.TraceExit();
    }

    public async Task<GetAllResourcesResult[]> GetAllResources()
    {
      return await _resourceRepository.GetAllResources();
    }

    public string GetCloudBlobConnectionString()
    {
      Tracer.TraceInfo();
      return Tracer.TraceReturn(_resourceRepository.GetCloudBlobConnectionString());
    }

    public async Task<GetAllResourcesResult[]> GetResources(UseFiltersRequest filter)
    {
      return await _resourceRepository.GetResources(filter);
    }

    public async Task<GetResourceDetailsResult> GetResourceDetails(string resourceId)
    {
      Tracer.TraceEnter($"resourceId = {resourceId}");
      return Tracer.TraceReturn(await _resourceRepository.GetResourceDetails(resourceId));
    }

    public async Task UpdateResource(UpdateResourceRequest request)
    {
      await _resourceRepository.UpdateResource(request);
    }

    public async Task DeleteResource(string id)
    {
      await _resourceRepository.DeleteResource(id);
    }

    public async Task CreateUser(CreateUserRequest request)
    {
      Tracer.TraceEnter($"request = <{JsonConvert.SerializeObject(request)}>");
      await Tracer.TraceReturn(_userRepository.CreateUser(request));
    }

    public async Task UpdateUser(UpdateUserRequest request)
    {
      Tracer.TraceEnter($"request = <{JsonConvert.SerializeObject(request)}>");
      await _userRepository.UpdateUser(request);
      Tracer.TraceExit();
    }

    public async Task DeleteUser(string login)
    {
      Tracer.TraceEnter($"login = {login}");
      await _userRepository.DeleteUser(login);
      Tracer.TraceExit();
    }

    public async Task<GetAllUsersResult[]> GetAllUsers()
    {
      Tracer.TraceInfo();
      return await _userRepository.GetAllUsers();
    }

    public async Task<GetUserByLoginResult> GetUserByLogin(string login)
    {
      Tracer.TraceEnter(login);
      return Tracer.TraceReturn(await _userRepository.GetUserByLogin(login));
    }

    public async Task CreateUserGroup(string name)
    {
      Tracer.TraceEnter($"name = {name}");
      await _userRepository.CreateUserGroup(name);
      Tracer.TraceExit();
    }

    public async Task UpdateUserGroup(UpdateUserGroupRequest request)
    {
      Tracer.TraceEnter($"request = <{JsonConvert.SerializeObject(request)}>");
      await _userRepository.UpdateUserGroup(request);
      Tracer.TraceExit();
    }

    public async Task<GetAllUserGroupsResult[]> GetUserGroups()
    {
      Tracer.TraceInfo();
      return Tracer.TraceReturn(await _userRepository.GetUserGroups());
    }

    public async Task DeleteUserGroup(int id)
    {
      Tracer.TraceEnter($"id = {id}");
      await _userRepository.DeleteUserGroup(id);
      Tracer.TraceExit();
    }
  }
}
