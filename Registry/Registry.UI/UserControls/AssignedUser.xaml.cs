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
using Registry.Common;
using Registry.Models;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for AssignedUser.xaml
  /// </summary>
  public partial class AssignedUser : UserControl
  {
    public UserBasicInfo SelectedUser { get; set; }
    public Roles Role { get; set; }

    private CreateTheme.UnassignUser _onUserUnassigned;
    public AssignedUser(UserBasicInfo selectedUser, CreateTheme.UnassignUser onUserUnassigned)
    {
      InitializeComponent();
      SelectedUser = selectedUser;
      UserTextBlock.Text = $"{SelectedUser.Name} ({SelectedUser.Login})";
      RoleCombobox.ItemsSource = Enum.GetNames(typeof (Roles));
      _onUserUnassigned = onUserUnassigned;
    }

    private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
    {
      _onUserUnassigned(this);
    }

    private void RoleCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Role = (Roles)Enum.Parse(typeof (Roles), RoleCombobox.SelectedValue.ToString());
    }
  }
}
