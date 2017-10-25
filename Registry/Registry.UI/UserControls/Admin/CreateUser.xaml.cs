using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls.Admin
{
  /// <summary>
  /// Interaction logic for CreateUser.xaml
  /// </summary>
  public partial class CreateUser : UserControl
  {
    private IUserService _userService;

    public CreateUser()
    {
      InitializeComponent();
      _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    }

    private void BackUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
    }

    private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var user = await _userService.GetUser(LoginTextBox.Text);
      if (user != null)
      {
        MessageBox.Show($"Користувач з логіном {user.Login} вже існує", "Помилка", MessageBoxButton.OK, MessageBoxImage.Hand);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return;
      }

      await _userService.CreateUser(
        LoginTextBox.Text, 
        NameTextBox.Text, 
        PasswordTextBox.Password);

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }
  }
}