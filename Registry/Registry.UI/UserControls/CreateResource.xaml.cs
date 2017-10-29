using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
  /// Interaction logic for CreateResource.xaml
  /// </summary>
  public partial class CreateResource : UserControl
  {
    private readonly IResourceService _resourceService = RegistryCommon.Instance.Container.Resolve<IResourceService>();
    private readonly ICategoryService _categoryService = RegistryCommon.Instance.Container.Resolve<ICategoryService>();
    private readonly IResourceGroupService _resourceGroupService = RegistryCommon.Instance.Container.Resolve<IResourceGroupService>();
    private GetAllGroupsResult[] _allGroups;

    public CreateResource()
    {
      InitializeComponent();
    }

    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Resources());
    }

    private void CategoriesTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {

    }

    private void BrowseButton_OnClick(object sender, RoutedEventArgs e)
    {
      Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
      bool? result = dlg.ShowDialog();
      if (result == true)
      {
        FileNameTextBox.Text = dlg.FileName;
      }
    }

    private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
      SaveButton.IsEnabled = false;
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      if (!ValidateFields())
      {
        SaveButton.IsEnabled = true;
        return;
      }

      var selectedCategory = (TreeViewItem)CategoriesTree.SelectedItem;
      var resourceGroups = new List<Guid>();
      foreach (CheckBox item in GroupsListBox.Items)
      {
        if (item.IsChecked == true)
        {
          resourceGroups.Add(Guid.Parse(item.Uid));
        }
      }

      var request = new CreateResourceRequest
      {
        Name = ResourceTitle.Text,
        Description = ResourceDescription.Text,
        OwnerLogin = RegistryCommon.Instance.Login,
        CategoryId = Guid.Parse(selectedCategory.Uid),
        ResourceGroups = resourceGroups.ToArray(),
        SaveDate = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString()
      };

      if (string.IsNullOrEmpty(ResourceTags.Text))
      {
        request.Tags = new string[0];
      }
      else
      {
        var tags = ResourceTags.Text.Split(',');
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

      try
      {
        using (var fileStream = new FileStream(FileNameTextBox.Text, FileMode.Open))
        {
          request.FileName = FileNameTextBox.Text.Substring(FileNameTextBox.Text.LastIndexOf("\\", StringComparison.Ordinal));
          request.Url = await _resourceService.UploadToBlob(fileStream, $"{request.SaveDate.ToString(CultureInfo.InvariantCulture)}_{request.FileName}");
        }

        await _resourceService.CreateResource(request);

        RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Resources());
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        SaveButton.IsEnabled = true;
      }
    }

    private bool ValidateFields()
    {
      if (!File.Exists(FileNameTextBox.Text))
      {
        MessageBox.Show(
         "Шлях до файлу вибрано не вірно",
         "Помилка",
         MessageBoxButton.OK,
         MessageBoxImage.Error);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return false;
      }

      if (string.IsNullOrEmpty(ResourceTitle.Text))
      {
        MessageBox.Show(
         "Задайте ім'я ресурса",
         "Помилка",
         MessageBoxButton.OK,
         MessageBoxImage.Error);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return false;
      }

      if (CategoriesTree.SelectedItem == null)
      {
        MessageBox.Show(
          "Виберіть категорію для ресурса",
          "Помилка",
          MessageBoxButton.OK,
          MessageBoxImage.Error);
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Failed;
        return false;
      }

      return true;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Loading;

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

      _allGroups = await _resourceGroupService.GetAllResourceGroups();
      for (int i = 0; i < _allGroups.Length; i++)
      {
        GroupsListBox.Items.Add(new CheckBox
        {
          Content = _allGroups[i].Name,
          Uid = _allGroups[i].Id.ToString()
        });
      }

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
  }
}
