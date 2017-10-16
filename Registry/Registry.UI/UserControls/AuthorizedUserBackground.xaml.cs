using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Registry.Common;
using Registry.UI.Extensions;
using Registry.UI.UserControls.Admin;

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