using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Core.Responses.DataResponses;
using KCRV_Statistics.Model.DataOperatorsService.Lists;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.Model.FileService.Readers;
using KCRV_Statistics.Model.SearchService.FileFinders;
using KCRV_Statistics.Model.ValidateService.FileCheckers;
using KCRV_Statistics.UI.AppService;

namespace KCRV_Statistics.UI.ViewModels
{
    /// <summary>
    /// Модель визуального представления для основного окна (открывается при открытии приложения).
    /// </summary>
    public class EntryWindowViewModel : NotifyPropertyChanged
    {

        #region Список файлов и директорий

        public List<ViewedDirectoryData> directoryDataEntities = AppData.AppDirectoryData;
        /// <summary>
        /// Отображает список доступных директорий.
        /// </summary>
        public List<ViewedDirectoryData> DirectoryDataEntities
        {
            get => directoryDataEntities;
            set
            {
                directoryDataEntities = value;
                CheckChanges();
            }
        }

        public List<FileDataEntity> fileDatas = ListOperators.CopyFileDataListEntities(AppData.AppFileData).Data;
        /// <summary>
        /// Отвечает за то, что будет отображено непосредственно на экране пользователю.
        /// </summary>
        public List<FileDataEntity> FileDatas
        {
            get => fileDatas;
            set 
            {
                var fileDatasResponse = ListOperators.CopyFileDataListEntities(value);
                if (fileDatasResponse.Status)
                {
                    fileDatas = fileDatasResponse.Data;
                    CheckChanges();
                }
                else MessageBox.Show(fileDatasResponse.Message);
            }
        }

