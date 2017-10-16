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
    private readonly IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();

    public ChangeUserDetails(string filter, string login)
    {
      InitializeComponent();
      _login = login;
      _filter = filter;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser(_filter));
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      UserDetailedInfo user = await _userService.GetUser(_login);

      LoginTextBox.Text = user.Login;
      NameTextBox.Text = user.Name;
      IsActiveCheckBox.IsChecked = user.IsActive;
      RoleCombobox.ItemsSource = Enum.GetNames(typeof(Role));
      RoleCombobox.SelectedValue = user.Role.ToString();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private async void UpdateUserButton_Click(object sender, RoutedEventArgs e)
    {
      if (!CheckFields(LoginTextBox.Text, "Login") ||
          !CheckFields(PasswordTextBox.Password, "Password") ||
          !CheckFields(NameTextBox.Text, "Name"))
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _userService.UpdateUser(
        LoginTextBox.Text,
        NameTextBox.Text,
        PasswordTextBox.Password,
        (Role) Enum.Parse(typeof (Role), RoleCombobox.SelectedValue.ToString()),
        IsActiveCheckBox.IsChecked ?? false);

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

      await _userService.DeleteUser(LoginTextBox.Text);

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser(_filter));
    }
  }
}