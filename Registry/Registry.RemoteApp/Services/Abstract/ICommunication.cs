using System.ServiceModel;

namespace Registry.Data.Services.Abstract
{
  [ServiceContract]
  public interface ICommunication : ICategoryRepository, IResourceGroupRepository, IResourceRepository, IUserRepository
  {
  }
}