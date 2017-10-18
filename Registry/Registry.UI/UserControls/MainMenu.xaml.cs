using System.Windows;
using System.Windows.Controls;
using Registry.Common;
using Registry.UI.Extensions;
using Registry.UI.UserControls.Admin;

namespace Registry.UI.UserControls
{
  public partial class MainMenu : UserControl
  {
    public MainMenu()
    {
      InitializeComponent();

      NewUserButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.CreateUser);
      ChangeUserButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.SeeUserList);
      CategoriesButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.SeeCategoriesList);
    }

    private void NewUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new CreateUser());
    }

    private void ChangeUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
    }

    private void CategoriesButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Categories());
    }
  }
}