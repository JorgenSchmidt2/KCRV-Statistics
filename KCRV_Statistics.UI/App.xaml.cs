using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.UI.AppService;
using System.IO;
using System.Windows;

namespace KCRV_Statistics.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Проверяем все ли необходимые для работы программы директории существуют
            // Если не существуют, выводим пользователю сообщение об их пересоздании и пересоздаём их
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

            if (!Directory.Exists(AppFolders.Results))
            {
                Directory.CreateDirectory(AppFolders.Results);
                MessageBox.Show("Папка \"Results\" пересоздана.");
            }

            // По умолчанию, при открытии должна быть прожата галочка на XLSX файлах, как следствие при начале работы программы,
            // в список задействованных директорий будет включена папка с XLSX файлами
            // После получаем в статическую переменную список всех файлов оттуда.
            AppData.ChoisedFolders.Add(AppFolders.InputFiles_XLSX);
            AppData.AppFileData = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);

            // Открываем главное окно (также прописан алгоритм закрытия главного окна, при котором вся программа заканчивает работу).
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