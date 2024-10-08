﻿using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Core.Entities.FileSystemEntites;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.DataOperatorsService.Lists;
using KCRV_Statistics.Model.DirectoryService.DirectoryInfoGetters;
using KCRV_Statistics.Model.FileService.Readers;
using KCRV_Statistics.Model.SearchService.FileFinders;
using KCRV_Statistics.Model.ValidateService.SimpleFileCheckers;
using KCRV_Statistics.UI.AppService;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// Содержит запрос к файловой системе.
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
                        // Проверка введено ли что-то в поле запроса, если нет - выдаётся сообщение об ошибке, а выполнение прерывается
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
                        FileDatas = ListOperators.CopyFileDataListEntities(AppData.AppFileData);
                    }
                );
            }
        }

        /// <summary>
        /// Обновляет информацию о содержимом в папках с изменением AppData.AppFileData.
        /// </summary>
        public Command Update
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        // Доработать метод с целью универсализации (другой кнопке будет дан немного отличающийся функционал)
                        UpdateFileInfo();
                    }
                );
            }
        }

        #endregion

        #region Список файлов и директорий

        public List<ViewedDirectoryData> directoryDataEntities = AppData.AppDirectoryData;
        /// <summary>
        /// Отображает список доступных директорий.
        /// </summary>
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

        /// <summary>
        /// Временная кнопка цель которой - помочь отобразить на экране изменения, внесённые пользователем в интерфейсе приложения.
        /// </summary>
        public Command ConfirmDirChoise
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        // В дальнейшем кнопка будет удалена
                        // Доработать метод с целью универсализации (другой кнопке будет дан немного отличающийся функционал)
                        UpdateFileInfo();
                    }
                );
            }
        }

        /// <summary>
        /// Временная кнопка цель которой - помочь понять содержимое каких папок отображено на экране компьютера на текущий момент времени.
        /// </summary>
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

        public List<FileDataEntity> fileDatas = ListOperators.CopyFileDataListEntities(AppData.AppFileData);
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
        private readonly string ChoisePartErrorMessage = "Должен быть выбран хотя-бы один вариант выбираемого формата файлов.";

        public bool xlsx_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у xlsx
        /// </summary>
        public bool XLSX_Check
        {
            get 
            { 
                return xlsx_Check; 
            }
            set
            {
                if (value == false && !(JSON_Check || CSV_Check || TXT_Check)) 
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    xlsx_Check = value;
                
                CheckChanges();
            }
        }

        public bool json_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у json
        /// </summary>
        public bool JSON_Check
        {
            get
            {
                return json_Check;
            }

            set
            {
                if (value == false && !(XLSX_Check || CSV_Check || TXT_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    json_Check = value;

                CheckChanges();
            }
        }

        public bool csv_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у csv
        /// </summary>
        public bool CSV_Check
        {
            get
            {
                return csv_Check;
            }

            set
            {
                if (value == false && !(XLSX_Check || JSON_Check || TXT_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    csv_Check = value;

                CheckChanges();
            }
        }

        public bool txt_Check = true;
        /// <summary>
        /// Переменная, привязанная к checkbutton'у txt
        /// </summary>
        public bool TXT_Check
        {
            get
            {
                return txt_Check;
            }
            set
            {
                if (value == false && !(XLSX_Check || JSON_Check || CSV_Check))
                    MessageBox.Show(ChoisePartErrorMessage);
                else
                    txt_Check = value;

                CheckChanges();
            }
        }

        #endregion

        #region Управление и описание программы

        public int id_Field = 0;
        /// <summary>
        /// ID файла
        /// </summary>
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

                            // Проверяем существует ли файл в директории с приложением
                            if (!File.Exists(Environment.CurrentDirectory + "\\" + fileInfo.Directory + "\\" + fileInfo.FileName))
                            {
                                var Message = "Не удалось найти файл " + fileInfo.FileName + " из директории " + fileInfo.Directory + ".\n"
                                    + "Проверьте целостность файловой структуры в корневой директории приложения.";
                                MessageBox.Show(Message);
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
            AppData.ChoisedFolders = choised_folders;

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
            List<FileDataEntity> PrimaryFileList = DirectoryInfoReader.GetFileListFromDirectory(AppData.ChoisedFolders);
            AppData.AppFileData = ListOperators.FilterFileListByExtension(PrimaryFileList, ChoisedExtensions);
            FileDatas = ListOperators.CopyFileDataListEntities(AppData.AppFileData);
        }

        #endregion
    }
}