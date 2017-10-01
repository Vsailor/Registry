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
using Registry.Models;
using Registry.Permissions;
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
      RegistryCommon.Instance.MainProgressBar.Visibility = Visibility.Visible;
      UserDetailedInfo user = await _userService.GetUser(_login).ContinueWith(task =>
      {
        Dispatcher.Invoke(() =>
        {
          RegistryCommon.Instance.MainProgressBar.Visibility = Visibility.Collapsed;
        });

        return task.Result;
      });

      LoginTextBox.Text = user.Login;
      NameTextBox.Text = user.Name;
      IsActiveCheckBox.IsChecked = user.IsActive;
      RoleCombobox.ItemsSource = Enum.GetNames(typeof(Role));
      RoleCombobox.SelectedValue = user.Role.ToString();
    }
  }
}
