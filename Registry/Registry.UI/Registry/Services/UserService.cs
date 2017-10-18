using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
using Registry.Data.Services.Abstract;
using Registry.Models;
using Registry.Services.Abstract;

namespace Registry.Services
{
  public class UserService : IUserService
  {
    private IUserRepository _userRepository;

    public UserService()
    {
      _userRepository = RegistryCommon.Instance.Container.Resolve<IUserRepository>();
    }

    public async Task<IEnumerable<UserBasicInfo>> GetAllUsers()
    {
      GetAllUsersResult[] result = await _userRepository.GetAllUsers();
      return result.Select(item => new UserBasicInfo
      {
        Login = item.Login,
        Name = item.Name
      });
    }

    public async Task<UserDetailedInfo> GetUser(string login)
    {
      GetUserByLoginResult result = await _userRepository.GetUserByLogin(login);
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
        Permissions = GetPermissions(result.Permissions)
      };
    }

    private Permission[] GetPermissions(string permissions)
    {
      if (string.IsNullOrEmpty(permissions))
      {
        return new Permission[0];
      }

      return permissions.Split(',').Select(item => (Permission) (int.Parse(item))).ToArray();
    }

    public async Task CreateUser(string login, string name, string password, Permission[] permissions)
    {
      await _userRepository.CreateUser(new CreateUserRequest
      {
        Name = name,
        Login = login,
        Password = SecurityService.Crypt(password),
        Permissions = string.Join(",", permissions.Select(p => (int)p))
      });
    }

    public async Task DeleteUser(string login)
    {
      await _userRepository.DeleteUser(login);
    }

    public async Task UpdateUser(
      string login, 
      string name, 
      string password, 
      bool isActive, 
      bool cryptPassword,
      Permission[] permissions)
    {
      await _userRepository.UpdateUser(new UpdateUserRequest
      {
        Name = name,
        Login = login,
        Password = cryptPassword ? SecurityService.Crypt(password) : password,
        IsEnabled = isActive,
        Permissions = string.Join(",", permissions.Select(p => (int)p))
      });
    }
  }
}