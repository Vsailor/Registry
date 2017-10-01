using System.Windows.Controls;
using Registry.UI.UserControls;

namespace Registry.UI.Extensions
{
  public static class GridExtension
  {
    public static void OpenUserControlWithSignOut(this Grid grid, UserControl userControl)
    {
      grid.Children.Clear();
      var authUserControl = new AuthorizedUserBackground();
      authUserControl.MainGrid.Children.Clear();
      authUserControl.MainGrid.Children.Add(userControl);
      grid.Children.Add(authUserControl);
    }

    public static void OpenUserControl(this Grid grid, UserControl userControl)
    {
      grid.Children.Clear();
      grid.Children.Add(userControl);
    }
  }
}