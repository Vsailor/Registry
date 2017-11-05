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
      ResourceId = int.Parse(res.Id);
      currentResource = res;
      ResourceName.Text = res.Name;
      ResourceDescription.Text = res.Description;
    }

    public int ResourceId { get; set; }

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

      using (WebClient client = new WebClient())
      {
        await Task.Run(() => 
          client.DownloadFileAsync(new Uri(currentResource.Url), $"{dlg.SelectedPath}{currentResource.FileName}"));
      }

      Process.Start(dlg.SelectedPath);
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }

    private void ResourceItemMainGrid_OnClick(object sender, RoutedEventArgs e)
    {
      RegistryCommon.Instance.MainGrid.OpenUserControlWithSignOut(new UpdateResource(_allResources.First(r => int.Parse(r.Id) == ResourceId)));
    }
  }
}
