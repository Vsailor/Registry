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
    }
  }
}