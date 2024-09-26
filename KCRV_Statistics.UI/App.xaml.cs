using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Model.DataOperatorsService.Lists;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.Model.FileService.Readers;
using KCRV_Statistics.Model.ValidateService.DirectoryCheckers;
using KCRV_Statistics.UI.AppService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Проверка существует ли конфигурационных файл
            if (!File.Exists(Environment.CurrentDirectory + "\\" + FileSystemNames.ConfigurationFile))
            {
                File.Create(Environment.CurrentDirectory + "\\" + FileSystemNames.ConfigurationFile);
                var Content = "Не найдено файла, содержащего имена используемых директорий ("
                    + FileSystemNames.ConfigurationFile + ")."
                    + "\nФайл будет пересоздан, а работа программы окончена, прежде чем снова запустить программу, заполните его, "
                    + "в противном случае работа программы снова будет завершена.";
                MessageBox.Show(Content);
                Shutdown();
                return;
            }

            // Получение содержимого файла и перевод его в ряд строк
            var FileListContent = SimpleContentReaders.GetContentFromFile("", FileSystemNames.ConfigurationFile);
            var FileList = FileListContent.Replace("\r", "").Split('\n');

            // Если в конфигурационном файле ничего не оказалось - работа программы завершается
            if (FileListContent.Equals("") || FileList.Length == 0)
            {
                MessageBox.Show("Так как указанный файл с именем " + FileSystemNames.ConfigurationFile + " оказался пуст и определял " +
                    "какие папки будут использоваться в приложении - работа программы закончится.");
                Shutdown();
                return;
            }

            // Передаём полученный ранее список строк из файла и проверяем каждую указанную в нём директорию 
            // на соответствие требованию Windodws к именованию директорий
            AppData.AppDirectoryData = CorrectDirectoryNamesGetter.GetCorrectDirectories(FileList);
            if (AppData.AppDirectoryData.Count == 0)
            {
                MessageBox.Show("Конфигурационный файл с именем " + FileSystemNames.ConfigurationFile + "не содержит имён пригодных для открытия директорий. " 
                    +"\nРабота программы окончена.");
                Shutdown();
                return;
            }

            // Проверяем каждую из принятых директорий на то, существует ли она, если какая-либо директория не существует - она пересоздаётся 
            foreach (var CurrentDir in AppData.AppDirectoryData)
            {
                if (!Directory.Exists(CurrentDir.DirectoryName))
                {
                    Directory.CreateDirectory(CurrentDir.DirectoryName);

                    var content = "Директория " + CurrentDir.DirectoryName + " пересоздана.";
                    MessageBox.Show(content);
                }

                // Определяет какие директории будут открыты изначально при запуске приложения
                if (CurrentDir.IsChoised)
                {
                    AppData.ChoisedFolders.Add(CurrentDir.DirectoryName);
                }
            }

            // Проверяем существует ли папка Results
            if (!Directory.Exists(Environment.CurrentDirectory + "\\" + FileSystemNames.ResultsFolder))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + FileSystemNames.ResultsFolder);
                MessageBox.Show("Папка " + FileSystemNames.ResultsFolder + " пересоздана.");
            }

            // Проверка папок на наличие файлов, если все папки разом не имеют ни одного файла - работа программы останавливается
            if (AppData.AppDirectoryData
                .Where(x => DirectoryInfoReader.CheckDirForEmpty(x.DirectoryName))
                .Select(x => x)
                .Count() == 0)
            {
                MessageBox.Show("Все указанные в конфигурационном файле " + FileSystemNames.ConfigurationFile + " папки оказались пусты." 
                    + "\nРабота программы окончена."
                    + "\nДля работы программы нужно чтобы как минимум одна папка содержала в себе хотя бы один файл.");
                Shutdown();
                return;
            }

            // Определяем то, файлы каких форматов будут отображены при старте программы
            List<string> ChoisedExtensions = new List<string>();
            ChoisedExtensions.Add(AppFileFormats.XLSX);
            ChoisedExtensions.Add(AppFileFormats.JSON);
            ChoisedExtensions.Add(AppFileFormats.CSV);
            ChoisedExtensions.Add(AppFileFormats.TXT);

            // Получаем первичный список всех файлов из директорий, после получаем нужные нам расширения файлов
            List<FileDataEntity> PrimaryFileList = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);
            AppData.AppFileData = ListOperators.FilterFileListByExtension(PrimaryFileList, ChoisedExtensions);

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