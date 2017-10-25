using System.Windows;
using System.Windows.Threading;
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

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      MessageBox.Show("Невідома помилка програми", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
      e.Handled = true;
    }
  }
}