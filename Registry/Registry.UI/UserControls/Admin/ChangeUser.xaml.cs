using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Design;
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
  public partial class ChangeUser : UserControl
  {
    private readonly IUserService _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();

    public ChangeUser()
    {
      InitializeComponent();
    }

    public ChangeUser(string filter) : this()
    {
      InitializeComponent();
      UserFilterTextBox.Text = filter;
    }

    private void UserFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      UsersListBox.Items.Filter = item =>
      {
        var user = (UserBasicInfo)item;
        return user.Login.ToLowerInvariant().StartsWith(UserFilterTextBox.Text.ToLowerInvariant()) ||
               user.Name.ToLowerInvariant().StartsWith(UserFilterTextBox.Text.ToLowerInvariant());
      };
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserControls.MainMenu());
    }

    private void UsersListBox_OnSelected(object sender, RoutedEventArgs e)
    {
      if (!RegistryCommon.Instance.CheckPermissions(Permission.UpdateUser, Permission.DeleteUser))
      {
        return;
      }

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(
        new ChangeUserDetails(
          UserFilterTextBox.Text, 
          (UsersListBox.SelectedItem as UserBasicInfo).Login));
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;
      UsersListBox.ItemsSource = await _userService.GetAllUsers();
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }
  }
}
