using System.Windows;
using Registry.Common;
using Registry.UI.UserControls;

namespace Registry.UI
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      RegistryCommon.Instance.MainGrid = MainGrid;
      RegistryCommon.Instance.MainProgressBar = MainProgressBar;
      MainGrid.Children.Add(new Login());
    }
  }
}
