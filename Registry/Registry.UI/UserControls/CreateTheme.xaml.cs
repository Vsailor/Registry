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
    public CreateTheme()
    {
      InitializeComponent();
      CreateThemeButton.IsEnabled = false;
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
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }

    private async void CreateThemeButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _themeService.CreateTheme(NameTextBox.Text, _selectedUser.Login);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Themes());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      UsersListBox.ItemsSource = await _userService.GetAllUsers();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void UnassignLeaderButton_OnClick(object sender, RoutedEventArgs e)
    {
      UsersListBox.Visibility = Visibility.Visible;
      UserFilterTextBox.Visibility = Visibility.Visible;
      UserFilterTextBox.Text = string.Empty;
      StartTypingOfLeaderAlert.Visibility = Visibility.Visible;
      AssignUserGrid.Visibility = Visibility.Collapsed;
      CreateThemeButton.IsEnabled = false;
    }

    private void UsersListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _selectedUser = (UserBasicInfo)UsersListBox.SelectedValue;
      if (_selectedUser == null)
      {
        return;
      }

      if (!string.IsNullOrEmpty(NameTextBox.Text))
      {
        CreateThemeButton.IsEnabled = true;
      }

      UsersListBox.Visibility = Visibility.Collapsed;
      UserFilterTextBox.Visibility = Visibility.Collapsed;
      StartTypingOfLeaderAlert.Visibility = Visibility.Collapsed;
      AssignUserGrid.Visibility = Visibility.Visible;

      AddedLeaderTextBlock.Text = $"{_selectedUser.Name} ({_selectedUser.Login})";
      UsersListBox.SelectedValue = null;
    }

    private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(NameTextBox.Text) ||
          AssignUserGrid.Visibility != Visibility.Visible)
      {
        CreateThemeButton.IsEnabled = false;
        return;
      }

      CreateThemeButton.IsEnabled = true;
    }
  }
}