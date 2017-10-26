using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
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
    private GetAllUserGroupsResult[] _userGroups;
    private RadioButton _selectedRadioButton;

    private readonly IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    private readonly IResourceGroupService _resourceGroupService = RegistryCommon.Instance.Container.Resolve<IResourceGroupService>();

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

      _user = await _userService.GetUser(_login);
      _userGroups = await _userService.GetAllUserGroups();
      for (int i = 0; i < _userGroups.Length; i++)
      {
        var userGroupRadioButton = new RadioButton()
        {
          Content = _userGroups[i].Name,
          Uid = _userGroups[i].Id.ToString(),
          IsChecked = _user.GroupId == _userGroups[i].Id,
        };

        userGroupRadioButton.Checked += UserGroupRadioButtonOnChecked;
        UserGroupListView.Items.Add(userGroupRadioButton);
      }

      _selectedRadioButton = UserGroupListView.Items[0] as RadioButton;

      LoginTextBox.Text = _user.Login;
      NameTextBox.Text = _user.Name;
      IsActiveCheckBox.IsChecked = _user.IsActive;
      PasswordTextBox.Password = _user.Password;

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void UserGroupRadioButtonOnChecked(object sender, RoutedEventArgs routedEventArgs)
    {
      _selectedRadioButton = (RadioButton)sender;
    }

    private async void UpdateUserButton_Click(object sender, RoutedEventArgs e)
    {
      if (!CheckFields(LoginTextBox.Text, "Логін") ||
          !CheckFields(NameTextBox.Text, "Ім'я"))
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;
      
      await _userService.UpdateUser(
        LoginTextBox.Text,
        NameTextBox.Text,
        PasswordTextBox.Password,
        isActive : IsActiveCheckBox.IsChecked ?? false,
        cryptPassword: _user.Password != PasswordTextBox.Password,
        groupId: int.Parse(_selectedRadioButton.Uid));

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private bool CheckFields(string field, string name)
    {
      if (string.IsNullOrEmpty(field))
      {
        string errorMessage = $"{name} має бути заповненим";
        MessageBox.Show(
          errorMessage,
          "Помилка",
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
        "Ви не зможете відмінити цю дію. Ви впевнені, що хочете видалити користувача?",
        "Підтвердіть операцію",
        MessageBoxButton.YesNoCancel,
        MessageBoxImage.Asterisk);
      if (result != MessageBoxResult.Yes)
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Deleting;

      await _userService.DeleteUser(LoginTextBox.Text);

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser(_filter));
    }
  }
}