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
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
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

    public CreateUser()
    {
      InitializeComponent();
      _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
      PermissionCommon.Titles.ForEach(item =>
      {
        PermissionsList.Items.Add(new CheckBox
        {
          Name = item.Key.ToString(),
          Content = item.Value
        });
      });
    }

    private void BackUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
    }

    private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var permissions = new List<Permission>();
      foreach (CheckBox item in PermissionsList.Items)
      {
        if (item.IsChecked == true)
        {
          permissions.Add((Permission)Enum.Parse(typeof(Permission), item.Name));
        }
      }

      var user = await _userService.GetUser(LoginTextBox.Text);
      if (user != null)
      {
        MessageBox.Show($"User with login {user.Login} is already exist", "Duplication error", MessageBoxButton.OK, MessageBoxImage.Hand);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return;
      }

      await _userService.CreateUser(
        LoginTextBox.Text, 
        NameTextBox.Text, 
        PasswordTextBox.Password,
        permissions.ToArray());

      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new ChangeUser());
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }
  }
}