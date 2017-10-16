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
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data;
using Registry.Models;
using Registry.Services;
using Registry.Services.Abstract;
using Registry.UI.Extensions;
using Registry.UI.UserControls.Admin;

namespace Registry.UI.UserControls
{
  public partial class Login : UserControl
  {
    private IUserService _userService;

    public Login()
    {
      InitializeComponent();
      _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    }

    private async void SignInButton_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(LoginTextBox.Text))
      {
        MessageBox.Show(
          "Login is empty",
          "Error",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = "Login is empty";
        return;
      }

      if (string.IsNullOrEmpty(PasswordTextBox.Password))
      {
        MessageBox.Show(
          "Password is empty",
          "Error",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = "Password is empty";
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Verifying;

      UserDetailedInfo result = await _userService.GetUser(LoginTextBox.Text);
      if (result == null ||
        SecurityService.Crypt(PasswordTextBox.Password) != result.Password)
      {
        MessageBox.Show(
          StatusBarState.InvalidUserNameOrPassword,
          "Wrong credentials",
          MessageBoxButton.OK,
          MessageBoxImage.Error);

        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.InvalidUserNameOrPassword;

        return;
      }

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new AdminMain());
      RegistryCommon.Instance.MainProgressBar.Text = $"Hi, {result.Name}";
    }
  }
}