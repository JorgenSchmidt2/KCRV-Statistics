using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.UI.AppService;
using System.IO;
using System.Windows;

namespace KCRV_Statistics.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Directory.Exists(AppFolders.InputFiles_Simple))
            {
                Directory.CreateDirectory(AppFolders.InputFiles_Simple);
                MessageBox.Show("Папка \"Input Files Simple\" пересоздана.");
            }

            if (!Directory.Exists(AppFolders.InputFiles_XLSX))
            {
                Directory.CreateDirectory(AppFolders.InputFiles_XLSX);
                MessageBox.Show("Папка \"Input Files XLSX\" пересоздана.");
            }

            if (!Directory.Exists(AppFolders.InputFiles_CSV_JSON))
            {
                Directory.CreateDirectory(AppFolders.InputFiles_CSV_JSON);
                MessageBox.Show("Папка \"Input Files CSV-JSON\" пересоздана.");
            }

            if (!Directory.Exists(AppFolders.Reports))
            {
                Directory.CreateDirectory(AppFolders.Reports);
                MessageBox.Show("Папка \"Reports\" пересоздана.");
            }

            if (!Directory.Exists(AppFolders.Results))
            {
                Directory.CreateDirectory(AppFolders.Results);
                MessageBox.Show("Папка \"Results\" пересоздана.");
            }

            AppData.ChoisedFolders.Add(AppFolders.InputFiles_XLSX);
            AppData.AppFileData = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);

            WindowsObjects.EntryWindow = new();
            if (WindowsObjects.EntryWindow.ShowDialog() == true)
            {
                WindowsObjects.EntryWindow.Show();
            }
            else
            {
                WindowsObjects.EntryWindow = null;
                Shutdown();
                return;
            }
        }
    }
}