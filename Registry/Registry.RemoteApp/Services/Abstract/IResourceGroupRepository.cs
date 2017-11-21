using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Registry.Data.Models;

namespace Registry.Data.Services.Abstract
{
  [ServiceContract]
  public interface IResourceGroupRepository
  {
    [OperationContract]
    Task<GetAllGroupsResult[]> GetAllResourceGroups();

    [OperationContract]
    Task UpdateResourceGroup(Guid id, string name);

    [OperationContract]
    Task DeleteGroup(Guid id);

    [OperationContract]
    Task CreateGroup(string name, string login);
  }
}