using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Communication;
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
    private GetAllUserGroupsResult[] _userGroups;
    private RadioButton _selectedRadioButton;

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
        PasswordTextBox.Password,
        int.Parse(_selectedRadioButton.Uid),
        IsAdminRadioButton.IsChecked == true);

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      _userGroups = await _userService.GetAllUserGroups();
      for (int i = 0; i < _userGroups.Length; i++)
      {
        var userGroupRadioButton = new RadioButton()
        {
          Content = _userGroups[i].Name,
          Uid = _userGroups[i].Id.ToString(),
          IsChecked = _userGroups[i].Id == 1
        };

        userGroupRadioButton.Checked += UserGroupRadioButtonOnChecked;
        UserGroupListView.Items.Add(userGroupRadioButton);
      }

      _selectedRadioButton = UserGroupListView.Items[0] as RadioButton;

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void UserGroupRadioButtonOnChecked(object sender, RoutedEventArgs e)
    {
      _selectedRadioButton = (RadioButton)sender;
    }
  }
}