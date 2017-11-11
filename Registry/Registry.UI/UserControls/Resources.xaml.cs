using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    private const int ResourcesPerLoad = 50;
    private int lastResId = -1;
    private GetAllResourcesResult[] _allResources;

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
      _allResources = await _resourceService.GetAllResources(ResourcesPerLoad, lastResId);

      foreach (var res in _allResources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res, _allResources));
      }

      var lastRes = _allResources.LastOrDefault();
      lastResId = lastRes == null ? -1 : int.Parse(lastRes.Id);

      if (_allResources.Length == ResourcesPerLoad)
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

      _allResources = await _resourceService.GetAllResources(ResourcesPerLoad, null);
      foreach (var res in _allResources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res, _allResources));
      }

      var lastRes = _allResources.LastOrDefault();
      lastResId = lastRes == null ? -1 : int.Parse(lastRes.Id);

      if (_allResources.Length == ResourcesPerLoad)
      {
        ResourcesListBox.Items.Add(_loadNextResources);
      }

      ResourcesListBox.SelectionChanged += ResourcesListBoxOnSelectionChanged;

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

    private void ResourcesListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
    {
     
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

    private async void UseFiltersButton_Click(object sender, RoutedEventArgs e)
    {
      var request = new UseFiltersRequest();

      request.Name = NameTextBox.Text;

      if (!string.IsNullOrEmpty(UniqueIdentifier.Text))
      {
        int id;
        if (int.TryParse(UniqueIdentifier.Text, out id))
        {
          request.Id = id;
        }
        else
        {
          MessageBox.Show(
            "Унікальний ідентифікатор має не вірний формат",
            "Помилка",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
          return;
        }
      }

      if (CategoriesTree.SelectedItem != null)
      {
        request.CategoryId = Guid.Parse(((TreeViewItem) CategoriesTree.SelectedItem).Uid);
      }

      if (GroupsListBox.SelectedItems.Count != 0)
      {
        request.ResourceGroupId = _allGroups.First(x => x.Name == GroupsListBox.SelectedItems[0].ToString()).Id;
      }

      if (string.IsNullOrEmpty(TagsTextBox.Text))
      {
        request.Tags = new string[0];
      }
      else
      {
        var tags = TagsTextBox.Text.Split(',');
        for (int i = 0; i < tags.Length; i++)
        {
          int count = 0;
          for (int j = 0; j < tags[i].Length; j++)
          {
            if (tags[i][j] != ' ')
            {
              break;
            }

            count++;
          }

          tags[i] = tags[i].Remove(0, count);
        }

        request.Tags = tags;
      }

      ResourcesListBox.Items.Clear();

      var filteredResources = await _resourceService.GetResources(request, ResourcesPerLoad, -1);
      foreach (var res in filteredResources)
      {
        ResourcesListBox.Items.Add(new ResourceItem(res, _allResources));
      }
    }

    private void ClearFiltersButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Resources());
    }
  }
}
