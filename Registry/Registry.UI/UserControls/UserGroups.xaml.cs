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
using Registry.Communication;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for UserGroups.xaml
  /// </summary>
  public partial class UserGroups : UserControl
  {
    IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    private GetAllUserGroupsResult[] _allUserGroups;
    private GetAllUserGroupsResult _selectedGroup;

    public UserGroups()
    {
      InitializeComponent();
    }

    public UserGroups(string filter)
    {
      InitializeComponent();
      GroupsFilterTextBox.Text = filter;
    }

    private async void UserGroups_OnLoaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      _allUserGroups = await _userService.GetAllUserGroups();
      GroupsListBox.ItemsSource = _allUserGroups.Select(item => item.Name);
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void GroupsFilterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      GroupsListBox.Items.Filter = item => 
        string.IsNullOrEmpty(GroupsFilterTextBox.Text) || 
        item.ToString().ToLowerInvariant().StartsWith(GroupsFilterTextBox.Text.ToLowerInvariant());
    }

    private void GroupsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      string selectedGroup = (string)GroupsListBox.SelectedItem;
      if (selectedGroup == null)
      {
        return;
      }

      DeleteGroup.IsEnabled = true;
      _selectedGroup = _allUserGroups.Single(item => item.Name == selectedGroup);
      GroupNameTextBox.IsEnabled = true;
      GroupNameTextBox.Text = selectedGroup;
    }

    private void GroupNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(GroupNameTextBox.Text))
      {
        UpdateGroup.IsEnabled = false;
        return;
      }

      UpdateGroup.IsEnabled = true;
    }

    private async void DeleteGroup_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Deleting;

      await _userService.DeleteUserGroup(_selectedGroup.Id);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserGroups(GroupsFilterTextBox.Text));
    }

    private async void UpdateGroup_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _userService.UpdateUserGroup(new UpdateUserGroupRequest
      {
        Name = GroupNameTextBox.Text,
        Id = _selectedGroup.Id
      });

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserGroups(GroupsFilterTextBox.Text));
    }

    private void NewGroupTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(NewGroupTextBox.Text))
      {
        AddGroupButton.IsEnabled = false;
        return;
      }

      AddGroupButton.IsEnabled = true;
    }

    private async void AddGroupButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _userService.CreateUserGroup(NewGroupTextBox.Text);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserGroups(GroupsFilterTextBox.Text));
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }
  }
}
