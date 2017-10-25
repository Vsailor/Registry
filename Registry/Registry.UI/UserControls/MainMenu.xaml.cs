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

      ChangeUserButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.SeeUserList);
      CategoriesButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.SeeCategoriesList);
      ThemesButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.SeeThemesList);
      ResourcesButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.CanSeeListOfResources);
    }

    private void ChangeUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
    }

    private void CategoriesButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Categories());
    }

    private void ThemesButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private void ResourcesButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Resources());
    }
  }
}