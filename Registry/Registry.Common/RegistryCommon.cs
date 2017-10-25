using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Practices.Unity;
using Microsoft.SqlServer.Server;

namespace Registry.Common
{
  public class RegistryCommon
  {
    private static RegistryCommon _instance;

    private RegistryCommon() { }

    public static RegistryCommon Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new RegistryCommon();
        }
        return _instance;
      }
    }

    public Grid MainGrid { get; set; }

    public TextBlock MainProgressBar { get; set; }

    public UnityContainer Container { get; set; }
    public string Login { get; set; }

    public Permission[] UserPermissions;

    public Visibility CheckVisibility(params Permission[] permissions)
    {
      return CheckPermissions(permissions) ? Visibility.Visible : Visibility.Collapsed;
    }

    public bool CheckPermissions(params Permission[] permissions)
    {
      foreach (var p in permissions)
      {
        if (UserPermissions.Contains(p))
        {
          return true;
        }
      }
      return false;
    }
  }
}