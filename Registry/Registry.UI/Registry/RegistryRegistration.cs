using Microsoft.Practices.Unity;
using Registry.Services;
using Registry.Services.Abstract;

namespace Registry
{
  public class RegistryRegistration
  {
    public static void Register(IUnityContainer container)
    {
      container.RegisterType<IUserService, UserService>();
    }
  }
}