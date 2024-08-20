using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
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
                        // Проверка введено ли что-то в поле запроса, если нет - выдаётся сообщение об ошибке,
                        // Выполнение прерывается
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

                        FileDatas = Result;
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
                        Query = "";
                        FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
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
                        Query = "";
                        var Result = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);
                        if (Result.Count == 0)
                        {
                            MessageBox.Show("Файлов в указанной/ых директории/ях не обнаружено");
                            return;
                        }
                        AppData.AppFileData = Result;
                        FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
                    }
                );
            }
        }

        #endregion

        #region Список файлов

        public List<EFileData> fileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);

        /// <summary>
        /// Отвечает за то, что будет отображено непосредственно на экране пользователю.
        /// </summary>
        public List<EFileData> FileDatas
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

        /// <summary>
        /// Проверяет значения дополнительных переменных, если равно true - добавляет в список соответствующую папку.
        /// </summary>
        private void ChangeChoises()
        {
            List<string> Result_Folders = new List<string>();

            if (xlsx_Check_addit)
            {
                Result_Folders.Add(AppFolders.InputFiles_XLSX);
            }

            if (json_Check_addit)
            {
                Result_Folders.Add(AppFolders.InputFiles_CSV_JSON);
            }

            if (simple_Check_addit)
            {
                Result_Folders.Add(AppFolders.InputFiles_Simple);
            }

            AppData.ChoisedFolders.Clear();
            AppData.ChoisedFolders = Result_Folders;

        }

        /// <summary>
        /// Позволяет удобно переключать при помощи дополнительных полей checkButton'ы на экране пользователя.
        /// </summary>
        private void ChangeCheckButtonsState (bool input_xlsx, bool input_json, bool input_simple)
        {
            try
            {
                // Если все checkbutton'ы оказались равны false - дополнительной переменной даём старые значения
                // Иначе 
                if (input_xlsx == false && input_json == false && input_simple == false)
                {
                    xlsx_Check_addit = XLSX_Check;
                    json_Check_addit = JSON_Check;
                    simple_Check_addit = Simple_Check;
                    MessageBox.Show(ChoisePartErrorMessage);
                }
                else
                {
                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_XLSX))
                    {
                        xlsx_Check_addit = false;
                    }
                    else
                    {
                        xlsx_Check_addit = input_xlsx;
                    }

                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_CSV_JSON))
                    {
                        json_Check_addit = false;
                    }
                    else
                    {
                        json_Check_addit = input_json;
                    }

                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_Simple))
                    {
                        simple_Check_addit = false;
                    }
                    else
                    {
                        simple_Check_addit = input_simple;
                    }

                    // Забиваем в список директорий все значения равные true, соответствующие определённой директории
                    ChangeChoises();

                    // Получаем список файлов из директорий
                    var DirResult = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders); 

                    // Даём статическому полю AppFileData ссылку на данные локальной переменной DirResult
                    // Если поле с запросом непустое - показываем только те файлы, которые имеют включения строки из Query
                    if (!String.IsNullOrEmpty(Query) && MustReadingQuery)
                    {
                        AppData.AppFileData = FileQueryMaker.DoQuery(Query, DirResult);
                    }
                    else
                    {
                        AppData.AppFileData = DirResult; 
                    }

                    // Копируем в свойство визуального представления AppFileData
                    FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData); 
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e);
            }
        }

        private bool xlsx_Check_addit = true;
        public bool xlsx_Check = true;
        public bool XLSX_Check
        {
            get 
            { 
                return xlsx_Check; 
            }
            set
            {
                ChangeCheckButtonsState(value, JSON_Check, Simple_Check);
                xlsx_Check = xlsx_Check_addit;
                CheckChanges();
            }
        }

        private bool json_Check_addit = false;
        public bool json_Check = false;
        public bool JSON_Check
        {
            get
            {
                return json_Check;
            }

            set
            {
                ChangeCheckButtonsState(XLSX_Check, value, Simple_Check);
                json_Check = json_Check_addit;
                CheckChanges();
            }
        }

        private bool simple_Check_addit = false;
        public bool simple_Check = false;
        public bool Simple_Check
        {
            get
            {
                return simple_Check;
            }
            set
            {
                ChangeCheckButtonsState(XLSX_Check, JSON_Check, value);
                simple_Check = simple_Check_addit;
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

        public Command Calculate
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
                            var fileInfo = FileDatas.Where(x => x.ID == ID_Field).Select(x => x).FirstOrDefault();
                            string Content = SimpleContentReaders.GetContentFromFile(fileInfo.Directory, fileInfo.FileName);
                            var Validate = InterlabDataSimpleChecker.CheckSimpleData(Content);
                            
                            if (!Validate.Status)
                            {
                                MessageBox.Show(Validate.Message);
                                return;
                            }

                            var ValuesList = ListConverters.StringToRegularData(Content);
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

                    }    
                );
            }
        }

        #endregion
    }
}