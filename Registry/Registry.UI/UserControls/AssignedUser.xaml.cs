using System;
using System.Windows;
using System.Windows.Controls;
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

    public AssignedUser(UserBasicInfo selectedUser)
    {
      InitializeComponent();
      SelectedUser = selectedUser;
      UserTextBlock.Text = $"{SelectedUser.Name} ({SelectedUser.Login})";
      RoleCombobox.ItemsSource = Enum.GetNames(typeof (Roles));
    }

    private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void RoleCombobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      Role = (Roles)Enum.Parse(typeof (Roles), RoleCombobox.SelectedValue.ToString());
    }
  }
}
