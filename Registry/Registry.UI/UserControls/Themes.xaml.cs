using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  public partial class Themes : UserControl
  {
    private GetAllGroupsResult _selectedGroup;
    private GetAllGroupsResult[] _allGroups;
    private readonly IResourceGroupService _resourceGroupService = RegistryCommon.Instance.Container.Resolve<IResourceGroupService>();
    public Themes()
    {
      InitializeComponent();
    }

    public Themes(string filter) : this()
    {
      InitializeComponent();
      ThemeFilterTextBox.Text = filter;
    }

    private void ThemeFilterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      GroupsListBox.Items.Filter = item =>
      {
        var theme = (GetAllGroupsResult)item;
        return theme.Name.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant()) ||
               theme.Login.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant());
      };
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      _allGroups = await _resourceGroupService.GetAllResourceGroups();
      GroupsListBox.ItemsSource = _allGroups; 

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
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
      MessageBoxResult result = MessageBox.Show(
        "Ви не зможете відмінити цю дію. Ви впевненні, що бажаєте видалити групу ресурсів?",
        "Підтвердіть операцію",
        MessageBoxButton.YesNoCancel,
        MessageBoxImage.Asterisk);
      if (result != MessageBoxResult.Yes)
      {
        return;
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var theme = (GetAllGroupsResult)GroupsListBox.SelectedValue;

      await _resourceGroupService.DeleteResourceGroup(theme.Id);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes(ThemeFilterTextBox.Text));
    }

    private async void UpdateGroup_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;
      if (_allGroups.Any(g => g.Name == GroupNameTextBox.Text && _selectedGroup.Name != g.Name))
      {
        MessageBox.Show(
          "Група з таким іменем вже існує. Виберіть іньше ім'я",
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return;
      }

      await _resourceGroupService.UpdateResourceGroup(_selectedGroup.Id, GroupNameTextBox.Text);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes(ThemeFilterTextBox.Text));
    }

    private async void AddGroupButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;
      if (_allGroups.Any(g => g.Name == NewGroupTextBox.Text))
      {
        MessageBox.Show(
          "Група з таким іменем вже існує. Виберіть іньше ім'я",
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return;
      }

      await _resourceGroupService.CreateResourceGroup(NewGroupTextBox.Text, RegistryCommon.Instance.Login);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes(ThemeFilterTextBox.Text));
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

    private void GroupsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var selectedGroup = (GetAllGroupsResult)GroupsListBox.SelectedItem;
      if (selectedGroup == null)
      {
        return;
      }

      DeleteGroup.IsEnabled = true;
      _selectedGroup = _allGroups.Single(item => item.Id == selectedGroup.Id);
      GroupNameTextBox.IsEnabled = true;
      GroupNameTextBox.Text = selectedGroup.Name;
    }
  }
}
