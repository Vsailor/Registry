using System.Windows.Controls;
using Microsoft.Practices.Unity;

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

    public string CacheDirectory { get; set; }
  }
}