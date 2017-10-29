using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Communication;
using Registry.Services.Abstract;
using Registry.UI.Extensions;

namespace Registry.UI.UserControls
{
  public partial class Resources : UserControl
  {
    private IResourceService _resourceService = RegistryCommon.Instance.Container.Resolve<IResourceService>();
    private readonly ICategoryService _categoryService = RegistryCommon.Instance.Container.Resolve<ICategoryService>();

    public Resources()
    {
      InitializeComponent();
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
      GetAllResourcesResult[] allResources = await _resourceService.GetAllResources();
      foreach (var res in allResources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res));
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      GetAllCategoriesResult[] allCategories = await _categoryService.GetAllCategories();
      var baseItem = allCategories.Single(item => item.ParentId == null);
      var newTreeItem = new TreeViewItem
      {
        Header = baseItem.Name,
        Uid = baseItem.Id.ToString(),
        IsExpanded = true
      };

      CategoriesTree.Items.Add(newTreeItem);
      FillCategories(CategoriesTree.Items[0] as TreeViewItem, baseItem, allCategories);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
    }

    private void FillCategories(
      TreeViewItem baseItem,
      GetAllCategoriesResult baseCategory,
      GetAllCategoriesResult[] allCategories)
    {
      allCategories.Where(item => item.ParentId == baseCategory.Id).ForEach(item =>
      {
        var newItem = new TreeViewItem
        {
          Header = item.Name,
          Uid = item.Id.ToString(),
          IsExpanded = true
        };

        baseItem.Items.Add(newItem);
        FillCategories(newItem, item, allCategories);
      });
    }

    private Task _quickSearchTask;
    private async void QuickSearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (_quickSearchTask == null || _quickSearchTask.Status == TaskStatus.RanToCompletion)
      {
        _quickSearchTask = QuickSearch();
        await _quickSearchTask;
      }
    }

    private async Task QuickSearch()
    {
      await Task.Delay(1000);
    }
  }
}
