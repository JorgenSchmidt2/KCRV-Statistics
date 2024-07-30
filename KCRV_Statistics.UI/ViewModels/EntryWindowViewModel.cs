using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.UI.AppService;
using System.Collections.Generic;
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

                    }
                );
            }
        }

        #endregion

        #region Список файлов

        public List<EFileData> fileDatas = new List<EFileData>();

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

        private string ChoisePartFalseMessage = "Должен быть выбран хотя бы один вариант выбираемого формата файлов.";

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