using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Communication;
using Registry.Models;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class UserService : IUserService
  {
    private IUserRepository _userRepository;

    public async Task<IEnumerable<UserBasicInfo>> GetAllUsers()
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      GetAllUsersResult[] result = await _userRepository.GetAllUsersAsync();
      return result.Select(item => new UserBasicInfo
      {
        Login = item.Login,
        Name = item.Name
      });
    }

    public async Task<UserDetailedInfo> GetUser(string login)
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      GetUserByLoginResult result = await _userRepository.GetUserByLoginAsync(login);
      if (result == null)
      {
        return null;
      }

      return new UserDetailedInfo
      {
        Login = login,
        Name = result.Name,
        IsActive = result.Enabled,
        Password = result.Password,
        GroupId = result.GroupId,
        IsAdmin = result.IsAdmin
      };
    }

    public async Task CreateUser(
      string login, 
      string name, 
      string password,
      int groupId,
      bool isAdmin)
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      await _userRepository.CreateUserAsync(new CreateUserRequest
      {
        Name = name,
        Login = login,
        Password = SecurityService.Crypt(password),
        IsAdmin = isAdmin,
        GroupId = groupId
      });
    }

    public async Task DeleteUser(string login)
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      await _userRepository.DeleteUserAsync(login);
    }

    public async Task UpdateUser(
      string login, 
      string name, 
      string password, 
      bool isActive, 
      bool cryptPassword,
      int groupId,
      bool isAdmin)
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      await _userRepository.UpdateUserAsync(new UpdateUserRequest
      {
        Name = name,
        Login = login,
        Password = cryptPassword ? SecurityService.Crypt(password) : password,
        IsEnabled = isActive,
        GroupId = groupId,
        IsAdmin = isAdmin
      });
    }

    public async Task<GetAllUserGroupsResult[]> GetAllUserGroups()
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      return await _userRepository.GetUserGroupsAsync();
    }

    public async Task CreateUserGroup(string name)
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
      await _userRepository.CreateUserGroupAsync(name);
    }

    public async Task UpdateUserGroup(UpdateUserGroupRequest request)
    {
      await _userRepository.UpdateUserGroupAsync(request);
    }

    public async Task DeleteUserGroup(int id)
    {
      await _userRepository.DeleteUserGroupAsync(id);
    }
  }
}