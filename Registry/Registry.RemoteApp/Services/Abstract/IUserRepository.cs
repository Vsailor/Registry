using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  [ServiceContract]
  public interface IUserRepository
  {
    [OperationContract]
    Task CreateUser(CreateUserRequest request);

    [OperationContract]
    Task UpdateUser(UpdateUserRequest request);

    [OperationContract]
    Task DeleteUser(string login);

    [OperationContract]
    Task<GetAllUsersResult[]> GetAllUsers();

    [OperationContract]
    Task<GetUserByLoginResult> GetUserByLogin(string login);

    [OperationContract]
    Task CreateUserGroup(string name);

    [OperationContract]
    Task UpdateUserGroup(UpdateUserGroupRequest request);

    [OperationContract]
    Task<GetAllUserGroupsResult[]> GetUserGroups();

    [OperationContract]
    Task DeleteUserGroup(int id);
  }
}