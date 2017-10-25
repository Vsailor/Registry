using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Registry.Common;
using Registry.Data.Models;
using UserControl = System.Windows.Controls.UserControl;

namespace Registry.UI.UserControls
{
  /// <summary>
  /// Interaction logic for ResourceItem.xaml
  /// </summary>
  public partial class ResourceItem : UserControl
  {
    private GetAllResourcesResult allResources;
    public ResourceItem(GetAllResourcesResult res)
    {
      InitializeComponent();
      allResources = res;
      ResourceName.Text = res.Name;
      ResourceDescription.Text = res.Description;
    }

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
          client.DownloadFileAsync(new Uri(allResources.Url), $"{dlg.SelectedPath}{allResources.FileName}"));
      }

      Process.Start(dlg.SelectedPath);
      RegistryCommon.Instance.MainProgressBar.Text = StatusBarState.Saved;
    }
  }
}
