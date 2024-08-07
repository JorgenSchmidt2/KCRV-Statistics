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
    public class EntryWindowViewModel : NotifyPropertyChanged
    {
        #region Поиск

        public string query;
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

        public Command Search
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (String.IsNullOrEmpty(Query))
                        {
                            MessageBox.Show("Введите запрос.");
                            return;
                        }

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

        public Command Reset
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
                    }
                );
            }
        }

        public Command Update
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var Result = DirectoryInfoReader.GetDirectoryList(AppData.ChoisedFolders);
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

        public List<EFileData> fileDatas = AppData.AppFileData;

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

        #region Выбор раздела (отрефакторить блоки выбора раздела)

        private string ChoisePartFalseMessage = "Должен быть выбран хотя бы один вариант выбираемого формата файлов.";

        private List<string> ChangeChoises ()
        {
            List<string> Result = new List<string>();

            if (XLSX_Check)
            {
                Result.Add(AppFolders.InputFiles_XLSX);
            }

            if (JSON_Check)
            {
                Result.Add(AppFolders.InputFiles_CSV_JSON);
            }

            if (Simple_Check)
            {
                Result.Add(AppFolders.InputFiles_Simple);
            }

            return Result;
        }

        public bool xlsx_Check = true;
        public bool XLSX_Check
        {
            get 
            { 
                return xlsx_Check; 
            }
            set
            {
                if (value == false && JSON_Check == false && Simple_Check == false)
                {
                    MessageBox.Show(ChoisePartFalseMessage);
                }
                else
                {
                    xlsx_Check = value;
                    AppData.ChoisedFolders = ChangeChoises();
                    var Result = DirectoryInfoReader.GetDirectoryList(AppData.ChoisedFolders);

                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_XLSX))
                    {
                        xlsx_Check = false;
                        AppData.ChoisedFolders = ChangeChoises();
                        return;
                    }

                    AppData.AppFileData = Result;
                    FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
                }
                CheckChanges();
            }
        }

        public bool json_Check = false;

        public bool JSON_Check
        {
            get
            {
                return json_Check;
            }

            set
            {
                if (value == false && XLSX_Check == false && Simple_Check == false)
                {
                    MessageBox.Show(ChoisePartFalseMessage);
                }
                else
                {
                    json_Check = value;
                    AppData.ChoisedFolders = ChangeChoises();
                    var Result = DirectoryInfoReader.GetDirectoryList(AppData.ChoisedFolders);

                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_CSV_JSON))
                    {
                        json_Check = false;
                        AppData.ChoisedFolders = ChangeChoises();
                        return;
                    }

                    AppData.AppFileData = Result;
                    FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);
                }
                CheckChanges();
            }
        }

        public bool simple_Check = false;

        public bool Simple_Check
        {
            get
            {
                return simple_Check;
            }
            set
            {
                if (value == false && XLSX_Check == false && JSON_Check == false)
                {
                    MessageBox.Show(ChoisePartFalseMessage);
                }
                else
                {
                    simple_Check = value;
                    AppData.ChoisedFolders = ChangeChoises();
                    var Result = DirectoryInfoReader.GetDirectoryList(AppData.ChoisedFolders);

                    if (!DirectoryInfoReader.CheckDirForEmpty(AppFolders.InputFiles_Simple))
                    {
                        simple_Check = false;
                        AppData.ChoisedFolders = ChangeChoises();
                        return;
                    }

                    AppData.AppFileData = Result;
                    FileDatas = ListDataOperator.CopyFileDataListEntities(AppData.AppFileData);

                }
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

                        try
                        {
                            var fileInfo = FileDatas.Where(x => x.ID == ID_Field).Select(x => x).FirstOrDefault();
                            string Content = SimpleContentReaders.GetContentFromFile(fileInfo.Directory, fileInfo.FileName);
                            var Validate = InterlabDataSimpleChecker.CheckData(Content);
                            
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