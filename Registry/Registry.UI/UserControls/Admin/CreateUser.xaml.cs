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
      RoleCombobox.ItemsSource = Enum.GetNames(typeof (Role));
      _userService = RegistryCommon.Instance.Container.Resolve<IUserService>();
    }

    private void BackUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserControls.MainMenu());
    }

    private async void CreateUserButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _userService.CreateUser(
        LoginTextBox.Text, 
        NameTextBox.Text, 
        PasswordTextBox.Password,
        (Role)Enum.Parse(typeof(Role), RoleCombobox.SelectedValue.ToString()));

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }
  }
}