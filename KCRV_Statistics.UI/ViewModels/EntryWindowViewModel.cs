using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.Model.FileService.Readers;
using KCRV_Statistics.Model.SearchService.FileFinders;
using KCRV_Statistics.Model.StructureDataService.Lists;
using KCRV_Statistics.Model.ValidateService.SimpleFileCheckers;
using KCRV_Statistics.UI.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KCRV_Statistics.UI.ViewModels
{
    /// <summary>
    /// Модель визуального представления для основного окна (открывается при открытии приложения).
    /// </summary>
    public class EntryWindowViewModel : NotifyPropertyChanged
    {
        #region Поиск

        
        public bool mustReadingQuery = true;
        /// <summary>
        /// Отвечает за то, будет ли при выборе раздела учитываться содержимое запроса (свойство Query).
        /// </summary>
        public bool MustReadingQuery
        {
            get 
            { 
                return mustReadingQuery; 
            }
            set 
            { 
                mustReadingQuery = value;
                CheckChanges();
            }
        }

        public string query; 
        /// <summary>
        /// Содержит запрос к файловой системе (условно).
        /// </summary>
        public string Query
        {
            get 
            { 
                return query; 
            }

            set 
            { 
                query = value; 
                CheckChanges();
            }
        }

        /// <summary>
        /// Выполнение запроса на поиск совпадений.
        /// </summary>
        public Command Search 
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        /*// Проверка введено ли что-то в поле запроса, если нет - выдаётся сообщение об ошибке, а выполнение прерывается
                        if (String.IsNullOrEmpty(Query))
                        {
                            MessageBox.Show("Введите запрос.");
                            return;
                        }

                        // Выполнение запроса
                        var Result = FileQueryMaker.DoQuery(Query, AppData.AppFileData);
                        if (Result.Count == 0)
                        {
                            MessageBox.Show("Файлов, содержащих \"" + Query + "\" в своём имени не обнаружено");
                            return;
                        }

                        FileDatas = Result;*/
                    }
                );
            }
        }

        /// <summary>
        /// Обращает отображаемое в окне до изначального вида без обновлении информации об изменениях в директориях
        /// </summary>
        public Command Reset
        {
            get
            {
                return new Command(
                    obj =>
                    {

                    }
                );
            }
        }

        /// <summary>
        /// Обновляет информацию о содержимом в папках
        /// </summary>
        public Command Update
        {
            get
            {
                return new Command(
                    obj =>
                    {

                    }
                );
            }
        }

        #endregion

        #region Список файлов и директорий

        public List<ViewedDirectoryData> directoryDataEntities = AppData.AppDirectoryData;

        public List<ViewedDirectoryData> DirectoryDataEntities
        {
            get
            {
                return directoryDataEntities;
            }
            set
            {
                directoryDataEntities = value;
                CheckChanges();
            }
        }

        public Command ConfirmDirChoise
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (DirectoryDataEntities.Where(x => x.IsChoised).Select(x => x).Count() == 0)
                        {
                            MessageBox.Show("Нужно выбрать как минимум одну папку.");
                            return;
                        }

                        List<string> choised_folders = new List<string>();
                        foreach (var CurrentDir in DirectoryDataEntities)
                        {
                            if (CurrentDir.IsChoised)
                            {
                                if (DirectoryInfoReader.CheckDirForEmpty(CurrentDir.DirectoryName))
                                {
                                    choised_folders.Add(CurrentDir.DirectoryName);
                                }
                                else
                                {
                                    MessageBox.Show("Папка " + CurrentDir.DirectoryName + " пуста.");
                                }
                            }
                        }

                        if (choised_folders.Count == 0)
                        {
                            MessageBox.Show("Проверьте выбранные папки на наличие в них файлов");
                            return;
                        }

                        AppData.ChoisedFolders = choised_folders;
                        AppData.AppFileData = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);
                        FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
                    }
                );
            }
        }

        public Command CurrentDirectories
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "Сейчас отображается содержимое следующих папок: \n";
                        foreach (var Item in AppData.ChoisedFolders)
                        {
                            Content += Item + "\n";
                        }
                        MessageBox.Show(Content);
                    }
                );
            }
        }

        /// <summary>
        /// Данные файлов
        /// </summary>
        public List<FileDataEntity> fileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
        /// <summary>
        /// Отвечает за то, что будет отображено непосредственно на экране пользователю.
        /// </summary>
        public List<FileDataEntity> FileDatas
        {
            get
            {
                return fileDatas;
            }
            set 
            { 
                fileDatas = value; 
                CheckChanges();
            }
        }
        #endregion

        #region Выбор раздела

        /// <summary>
        /// Содержит сообщение об ошибке. Вынесено в отдельное поле для удобства.
        /// </summary>
        private readonly string ChoisePartErrorMessage = "Должен быть выбран хотя бы один вариант выбираемого формата файлов.";

        public bool xlsx_Check = true;
        public bool XLSX_Check
        {
            get 
            { 
                return xlsx_Check; 
            }
            set
            {
                xlsx_Check = value;
                CheckChanges();
            }
        }

        public bool json_Check = true;
        public bool JSON_Check
        {
            get
            {
                return json_Check;
            }

            set
            {
                json_Check = value;
                CheckChanges();
            }
        }

        public bool csv_Check = true;
        public bool CSV_Check
        {
            get
            {
                return json_Check;
            }

            set
            {
                csv_Check = value;
                CheckChanges();
            }
        }

        public bool txt_Check = true;
        public bool TXT_Check
        {
            get
            {
                return txt_Check;
            }
            set
            {
                txt_Check = value;
                CheckChanges();
            }
        }

        #endregion

        #region Управление и описание программы

        public int id_Field = 0;

        public int ID_Field
        {
            get 
            { 
                return id_Field; 
            }
            set
            {
                id_Field = value;
                CheckChanges();
            }
        }

        public Command Calculate // Требуется адаптировать под открытие .xlsx и json-csv файлов
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (ID_Field == 0)
                        {
                            MessageBox.Show("Введите ID файла.");
                            return;
                        }
                        if (ID_Field < 0)
                        {
                            MessageBox.Show("ID файла не может быть меньше нуля, либо равно нулю.");
                        }

                        try
                        {
                            // Ищем в списке файл с нужным ID
                            var fileInfo = FileDatas.Where(x => x.ID == ID_Field).Select(x => x).FirstOrDefault();

                            // Если не найдено объекта с нужным айди - пользователю показывается сообщение об ошибке, ход прерывается
                            if (fileInfo == null)
                            {
                                MessageBox.Show("Элемента с таким ID (" + ID_Field + ") не обнаружено.");
                                return;
                            }

                            // Получаем содержимое файла
                            string Content = SimpleContentReaders.GetContentFromFile(fileInfo.Directory, fileInfo.FileName);

                            // Проверяем содержимое файла на соответствие его формату "два столбца разделены табуляцией, строки - переносом строки"
                            // Если содержимое не соответствует вышеуказанным требованиям - выводится сообщение об ошибке, ход прерывается
                            var Validate = InterlabDataSimpleChecker.CheckSimpleData(Content);
                            if (!Validate.Status)
                            {
                                MessageBox.Show(Validate.Message);
                                return;
                            }

                            // Получаем переданный ранее контент в удобном для обработки виде, дополнительно забиваем его в статическую переменную
                            var ValuesList = ListConverters.StringToRegularData(Content);
                            AppData.CurrentData.Clear();
                            AppData.CurrentData = ValuesList;

                            // Открываем окно указания начала координат (для xlsx файла)
                            WindowsObjects.OpenCalculateIntermediateWindow = new();
                            if (WindowsObjects.OpenCalculateIntermediateWindow.ShowDialog() == true)
                            {
                                WindowsObjects.OpenCalculateIntermediateWindow.Show();
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Возникла неустранимая ошибка: \n" + e.Message);
                            return;
                        }
                    }
                );
            }
        }

        public Command ShowVisualization
        {
            get
            {
                return new Command(
                    obj => 
                    {
                        MessageBox.Show("Not Implemented.");
                    }
                );
            }
        }

        public Command ShortDescription
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "Будет добавлено по доведению программы.";
                        MessageBox.Show(Content);
                    }    
                );
            }
        }

        public Command SearchInfo
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "Будет добавлено по доведению программы.";
                        MessageBox.Show(Content);
                    }    
                );
            }
        }

        public Command ProgrammCycleInfo
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "Будет добавлено по доведению программы.";
                        MessageBox.Show(Content);
                    }    
                );
            }
        }

        public Command Help
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Content = "Будет добавлено по доведению программы.";
                        MessageBox.Show(Content);
                    }    
                );
            }
        }

        #endregion
    }
}