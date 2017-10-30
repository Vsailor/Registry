using System;
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
    private GetAllGroupsResult _selectedGroup;
    private GetAllGroupsResult[] _allGroups;
    private const int ResourcesPerLoad = 2;
    private int lastResId = -1;

    private Button _loadNextResources = new Button
    {
      HorizontalAlignment = HorizontalAlignment.Center,
      FontSize = 15,
      Width = 500,
      Content = $"Загрузити наступні {ResourcesPerLoad} ресурсів"
    };

    private readonly IResourceGroupService _resourceGroupService = RegistryCommon.Instance.Container.Resolve<IResourceGroupService>();

    public Resources()
    {
      InitializeComponent();
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;
      _loadNextResources.Click += LoadNextResourcesOnClick;
    }

    private async void LoadNextResourcesOnClick(object sender, RoutedEventArgs routedEventArgs)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;
      ResourcesListBox.Items.Remove(_loadNextResources);
      GetAllResourcesResult[] resources = await _resourceService.GetAllResources(ResourcesPerLoad, lastResId);

      foreach (var res in resources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res));
      }

      var lastRes = resources.LastOrDefault();
      lastResId = lastRes == null ? -1 : int.Parse(lastRes.Id);

      if (resources.Length == ResourcesPerLoad)
      {
        ResourcesListBox.Items.Add(_loadNextResources);
      }

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Ready;
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
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

      GetAllResourcesResult[] allResources = await _resourceService.GetAllResources(ResourcesPerLoad, null);
      foreach (var res in allResources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res));
      }

      var lastRes = allResources.LastOrDefault();
      lastResId = lastRes == null ? -1 : int.Parse(lastRes.Id);

      if (allResources.Length == ResourcesPerLoad)
      {
        ResourcesListBox.Items.Add(_loadNextResources);
      }

      _allGroups = await _resourceGroupService.GetAllResourceGroups();
      GroupsListBox.ItemsSource = _allGroups.Select(g => g.Name).ToArray();

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
    private void UseFiltersButton_Click(object sender, RoutedEventArgs e)
    {

      //if (string.IsNullOrEmpty(TagsTextBox.Text))
      //{
      //  request.Tags = new string[0];
      //}
      //else
      //{
      //  var tags = ResourceTags.Text.Split(',');
      //  for (int i = 0; i < tags.Length; i++)
      //  {
      //    int count = 0;
      //    for (int j = 0; j < tags[i].Length; j++)
      //    {
      //      if (tags[i][j] != ' ')
      //      {
      //        break;
      //      }

      //      count++;
      //    }

      //    tags[i] = tags[i].Remove(0, count);
      //  }

      //  request.Tags = tags;
      //}
    }
  }
}
