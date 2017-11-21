using Microsoft.Practices.Unity;
using Registry.Data.Services;
using Registry.Data.Services.Abstract;

namespace Registry.Data
{
  public class RegistryDataRegistration
  {
    public static void Register(IUnityContainer container)
    {
      container.RegisterType<IUserRepository, UserRepository>();
      container.RegisterType<ICategoryRepository, CategoryRepository>();
      container.RegisterType<IResourceGroupRepository, ResourceGroupRepository>();
      container.RegisterType<IResourceRepository, ResourceRepository>();
    }
  }
}