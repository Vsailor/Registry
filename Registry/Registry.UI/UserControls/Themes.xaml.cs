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

    private void CreateTheme()
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private void ThemeFilterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      ThemesListBox.Items.Filter = item =>
      {
        var theme = (GetAllGroupsResult)item;
        return theme.Name.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant()) ||
               theme.Leader.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant());
      };
    }

    private void ThemesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      ThemesListBox.ItemsSource = await _resourceGroupService.GetAllThemes();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private async void DeleteTheme_OnClick(object sender, RoutedEventArgs e)
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

      var theme = (GetAllGroupsResult) ThemesListBox.SelectedValue;

      await _resourceGroupService.DeleteTheme(theme.Id);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private void GroupNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void DeleteGroup_OnClick(object sender, RoutedEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void UpdateGroup_OnClick(object sender, RoutedEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void AddGroupButton_OnClick(object sender, RoutedEventArgs e)
    {
      throw new System.NotImplementedException();
    }

    private void NewGroupTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      throw new System.NotImplementedException();
    }
  }
}
