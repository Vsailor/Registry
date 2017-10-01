using System.Collections.Generic;
using System.Threading.Tasks;
using Registry.Models;

namespace Registry.Services.Abstract
{
  public interface IUserService
  {
    Task<IEnumerable<UserBasicInfo>> GetAllUsers();

    Task<UserDetailedInfo> GetUser(string login);
  }
}