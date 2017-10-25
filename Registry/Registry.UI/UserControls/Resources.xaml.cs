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
using Registry.Services.Abstract;
using Registry.UI.Extensions;
using Registry.UI.UserControls.Admin;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for Resources.xaml
  /// </summary>
  public partial class Resources : UserControl
  {
    private IResourceService _resourceService = RegistryCommon.Instance.Container.Resolve<IResourceService>();
    
    public Resources()
    {
      InitializeComponent();
      AddNewResourceButton.Visibility = RegistryCommon.Instance.CheckVisibility(Permission.CanCreateResource);
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;
    }

    private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
    }

    private void AddNewResourceButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new CreateResource());
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new MainMenu());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      GetAllResourcesResult[] result = await _resourceService.GetAllResources();
      foreach (var res in result)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res));
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }
  }
}
