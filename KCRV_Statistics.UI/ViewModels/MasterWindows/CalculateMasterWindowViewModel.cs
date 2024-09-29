using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.FileService.Writers;
using KCRV_Statistics.Model.GraphicsShell;
using KCRV_Statistics.UI.AppService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace KCRV_Statistics.UI.ViewModels.MasterWindows
{
    /// <summary>
    /// Модель визуального представления для окна отображения результатов расчёта
    /// </summary>
    public class CalculateMasterWindowViewModel : NotifyPropertyChanged
    {
        #region Определение размера графика (внешняя часть)

        /// <summary>
        /// Возвращает высоту рамки для внешней области данных
        /// </summary>
        public int ExternalFrameHeight
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        /// <summary>
        /// Возвращает высоту внешней области данных
        /// </summary>
        public int ExternalCanvasHeight
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
            }
        }

        /// <summary>
        /// Возвращает ширину рамки для внешней области данных
        /// </summary>
        public int ExternalFrameWidth
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2 + 60;
            }
        }

        /// <summary>
        /// Возвращает ширину внешней области данных
        /// </summary>
        public int ExternalCanvasWidth
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 30;
            }
        }

        #endregion

        #region Определение размера графика (внутренняя часть)

        /// <summary>
        /// Возвращает высоту рамки для внутренней области данных
        /// </summary>
        public int InternalFrameHeight
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        /// <summary>
        /// Возвращает высоту внутренней области данных
        /// </summary>
        public int InternalCanvasHeight
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
            }
        }

        /// <summary>
        /// Возвращает ширину рамки для внутренней области данных
        /// </summary>
        public int InternalFrameWidth
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        /// <summary>
        /// Возвращает ширину внутренней области данных
        /// </summary>
        public int InternalCanvasWidth
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius;
            }
        }

        #endregion

        #region Область графических данных
        
        /// <summary>
        /// При инициализации окна получает данные о подписях (для внешнего окна)
        /// </summary>
        public List<TextLabelEntity> labelData = GraphicsSketchers.GetLabels(
                AppData.CurrentData.Min(x => x.Value - x.Uncertanity),
                AppData.CurrentData.Max(x => x.Value + x.Uncertanity),
                AppData.CurrentData.Count()
        );

        /// <summary>
        /// Возвращает список объектов типа Label для отображения в интерфейсе приложения
        /// </summary>
        public List<TextLabelEntity> LabelData
        {
            get
            {
                for (int i = 0; i < labelData.Count; i++)
                {
                    labelData[i].LabelMargin = ThicknessGetter.GetCoords(
                        labelData[i].X,
                        labelData[i].Y
                    );
                }
                return labelData;
            }

            set
            {
                labelData = value;
                CheckChanges();
            }
        }

        /// <summary>
        /// При инициализации окна получает данные о значениях расчётов в виде набора точек (для внутреннего окна)
        /// </summary>
        public List<PointGraphicsEntity> pointEntities = GraphicsSketchers.GetPoints(AppData.CurrentData);

        /// <summary>
        /// Возвращает список объектов типа Point для отображения в интерфейсе приложения (значения результатов лабораторий)
        /// </summary>
        public List<PointGraphicsEntity> PointEntities
        {
            get
            {
                for (int i = 0; i < pointEntities.Count; i++)
                {
                    pointEntities[i].PointMargin = ThicknessGetter.GetTranslatedCoords(
                        pointEntities[i].X,
                        pointEntities[i].Y,
                        1,
                        AppData.CurrentData.Count,
                        AppData.CurrentData.Min(x => x.Value - x.Uncertanity),
                        AppData.CurrentData.Max(x => x.Value + x.Uncertanity)
                    );
                }

                return pointEntities;
            }
            set
            {
                pointEntities = value;
                CheckChanges();
            }
        }

        public List<LineGraphicsEntity> KCRV_data = GraphicsSketchers.GetKCRV_Lines(AppData.OutputData[0], AppData.CurrentData);
        /// <summary>
        /// Для отображения значения показателя KCRV с его интервалами доверия
        /// </summary>
        public List<LineGraphicsEntity> KCRV_Data
        {
            get
            {
                return KCRV_data;
            }
            set
            {
                KCRV_data = value;
                CheckChanges();
            }
        }

        public List<LineGraphicsEntity> uncertanityData = GraphicsSketchers.GetUncertanityLines(AppData.CurrentData);
        /// <summary>
        /// Возвращает список объектов типа Line для отображения в интерфейсе приложения (значения неопределённости результатов лабораторий)
        /// </summary>
        public List<LineGraphicsEntity> UnvertanityData
        {
            get
            {
                return uncertanityData;
            }
            set
            {
                uncertanityData = value;
                CheckChanges();
            }
        }

        #endregion

        #region Данные расчётов 

        public ObservableCollection<ViewedOutputData> viewedOutputData = GraphicsShellService.GetViewedOutputData(AppData.OutputData);
        /// <summary>
        /// Для отображения полученных показателей KCRV в виде списка значений "Значение-Погрешность"
        /// </summary>
        public ObservableCollection<ViewedOutputData> ViewedOutputData
        {
            get
            {
                return viewedOutputData;
            }
            set
            {
                viewedOutputData = value;
                CheckChanges();
            }
        }

        // Нужно будет убрать, заменив на отслеживание изменений в списке через событие
        public Command ShowOnGraphic
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var truelistcount = ViewedOutputData
                                .Where(x => x.IsChoised)
                                .Select(x => x)
                                .Count();

                        if (truelistcount != 1)
                        {
                            MessageBox.Show("Должен быть выбран ровно один элемент");
                        }
                        else
                        {
                            var choisedobj = ViewedOutputData.FirstOrDefault(x => x.IsChoised);

                            KCRV_Data = GraphicsSketchers.GetKCRV_Lines(choisedobj, AppData.CurrentData);
                        }
                    }
                );
            }
        }
        #endregion

        #region Управляющие кнопки и всё что с ними связано

        public string folderName = "";
        /// <summary>
        /// Подвязка к полю ввода имени папки
        /// </summary>
        public string FolderName
        {
            get
            {
                return folderName;
            }
            set
            {
                folderName = value;
                CheckChanges();
            }
        }

        public bool mustCreateReport = true;
        /// <summary>
        /// Определяет будет ли дополнительно генерироваться отчёт
        /// </summary>
        public bool MustCreateReport
        {
            get 
            { 
                return mustCreateReport; 
            }
            set
            {
                mustCreateReport = value;
                CheckChanges();
            }
        }

        /// <summary>
        /// Подвязка к кнопке создания отчёта
        /// </summary>
        public Command Create
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (FolderName.Equals("") || String.IsNullOrEmpty(FolderName))
                        {
                            MessageBox.Show("Введите имя папки.");
                            return;
                        }

                        // Временный вариант вывода результатов.
                        string Content = "Результаты лабораторий: \n"
                               + "N\tX\tU\tE\n";

                        foreach (var Item in AppData.CurrentData)
                        {
                            Content += Item.LaboratoryNumber + "\t"
                                     + Item.Value + "\t"
                                     + Item.Uncertanity + "\t"
                                     + Item.E + "\n";
                        }

                        Content += "\nПоказатели KCRV: \n"
                                 + "Метод\tX\tU\n";
                        
                        foreach (var Item in AppData.OutputData)
                        {
                            Content += Item.MethodName + "\t"
                                     + Item.X + "\t"
                                     + Item.U + "\n";
                        }

                        if (!Directory.Exists(
                                Environment.CurrentDirectory + "\\" + FileSystemNames.ResultsFolder + "\\" + FolderName
                            )
                        )
                        {
                            Directory.CreateDirectory(
                                Environment.CurrentDirectory + "\\" + FileSystemNames.ResultsFolder + "\\" + FolderName
                            );
                        }

                        SimpleContentWriters.WriteContentToFile(
                            Content,
                            FileSystemNames.ResultsFolder + "\\" + FolderName,
                            FolderName,
                            AppFileFormats.TXT
                        );
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
                        var content = "";
                        MessageBox.Show(content);
                    }
                );
            }
        }

        public Command OutputFilesInfo
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var content = "";
                        MessageBox.Show(content);
                    }
                );
            }
        }

        public Command ReportInfo
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        var content = "";
                        MessageBox.Show(content);
                    }
                );
            }
        }

        public Command Close
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        AppData.OutputData.Clear();
                        WindowsObjects.CalculateMasterWindow.Close();
                        WindowsObjects.CalculateMasterWindow = null;
                    }    
                );
            }
        }

        #endregion
    }
}