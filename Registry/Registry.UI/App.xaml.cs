using System.Windows;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data;

namespace Registry.UI
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      RegistryCommon.Instance.Container = new UnityContainer();

      RegistryRegistration.Register(RegistryCommon.Instance.Container);
      RegistryDataRegistration.Register(RegistryCommon.Instance.Container);
    }
  }
}