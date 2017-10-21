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
using Registry.Data.Models;
using Registry.Models;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  public partial class Themes : UserControl
  {
    private readonly IThemeService _themeService = RegistryCommon.Instance.Container.Resolve<IThemeService>();
    public Themes()
    {
      InitializeComponent();
      DeleteTheme.IsEnabled = false;

      DeleteTheme.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.DeleteTheme);
    }

    public Themes(string filter) : this()
    {
      InitializeComponent();
      ThemeFilterTextBox.Text = filter;
    }

    private void ThemeFilterTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      ThemesListBox.Items.Filter = item =>
      {
        var theme = (GetAllThemesResult)item;
        return theme.Name.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant()) ||
               theme.Leader.ToLowerInvariant().StartsWith(ThemeFilterTextBox.Text.ToLowerInvariant());
      };

      DeleteTheme.IsEnabled = false;
    }

    private void ThemesListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DeleteTheme.IsEnabled = true;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      ThemesListBox.ItemsSource = await _themeService.GetAllThemes();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private async void DeleteTheme_OnClick(object sender, RoutedEventArgs e)
    {
      MessageBoxResult result = MessageBox.Show(
       "You will not be able to undo this action",
       "Confirm deletion",
       MessageBoxButton.YesNoCancel,
       MessageBoxImage.Asterisk);
      if (result != MessageBoxResult.Yes)
      {
        return;
      }
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var theme = (GetAllThemesResult) ThemesListBox.SelectedValue;

      await _themeService.DeleteTheme(theme.Id);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }
  }
}
