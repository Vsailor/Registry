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
      container.RegisterType<IThemeRepository, ThemeRepository>();
      container.RegisterType<IResourceRepository, ResourceRepository>();
    }
  }
}