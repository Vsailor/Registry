using System.CodeDom;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Registry.Common;

namespace Registry.UI
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      RegistryCommon.Instance.Container = new UnityContainer();
      RegistryRegistration.Register(RegistryCommon.Instance.Container);
    }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      if (e.Exception.GetType() == typeof (EndpointNotFoundException))
      {
        RegistryCommon.Instance.MainProgressBar.Text = "Неможливо під'єднатися до віддаленого сервера";
        e.Handled = true;
        return;
      }

      MessageBox.Show("Невідома помилка програми", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
      e.Handled = true;
    }
  }
}