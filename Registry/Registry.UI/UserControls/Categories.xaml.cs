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
  /// <summary>
  /// Interaction logic for Categories.xaml
  /// </summary>
  public partial class Categories : UserControl
  {
    private readonly ICategoryService _categoryService = RegistryCommon.Instance.Container.Resolve<ICategoryService>();

    public Categories()
    {
      InitializeComponent();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UserControls.MainMenu());
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      await LoadCategories();
    }

    private async Task LoadCategories()
    {
      CategoriesTree.Items.Clear();
      CategoryNameTextBox.Clear();
      NewSubcategoryTextBox.Clear();

      CategoryNameTextBox.IsEnabled = false;
      NewSubcategoryTextBox.IsEnabled = false;
      UpdateCategory.IsEnabled = false;
      DeleteCategory.IsEnabled = false;
      AddSubCategoryButton.IsEnabled = false;

      GetAllCategoriesResult[] result = await _categoryService.GetAllCategories();
      var baseItem = result.Single(item => item.ParentId == null);
      var newTreeItem = new TreeViewItem
      {
        Header = baseItem.Name,
        Uid = baseItem.Id.ToString(),
        IsExpanded = true
      };

      CategoriesTree.Items.Add(newTreeItem);
      FillCategories(CategoriesTree.Items[0] as TreeViewItem, baseItem, result);
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

    private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      var selectedItem = ((TreeView)sender).SelectedItem;

      if (selectedItem != null)
      {
        NewSubcategoryTextBox.IsEnabled = true;
        var treeItemView = (TreeViewItem) (selectedItem);
        if (treeItemView.Items.Count == 0)
        {
          DeleteCategory.IsEnabled = true;
        }
        else
        {
          DeleteCategory.IsEnabled = false;
        }

        CategoryNameTextBox.Text = treeItemView.Header.ToString();
        CategoryNameTextBox.IsEnabled = true;
      }
    }

    private async void UpdateCategory_Click(object sender, RoutedEventArgs e)
    {
      var selectedItem = (TreeViewItem) CategoriesTree.SelectedItem;
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _categoryService.UpdateCategory(Guid.Parse(selectedItem.Uid), CategoryNameTextBox.Text);
      await LoadCategories();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
    {
      var selectedItem = (TreeViewItem)CategoriesTree.SelectedItem;
      await _categoryService.DeleteCategory(Guid.Parse(selectedItem.Uid));
      await LoadCategories();
    }

    private async void AddSubCategoryButton_Click(object sender, RoutedEventArgs e)
    {
      var selectedItem = (TreeViewItem)CategoriesTree.SelectedItem;
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      await _categoryService.CreateCategory(Guid.Parse(selectedItem.Uid), NewSubcategoryTextBox.Text);
      await LoadCategories();

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private void NewSubcategoryTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      var selectedItem = (TreeViewItem)CategoriesTree.SelectedItem;
      AddSubCategoryButton.IsEnabled = selectedItem != null && !string.IsNullOrEmpty(NewSubcategoryTextBox.Text);
    }

    private void CategoryNameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
      var selectedItem = (TreeViewItem)CategoriesTree.SelectedItem;
      UpdateCategory.IsEnabled = selectedItem != null && !string.IsNullOrEmpty(CategoryNameTextBox.Text);
    }
  }
}