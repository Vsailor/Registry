using System;
using System.CodeDom;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using Registry.Common;

namespace Registry.UI
{
  public partial class App : Application
  {
    protected override async void OnStartup(StartupEventArgs e)
    {
      await ClearCache();
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

      if (e.Exception.GetType() == typeof (FaultException))
      {
        RegistryCommon.Instance.MainProgressBar.Text = "Сталася помилка при спробi під'єднатися до віддаленого сервера'";
        e.Handled = true;
        return;
      }

      MessageBox.Show("Невідома помилка програми", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
      e.Handled = true;
    }

    private async void App_OnExit(object sender, ExitEventArgs e)
    {
      await ClearCache();
    }

    private async Task ClearCache()
    {
      RegistryCommon.Instance.CacheDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}Temp\\";
      if (!Directory.Exists(RegistryCommon.Instance.CacheDirectory))
      {
        Directory.CreateDirectory(RegistryCommon.Instance.CacheDirectory);
        return;
      }

      try
      {
        DirectoryInfo di = new DirectoryInfo(RegistryCommon.Instance.CacheDirectory);
        foreach (FileInfo file in di.GetFiles())
        {
          file.Delete();
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
          dir.Delete(true);
        }
      }
      catch(Exception ex)
      {
        return;
      }

      Directory.CreateDirectory(RegistryCommon.Instance.CacheDirectory);
    }
  }
}