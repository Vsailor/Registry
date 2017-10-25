using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Registry.Data.Models;
using Registry.Models;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for CreateTheme.xaml
  /// </summary>
  public partial class CreateTheme : UserControl
  {
    private readonly IThemeService _themeService = RegistryCommon.Instance.Container.Resolve<IThemeService>();
    private readonly IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    private UserBasicInfo _selectedUser;

    public delegate void UnassignUser(AssignedUser userBasicInfo);
    public event UnassignUser UserUnassigned;

    private Dictionary<UserBasicInfo, Roles> _assignedUsers = new Dictionary<UserBasicInfo, Roles>();

    public CreateTheme()
    {
      InitializeComponent();
      CreateThemeButton.IsEnabled = false;
      UserUnassigned += OnUserUnassigned;
    }

    private void OnUserUnassigned(AssignedUser user)
    {
      AssignedUserListBox.Items.Remove(user);
      _assignedUsers.Remove(_assignedUsers.Single(x => x.Key.Login == user.SelectedUser.Login).Key);
      if (!_assignedUsers.Any())
      {
        CreateThemeButton.IsEnabled = false;
      }
    }


    private void UserFilterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      UsersListBox.Items.Filter = item =>
      {
        var user = (UserBasicInfo)item;
        return user.Login.ToLowerInvariant().StartsWith(UserFilterTextBox.Text.ToLowerInvariant()) ||
               user.Name.ToLowerInvariant().StartsWith(UserFilterTextBox.Text.ToLowerInvariant());
      };

      if (UsersListBox.SelectedValue != null)
      {
        CreateThemeButton.IsEnabled = true;
      }
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private async void CreateThemeButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var request = _assignedUsers.Select(u => new CreateThemeUserRequest
      {
        Role = (int)u.Value,
        UserLogin = u.Key.Login
      });

      await _themeService.CreateTheme(NameTextBox.Text, request.ToArray());

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      UsersListBox.ItemsSource = await _userService.GetAllUsers();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void UsersListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _selectedUser = (UserBasicInfo)UsersListBox.SelectedValue;
      if (_selectedUser == null)
      {
        return;
      }

      if (_assignedUsers.Any(u => u.Key.Login == _selectedUser.Login))
      {
        return;
      }

      if (!string.IsNullOrEmpty(NameTextBox.Text))
      {
        CreateThemeButton.IsEnabled = true;
      }

      _assignedUsers.Add(_selectedUser, Roles.None);
      AssignedUserListBox.Items.Add(new AssignedUser(_selectedUser, UserUnassigned));
      Thread.Sleep(200);
    }

    private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(NameTextBox.Text))
      {
        CreateThemeButton.IsEnabled = false;
        return;
      }

      CreateThemeButton.IsEnabled = true;
    }
  }
}