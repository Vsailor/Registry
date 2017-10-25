using System.Windows;
using System.Windows.Controls;
using Registry.Common;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  public partial class AuthorizedUserBackground : UserControl
  {
    public AuthorizedUserBackground()
    {
      InitializeComponent();
    }

    private void SignOutButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControl(new Login());
    }
  }
}