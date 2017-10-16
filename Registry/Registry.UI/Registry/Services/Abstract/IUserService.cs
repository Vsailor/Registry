using System.Collections.Generic;
using System.Threading.Tasks;
using Registry.Common;
using Registry.Models;

namespace Registry.Services.Abstract
{
  public interface IUserService
  {
    Task<IEnumerable<UserBasicInfo>> GetAllUsers();

    Task<UserDetailedInfo> GetUser(string login);

    Task CreateUser(string login, string name, string password, Role role);

    Task DeleteUser(string login);

    Task UpdateUser(
      string login,
      string name,
      string password,
      Role role,
      bool isActive);
  }
}