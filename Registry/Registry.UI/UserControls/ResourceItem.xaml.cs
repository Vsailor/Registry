using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Registry.Common;
using Registry.Communication;
using Registry.UI.Extensions;
using UserControl = System.Windows.Controls.UserControl;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for ResourceItem.xaml
  /// </summary>
  public partial class ResourceItem : UserControl
  {
    private GetAllResourcesResult currentResource;
    private GetAllResourcesResult[] _allResources;

    public ResourceItem(GetAllResourcesResult res, GetAllResourcesResult[] allResources)
    {
      InitializeComponent();
      _allResources = allResources;
      ResourceId = res.Id;
      currentResource = res;
      ResourceName.Text = res.Name;
      ResourceDescription.Text = res.Description;
      if (res.FileName != null)
      ResourceFileName.Text = $"Файл ресурсу: {res.FileName.Substring(1)}";
    }

    public string ResourceId { get; set; }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
      var dlg = new FolderBrowserDialog
      {
        ShowNewFolderButton = true
      };

      DialogResult result = dlg.ShowDialog();
      if (result != DialogResult.OK)
      {
        return;
      }

      await DownloadFile(dlg.SelectedPath, currentResource.FileName);

      Process.Start(dlg.SelectedPath);

      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private async Task DownloadFile(string path, string fileName)
    {
      using (WebClient client = new WebClient())
      {
        await Task.Run(() =>
          client.DownloadFileAsync(new Uri(currentResource.Url), $"{path}{fileName}"));
      }
    }

    private void ResourceItemMainGrid_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UpdateResource(_allResources.First(r => r.Id == ResourceId)));
    }

    private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
    {
      string fileName = currentResource.FileName.Insert(currentResource.FileName.LastIndexOf('.'), $"-{Guid.NewGuid().ToString("N")}");
      await DownloadFile(RegistryCommon.Instance.CacheDirectory, fileName);
      Process.Start($"{RegistryCommon.Instance.CacheDirectory}{fileName}");
    }
  }
}