        public Command OpenDirectoryWindow
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        MessageBox.Show("Hasn't implemented.");
                    }    
                );
            }
        }

        public Command OpenProgrammFolder
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var path = Environment.CurrentDirectory;
                        Process.Start("explorer.exe", path);
                    }
                );
            }
        }
        #endregion

        #region Поиск

        public string query;
        /// <summary>
        /// Содержит запрос к файловой системе.
        /// </summary>
        public string Query
        {
            get => query;

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
                        // Проверка введено ли что-то в поле запроса, если нет - выдаётся сообщение об ошибке, а выполнение прерывается
                        if (String.IsNullOrEmpty(Query))
                        {
                            MessageBox.Show("Введите запрос.");
                            return;
                        }

                        // Выполнение запроса
                        var Result = QueryMaker.DoQuery(Query, AppData.AppFileData);
                        if (Result.Data.Count == 0)
                        {
                            MessageBox.Show("Файлов, содержащих \"" + Query + "\" в своём имени не обнаружено");
                            return;
                        }

                        FileDatas = Result.Data;
                    }
                );
            }
        }

        /// <summary>
        /// Обращает отображаемое в окне до изначального вида без обновлении информации об изменениях в директориях.
        /// Происходит без изменения AppData.AppFileData.
        /// </summary>
        public Command Reset
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var FileDatasResponse = ListOperators.CopyFileDataListEntities(AppData.AppFileData);

                        if (FileDatasResponse.Status)
                            FileDatas = FileDatasResponse.Data;
                        else
                            MessageBox.Show(FileDatasResponse.Message);
                    }
                );
            }
        }
        #endregion

        #region Выбор раздела

        /// <summary>
        /// Содержит сообщение об ошибке. Вынесено в отдельное поле для удобства.
        /// </summary>
        private readonly string ChoisePartErrorMessage = "Должен быть выбран хотя-бы один вариант выбираемого формата файлов.";

        public bool xlsx_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у xlsx
        /// </summary>
        public bool XLSX_Check
        {
            get => xlsx_Check; 
            set
            {
                if (value == false && !(JSON_Check || CSV_Check || TXT_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    xlsx_Check = value;

                CheckChanges();
                UpdateFileInfo();
            }
        }

        public bool json_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у json
        /// </summary>
        public bool JSON_Check
        {
            get => json_Check;

            set
            {
                if (value == false && !(XLSX_Check || CSV_Check || TXT_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    json_Check = value;

                CheckChanges();
                UpdateFileInfo();
            }
        }

        public bool csv_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у csv
        /// </summary>
        public bool CSV_Check
        {
            get => csv_Check;

            set
            {
                if (value == false && !(XLSX_Check || JSON_Check || TXT_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    csv_Check = value;

                CheckChanges();
                UpdateFileInfo();
            }
        }

        public bool txt_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у txt
        /// </summary>
        public bool TXT_Check
        {
            get => txt_Check;
            set
            {
                if (value == false && !(XLSX_Check || JSON_Check || CSV_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    txt_Check = value;

                CheckChanges();
                UpdateFileInfo();
            }
        }

        #endregion

        #region Выбор логики работы со списком файлов

        public bool mustBeViewed = false;
        /// <summary>
        /// Определяет, что при клике на элемент, будет отображено его содержимое
        /// </summary>
        public bool MustBeViewed
        {
            get => mustBeViewed;
            set
            {
                if (!ChangeFileChoisesInProcess && value != false)
                {
                    ChangeFileChoisesInProcess = true;
                    ChangeFileChoises(value, !value, !value);
                }
                CheckChanges();
            }
        }

        public bool mustToID = false;
        /// <summary>
        /// Определяет, что при клике на элемент, его ID будет помещён в соответсвующее поле
        /// </summary>
        public bool MustToID
        {
            get => mustToID;
            set
            {
                if (!ChangeFileChoisesInProcess && value != false)
                {
                    ChangeFileChoisesInProcess = true;
                    ChangeFileChoises(!value, value, !value);
                }
                CheckChanges();
            }
        }

        public bool mustBeReaded = true;
        /// <summary>
        /// Определяет, что при клике на элемент, будет открыт файл, данные которого соответствуют этому файлу
        /// </summary>
        public bool MustBeReaded
        {
            get => mustBeReaded;
            set
            {
                if (!ChangeFileChoisesInProcess && value != false)
                {
                    ChangeFileChoisesInProcess = true;
                    ChangeFileChoises(!value, !value, value);
                }
                CheckChanges();
            }
        }

        /// <summary>
        /// Дополнительная переменная для лучшего контроля над переключениями checkbutton'ов, отвечающих за выбор логики работы со списком файлов
        /// </summary>
        private bool ChangeFileChoisesInProcess = false;

        /// <summary>
        /// Переключает checkbutton'ы, которые отвечают за выбор логики работы со списком файлов
        /// </summary>
        private void ChangeFileChoises (bool Viewed, bool ToID, bool Readed)
        {
            if (ChangeFileChoisesInProcess)
            {
                mustBeViewed = Viewed;
                MustBeViewed = mustBeViewed;
                mustToID = ToID;
                MustToID = mustToID;
                mustBeReaded = Readed;
                MustBeReaded = MustBeReaded;
                ChangeFileChoisesInProcess = false;
            }
        }

        #endregion

        #region Открытие окон

        public int id_Field = 0;
        /// <summary>
        /// ID файла
        /// </summary>
        public int ID_Field
        {
            get => id_Field;
            set
            {
                id_Field = value;
                CheckChanges();
            }
        }

        /// <summary>
        /// Находит и передаёт в нужный метод данные по файлу или внешним данным
        /// </summary>
        public Command OpenChosenData
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (IsYourselfDataFieldActive && !String.IsNullOrEmpty(YourselfData))
                        {
                            OpenYourselfData();
                            return;
                        }

                        OpenWithID();
                    }
                );
            }
        }

        private void OpenWithID ()
        {
            if (ID_Field == 0)
            {
                MessageBox.Show("Введите ID файла.");
                return;
            }
            if (ID_Field < 0)
            {
                MessageBox.Show("ID файла не может быть меньше нуля, либо равно нулю.");
                return;
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

                // Открываем файл с помощью специально заготовленного универсального void-метода
                OpenFile(fileInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e.Message + "\n\nСообщите о проблеме разработчику.");
                return;
            }
        }

        private void OpenYourselfData()
        {
            var SimpleFileDataResponse = SimpleFileDataValidator.IsTwoColumnsFormat(YourselfData);
            if (SimpleFileDataResponse.Status)
            {
                // Получаем переданный ранее контент в удобном для обработки виде, дополнительно забиваем его в статическую переменную
                var ValuesListResponse = ListConverters.StringToRegularData(YourselfData);
                if (!ValuesListResponse.Status)
                {
                    MessageBox.Show(ValuesListResponse.Message);
                    return;
                }

                OpenWindowWithData(ValuesListResponse.Data);
            }
        }
        #endregion

        #region Описание программы 

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

        #region Для внешних данных пользователя

        public string yourselfData;
        public string YourselfData
        {
            get => yourselfData;
            set 
            { 
                yourselfData = value;
                CheckChanges();
            }
        }

        public bool isYourselfDataFieldActive = true;
        public bool IsYourselfDataFieldActive
        {
            get => isYourselfDataFieldActive;
            set
            {
                isYourselfDataFieldActive = value;
                CheckChanges();
            }
        }

        #endregion

        #region События

        public ICommand SelectFileCommand { get; }
        public ICommand SelectDirectoryCommand { get; }
        //public ICommand SelectAllTextInTextBox { get; }

        public EntryWindowViewModel()
        {
            SelectFileCommand = new Command(SelectFile);
            SelectDirectoryCommand = new Command(SelectDirectory);
            //SelectAllTextInTextBox = new ActionCommand(SelectAllText);
        }

        /// <summary>
        /// Позволяет по клику на конкретный элемент списка папок, определяемых конфигурацией пользователя, отобразить её содержимое в списке файлов
        /// </summary>
        private void SelectDirectory(object parameter)
        {
            try
            {
                // Получаем данные о полученном объекте parameter в отдельную переменную obj
                ViewedDirectoryData obj = (ViewedDirectoryData) parameter; // Прямая ссылка на входной объект!!!
                if (obj == null)
                {
                    MessageBox.Show("Апкаст к целевому объекту не удался.");
                    return;
                }

                // Создаём отдельный промежуточный список, который будет содержать ин-цию о том, какие папки будут открыты и на который впоследствии будет ссылаться основной список
                List<ViewedDirectoryData> NewList = new List<ViewedDirectoryData>();

                // Перебираем список директорий, создавая заново каждый элемент списка, в новом списке выбранный пользователем элемент обязательно должен изменить статус IsChoised
                foreach (var Item in DirectoryDataEntities)
                {
                    if (Item.DirectoryName.Equals(obj.DirectoryName))
                        NewList.Add(new ViewedDirectoryData { DirectoryName = Item.DirectoryName, IsChoised = !Item.IsChoised });
                    else
                        NewList.Add(new ViewedDirectoryData { DirectoryName = Item.DirectoryName, IsChoised = Item.IsChoised });
                }

                // Если количество элементов с положительным статусом по полю IsChoised промежуточного списка оказалось равно нулю, ничего не происходит, все списки остаются без изменений
                // Если глобальный список с выбранными для отображениями директориями содержит выбранный элемент, объект, соответствующий по полю DirName удаляется из списка
                // В противном случае добавляется в список отображения директорий
                if (AppData.ChosenFolders.Contains(obj.DirectoryName) && NewList.Where(x => x.IsChoised == true).Select(x => x).Count() != 0)
                {
                    AppData.ChosenFolders.Remove(obj.DirectoryName);
                }
                else if (!AppData.ChosenFolders.Contains(obj.DirectoryName))
                {
                    AppData.ChosenFolders.Add(obj.DirectoryName);
                }
                else return;

                // Даём ссылку основному списку элементов на составленный ранее
                DirectoryDataEntities = NewList;
                UpdateFileInfo();
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e.Message + "\n\nСообщите о проблеме разработчику.");
            }
        }

        /// <summary>
        /// Позволяет по клику на конкретный элемент списка файлов, произвести какие либо данные при помощи данных, содержащихся в элементе
        /// </summary>
        private void SelectFile(object parameter)
        {
            try
            {
                FileDataEntity obj = (FileDataEntity)parameter;
                if (obj == null) throw new Exception("Апкаст к целевому объекту не удался.");

                // Определяем какое действие хочет произвести пользователь
                // В зависимости от того, какое из полей приведено в состояние true, выполняем соответствующее действие
                if (MustBeViewed)
                {
                    var splits = obj.FileName.Split(".");
                    string FileExtension = splits[splits.Length - 1];
                    if (FileExtension.Equals(AppFileFormats.TXT))
                    {
                        var Content = SimpleContentReaders.GetContentFromFile(obj.Directory, obj.FileName);
                        if (Content.Status)
                        {
                            var ValidateResponse = SimpleFileDataValidator.IsTwoColumnsFormat(Content.Data);
                            if (ValidateResponse.Status) MessageBox.Show(Content.Data);
                            else MessageBox.Show("Формат входного файла был некорректен или возникла ошибка. Описание ошибки:\n" + ValidateResponse.Message);
                        }
                        else
                        {
                            MessageBox.Show(Content.Message);
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Пока доступен только просмотр простых .txt файлов.");
                    }
                    return;
                }
                if (MustToID)
                {
                    ID_Field = obj.ID;
                    return;
                }
                if (MustBeReaded)
                {
                    OpenFile(obj);
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e.Message + "\n\nСообщите о проблеме разработчику.");
            }
        }
        #endregion

        // Под мультифункциями подразумеваются такие методы, которые могут быть использованы более, чем в одном разделе
        #region Мультифункции 

        /// <summary>
        /// Позволяет обновить информацию о имеющихся в папках файлов. Используется в разделах поиска и операций с файлами. 
        /// (потребуется доработка с целью универсализации)
        /// </summary>
        private void UpdateFileInfo ()
        {
            // Если не выбрано ни одной папки - работа метода останавливается
            if (DirectoryDataEntities.Where(x => x.IsChoised).Select(x => x).Count() == 0)
            {
                MessageBox.Show("Нужно выбрать как минимум одну папку.");
                return;
            }

            // Составляем список директорий, из которых получим файлы, которые забиваем во временную переменную
            List<string> choised_folders = new List<string>();
            foreach (var CurrentDir in DirectoryDataEntities)
            {
                if (CurrentDir.IsChoised)
                {
                    if (DirectoryInfoReader.CheckDirForEmpty(CurrentDir.DirectoryName))
                    {
                        choised_folders.Add(CurrentDir.DirectoryName);
                    }
                }
            }
            AppData.ChosenFolders = choised_folders;

            if (choised_folders.Count == 0)
            {
                MessageBox.Show("Проверьте выбранные папки на наличие в них файлов");
                return;
            }

            // Определяем форматы каких файлов будут выведены на экран
            List<string> ChoisedExtensions = new List<string>();
            if (XLSX_Check) ChoisedExtensions.Add(AppFileFormats.XLSX);
            if (JSON_Check) ChoisedExtensions.Add(AppFileFormats.JSON);
            if (CSV_Check) ChoisedExtensions.Add(AppFileFormats.CSV);
            if (TXT_Check) ChoisedExtensions.Add(AppFileFormats.TXT);

            // Через первичный список получаем все находящиеся в папках файлы, после чего фильтруем папки по их расширению
            ListDataResponse<FileDataEntity> PrimaryFileListResponse = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChosenFolders);
            if (PrimaryFileListResponse.Status)
            {
                var FileDataResponse = ListOperators.FilterFileListByExtension(PrimaryFileListResponse.Data, ChoisedExtensions);
                if (FileDataResponse.Status) 
                {
                    AppData.AppFileData = FileDataResponse.Data;
                    FileDatas = ListOperators.CopyFileDataListEntities(AppData.AppFileData).Data;
                }
                else
                {
                    MessageBox.Show(FileDataResponse.Message);
                }
            }
            else
            {
                MessageBox.Show(PrimaryFileListResponse.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void OpenFile (FileDataEntity FileData)
        {
            try
            {
                // Проверяем существует ли файл в директории с приложением
                if (!File.Exists(Environment.CurrentDirectory + "\\" + FileData.Directory + "\\" + FileData.FileName))
                {
                    var Message = "Не удалось найти файл " + FileData.FileName + " из директории " + FileData.Directory + ".\n"
                        + "Проверьте целостность файловой структуры в корневой директории приложения.";
                    MessageBox.Show(Message);
                    return;
                }

                // Получаем содержимое файла
                var splits = FileData.FileName.Split(".");
                string FileExtension = splits[splits.Length - 1];

                var ContentResponse = SimpleContentReaders.GetContentFromFile(FileData.Directory, FileData.FileName);
                if (!ContentResponse.Status)
                {
                    MessageBox.Show(ContentResponse.Message);
                    return;
                }

                if (FileExtension.Equals(AppFileFormats.TXT))
                {
                    // Проверяем содержимое файла на соответствие его формату "два столбца разделены табуляцией, строки - переносом строки"
                    // Если содержимое не соответствует вышеуказанным требованиям - выводится сообщение об ошибке, ход прерывается
                    var Validate = SimpleFileDataValidator.IsTwoColumnsFormat(ContentResponse.Data);
                    if (!Validate.Status)
                    {
                        MessageBox.Show(Validate.Message);
                        return;
                    }
                }
                else if (FileExtension.Equals(AppFileFormats.XLSX))
                {
                    MessageBox.Show("Not implemented.");
                    return;
                }
                else if (FileExtension.Equals(AppFileFormats.CSV))
                {
                    MessageBox.Show("Not implemented");
                    return;
                }
                else if (FileExtension.Equals(AppFileFormats.JSON))
                {
                    MessageBox.Show("Not implemented");
                    return;
                }
                else
                {
                    MessageBox.Show("Формат файла неизвестен.");
                    return;
                }

                // Получаем переданный ранее контент в удобном для обработки виде, дополнительно забиваем его в статическую переменную
                var ValuesList = ListConverters.StringToRegularData(ContentResponse.Data);

                OpenWindowWithData(ValuesList.Data);
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e.Message + "\n\nСообщите о проблеме разработчику.");
            }
        }

        public void OpenWindowWithData(List<RegularData> Data)
        {
            AppData.CurrentData.Clear();
            AppData.CurrentData = Data;

            // Открываем окно указания начала координат (для xlsx файла)
            WindowsObjects.OpenInterlabIntermediateWindow = new();
            if (WindowsObjects.OpenInterlabIntermediateWindow.ShowDialog() == true)
            {
                WindowsObjects.OpenInterlabIntermediateWindow.Show();
            }
        }

        #endregion
    }
}