using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace Registry.UI
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

    public ProgressBar MainProgressBar { get; set; }

    public IUnityContainer Container { get; set; }
  }
}