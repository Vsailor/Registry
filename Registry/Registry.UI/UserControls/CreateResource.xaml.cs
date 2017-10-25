using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Registry.Common;
using Registry.Data.Models;
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
      throw new NotImplementedException();
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
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saving;

      var request = new CreateResourceRequest
      {
        Name = ResourceTitle.Text,
        Description = ResourceDescription.Text,
        Tags = ResourceTags.Text,
        OwnerLogin = RegistryCommon.Instance.Login
      };

      try
      {
        using (var fileStream = new FileStream(FileNameTextBox.Text, FileMode.Open))
        {
          request.FileName = FileNameTextBox.Text.Substring(FileNameTextBox.Text.LastIndexOf("\\", StringComparison.Ordinal));
          request.Url = await _resourceService.UploadToBlob(fileStream, $"{(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString(CultureInfo.InvariantCulture)}_{request.FileName}");
        }

        await _resourceService.CreateResource(request);

        RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new Resources());
        RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
      }

    }
  }
}
