using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Model.DataOperatorsService.Lists;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.Model.FileService.Readers;
using KCRV_Statistics.Model.ValidateService.DirectoryCheckers;
using KCRV_Statistics.UI.AppService;

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
                    + "в противном случае работа программы снова будет завершена при следующем запуске.";
                MessageBox.Show(Content);
                Shutdown();
                return;
            }

            // Получение содержимого файла и перевод его в ряд строк
            var FileListContentResponse = SimpleContentReaders.GetContentFromFile("", FileSystemNames.ConfigurationFile);
            if (!FileListContentResponse.Status)
            {
                MessageBox.Show(FileListContentResponse.Message);
                Shutdown();
                return;
            }
            var FileList = FileListContentResponse.Data.Replace("\r", "").Split('\n');

            // Если в конфигурационном файле ничего не оказалось - работа программы завершается
            if (FileListContentResponse.Equals("") || FileList.Length == 0)
            {
                MessageBox.Show("Так как указанный файл с именем " + FileSystemNames.ConfigurationFile + " оказался пуст и определял " +
                    "какие папки будут использоваться в приложении - работа программы закончится.");
                Shutdown();
                return;
            }

            // Передаём полученный ранее список строк из файла и проверяем каждую указанную в нём директорию 
            // на соответствие требованию Windodws к именованию директорий
            AppData.AppDirectoryData = CorrectDirNamesGetter.GetCorrectLocalDirNames(FileList);
            if (AppData.AppDirectoryData.Count == 0)
            {
                MessageBox.Show("Конфигурационный файл с именем " + FileSystemNames.ConfigurationFile + " не содержит имён, пригодных для открытия директорий. "
                    + "\nРабота программы окончена.");
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
                    AppData.ChosenFolders.Add(CurrentDir.DirectoryName);
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

            // Получаем первичный список всех файлов из директорий, после получаем нужные нам расширения файлов (СМ)
            var PrimaryFileListResponse = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChosenFolders);
            if (!PrimaryFileListResponse.Status || PrimaryFileListResponse.Data == null)
            {
                MessageBox.Show("Ошибка на этапе получения первичного списка файлов.\n" 
                    + PrimaryFileListResponse.Message);
                Shutdown();
                return;
            }
            var FileListExtensionsResponse = ListOperators.FilterFileListByExtension(PrimaryFileListResponse.Data, ChoisedExtensions);
            if (!FileListExtensionsResponse.Status || FileListExtensionsResponse.Data == null)
            {
                MessageBox.Show("Ошибка на этапе получения первичного списка файлов.\n"
                    + FileListExtensionsResponse.Message);
                Shutdown();
                return;
            }
            AppData.AppFileData = FileListExtensionsResponse.Data;


            WindowsObjects.StartEntryWindow();
        }

    }
}