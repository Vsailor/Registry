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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Models;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls.Admin
{
  /// <summary>
  /// Interaction logic for ChangeUserDetails.xaml
  /// </summary>
  public partial class ChangeUserDetails : UserControl
  {
    private string _filter;
    private string _login;
    private UserDetailedInfo _user;
    private readonly IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    private readonly IThemeService _themeService = RegistryCommon.Instance.Container.Resolve<IThemeService>();

    public ChangeUserDetails(string filter, string login)
    {
      InitializeComponent();
      _login = login;
      _filter = filter;

      DeleteUserButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.DeleteUser);
      UpdateUserButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.UpdateUser);
      PermissionsList.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.UpdatePermissions);
      PermissionsTextBlock.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.UpdatePermissions);

      PermissionCommon.Titles.ForEach(item =>
      {
        PermissionsList.Items.Add(new CheckBox
        {
          Name = item.Key.ToString(),
          Content = item.Value
        });
      });
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser(_filter));
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      _user = await _userService.GetUser(_login);

      LoginTextBox.Text = _user.Login;
      NameTextBox.Text = _user.Name;
      IsActiveCheckBox.IsChecked = _user.IsActive;
      PasswordTextBox.Password = _user.Password;

      foreach (CheckBox item in PermissionsList.Items)
      {
        if (_user.Permissions.FirstOrDefault(p => p.ToString() == item.Name) != Permission.Unknown)
        {
          item.IsChecked = true;
        }
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private async void UpdateUserButton_Click(object sender, RoutedEventArgs e)
    {
      if (!CheckFields(LoginTextBox.Text, "Login") ||
          !CheckFields(NameTextBox.Text, "Name"))
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;
      
      var permissions = new List<Permission>();
      foreach (CheckBox item in PermissionsList.Items)
      {
        if (item.IsChecked == true)
        {
          permissions.Add((Permission)Enum.Parse(typeof(Permission), item.Name));
        }
      }

      await _userService.UpdateUser(
        LoginTextBox.Text,
        NameTextBox.Text,
        PasswordTextBox.Password,
        isActive : IsActiveCheckBox.IsChecked ?? false,
        cryptPassword: _user.Password != PasswordTextBox.Password,
        permissions: permissions.ToArray());

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private bool CheckFields(string field, string name)
    {
      if (string.IsNullOrEmpty(field))
      {
        string errorMessage = $"{name} can't be empty";
        MessageBox.Show(
          errorMessage,
          "Error",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = errorMessage;
        return false;
      }

      return true;
    }

    private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
    {
      MessageBoxResult result =  MessageBox.Show(
        "You will not be able to undo this action",
        "Confirm deletion",
        MessageBoxButton.YesNoCancel,
        MessageBoxImage.Asterisk);
      if (result != MessageBoxResult.Yes)
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Verifying;
      string[] themes = await _themeService.GetUserThemes(LoginTextBox.Text);
      if (themes.Any())
      {
        MessageBox.Show(
          $"This user is leader of themes: {string.Join(", ", themes)}. Please, choose another leader for that themes before deletion.",
          "Error",
          MessageBoxButton.OK,
          MessageBoxImage.Stop);

        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Deleting;

      await _userService.DeleteUser(LoginTextBox.Text);

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser(_filter));
    }
  }
}