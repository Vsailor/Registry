using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Registry.Common;
using Registry.Data.Models;
using Registry.Models;

namespace Registry.Services.Abstract
{
  public interface IUserService
  {
    Task<IEnumerable<UserBasicInfo>> GetAllUsers();

    Task<UserDetailedInfo> GetUser(string login);

    Task CreateUser(string login, string name, string password, int groupId, bool isAdmin);

    Task DeleteUser(string login);

    Task UpdateUser(
      string login,
      string name,
      string password,
      bool isActive,
      bool cryptPassword,
      int groupId,
      bool isAdmin);

    Task<GetAllUserGroupsResult[]> GetAllUserGroups();

    Task CreateUserGroup(string name);

    Task UpdateUserGroup(UpdateUserGroupRequest request);

    Task DeleteUserGroup(int id);
  }
}