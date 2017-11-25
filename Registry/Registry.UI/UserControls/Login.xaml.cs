using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Models;
using Registry.Services;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

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
          "Логін не заповнено",
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = "Логін не заповнено";
        return;
      }

      if (string.IsNullOrEmpty(PasswordTextBox.Password))
      {
        MessageBox.Show(
          "Пароль не заповнено",
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = "Пароль не заповнено";
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Verifying;

      SignInButton.IsEnabled = false;
      UserDetailedInfo result;
      try
      {
        result = await _userService.GetUser(LoginTextBox.Text);
      }
      finally
      {
        SignInButton.IsEnabled = true;
      }

      if (result == null ||
        SecurityService.Crypt(PasswordTextBox.Password) != result.Password ||
        !result.IsActive)
      {
        MessageBox.Show(
          StatusBarState.InvalidUserNameOrPassword,
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Error);

        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.InvalidUserNameOrPassword;

        return;
      }

      RegistryCommon.Instance.Login = result.Login;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
      RegistryCommon.Instance.MainProgressBar.Text = $"Привiт, {result.Name}";
    }
  }
}