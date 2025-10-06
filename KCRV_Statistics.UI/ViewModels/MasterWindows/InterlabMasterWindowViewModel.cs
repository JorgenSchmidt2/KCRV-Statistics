using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.AppConstants;
using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.FileService.Writers;
using KCRV_Statistics.Model.GraphicsShell;
using KCRV_Statistics.Model.MathService;
using KCRV_Statistics.UI.AppService;

namespace KCRV_Statistics.UI.ViewModels.MasterWindows
{
    /// <summary>
    /// Модель визуального представления для окна отображения результатов расчёта
    /// </summary>
    public class InterlabMasterWindowViewModel : NotifyPropertyChanged
    {
        #region Определение размера графика (внешняя часть)

        /// <summary>
        /// Возвращает высоту рамки для внешней области данных
        /// </summary>
        public int ExternalFrameHeight
        {
            get => GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
        }

        /// <summary>
        /// Возвращает высоту внешней области данных
        /// </summary>
        public int ExternalCanvasHeight
        {
            get => GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
        }

        /// <summary>
        /// Возвращает ширину рамки для внешней области данных
        /// </summary>
        public int ExternalFrameWidth
        {
            get => GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2 + 60;
        }

        /// <summary>
        /// Возвращает ширину внешней области данных
        /// </summary>
        public int ExternalCanvasWidth
        {
            get => GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 30;
        }

        #endregion

        #region Определение размера графика (внутренняя часть)

        /// <summary>
        /// Возвращает высоту рамки для внутренней области данных
        /// </summary>
        public int InternalFrameHeight
        {
            get => GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
        }

        /// <summary>
        /// Возвращает высоту внутренней области данных
        /// </summary>
        public int InternalCanvasHeight
        {
            get => GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
        }

        /// <summary>
        /// Возвращает ширину рамки для внутренней области данных
        /// </summary>
        public int InternalFrameWidth
        {
            get => GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2;
        }

        /// <summary>
        /// Возвращает ширину внутренней области данных
        /// </summary>
        public int InternalCanvasWidth
        {
            get => GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius;
        }

        #endregion

        #region Область графических данных
        
        /// <summary>
        /// При инициализации окна получает данные о подписях (для внешнего окна)
        /// </summary>
        public List<TextLabelEntity> labelData = GraphicsSketchers.GetSortedLabels(
                AppData.CurrentData
        );

        /// <summary>
        /// Возвращает список объектов типа Label для отображения в интерфейсе приложения
        /// </summary>
        public List<TextLabelEntity> LabelData
        {
            get
            {
                if (labelData.Count != 0)
                {
                    for (int i = 0; i < labelData.Count; i++)
                    {
                        labelData[i].LabelMargin = ThicknessGetter.GetCoords(
                            labelData[i].X,
                            labelData[i].Y
                        );
                    }
                }
                return labelData;
            }

            set
            {
                if (value.Count != 0)
                {
                    labelData = value;
                    CheckChanges();
                }
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
                if (value.Count != 0)
                {
                    pointEntities = value;
                    CheckChanges();
                }
            }
        }

        public List<LineGraphicsEntity> KCRV_data = GraphicsSketchers.GetKCRV_Lines(GetOutputData(), AppData.CurrentData);
        /// <summary>
        /// Для отображения значения показателя KCRV с его интервалами доверия
        /// </summary>
        public List<LineGraphicsEntity> KCRV_Data
        {
            get => KCRV_data;
            set
            {
                if (value.Count != 0)
                {
                    KCRV_data = value;
                    CheckChanges();
                }
            }
        }

        /// <summary>
        /// Метод для защиты окна от случайных вылетов
        /// </summary>
        private static OutputData GetOutputData()
        {
            try { return AppData.OutputData[0]; }
            catch (Exception e) { MessageBox.Show("Ошибка при получении объекта данных:\n" + e.Message); return new OutputData(); }
        }

        public List<LineGraphicsEntity> uncertanityData = GraphicsSketchers.GetUncertanityLines(AppData.CurrentData);
        /// <summary>
        /// Возвращает список объектов типа Line для отображения в интерфейсе приложения (значения неопределённости результатов лабораторий)
        /// </summary>
        public List<LineGraphicsEntity> UnvertanityData
        {
            get => uncertanityData; 
            set
            {
                if (value.Count != 0)
                {
                    uncertanityData = value;
                    CheckChanges();
                }
            }
        }

        #endregion

        #region Данные расчётов 

        public List<ViewedOutputData> viewedOutputDataList = GraphicsShellService.GetViewedOutputData(AppData.OutputData).Data;
        /// <summary>
        /// Для отображения полученных показателей KCRV в виде списка значений "Значение-Погрешность"
        /// </summary>
        public List<ViewedOutputData> ViewedOutputDataList
        {
            get => viewedOutputDataList;
            set
            {
                if (value.Count != 0)
                {
                    viewedOutputDataList = value;
                    CheckChanges();
                }
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
            get => folderName;
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
            get => mustCreateReport; 
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

                        var truelistcount = ViewedOutputDataList
                                .Where(x => x.IsChoised)
                                .Select(x => x)
                                .Count();

                        if (truelistcount != 1)
                        {
                            MessageBox.Show("Должен быть выбран ровно один элемент");
                            return;
                        }

                        var choised = ViewedOutputDataList.FirstOrDefault(x => x.IsChoised);
                        var sert_cha = new OutputData()
                        {
                            InterLabVariance = choised.InterLabVariance,
                            X = choised.X,
                            U = choised.U,
                        };
                        var EnList = Estimators.GetEnValues(AppData.CurrentData, sert_cha, 4);

                        // Временный вариант вывода результатов.
                        string Content = "Результаты лабораторий: \n"
                               + "N\tX\tU\tE\n";

                        foreach (var Item in EnList)
                        {
                            Content += Item.LaboratoryNumber + "\t"
                                     + Item.Value + "\t"
                                     + Item.Uncertanity + "\t"
                                     + Item.E + "\n";
                        }

                        Content += "\nПоказатели KCRV: \n"
                                 + "Метод\tX\tU\tλ\n";
                        
                        foreach (var Item in AppData.OutputData)
                        {
                            Content += Item.MethodName + "\t"
                                     + Item.X + "\t"
                                     + Item.U + "\t"
                                     + Item.InterLabVariance + "\n";
                        }

                        Content += "\nТаблица \"Промежуточные результаты\" \n";
                        var counter = 0;
                        foreach(var Item in AppData.COX_Datas)
                        {
                            counter++;
                            Content += "\nТаблица №" + counter + "\n";
                            Content += "\nN\tX\tU\tχi\n";
                            foreach (var Col in Item.Data)
                            {
                                Content += Col.LaboratoryNumber + "\t" + Col.Value + "\t" + Col.Uncertanity + "\t" + Col.ChiSquareValue + "\n";
                            }
                            Content += "\nСредневзвешенное: " + Item.EstimateValue ;
                            Content += "\nχКрит: " + Item.CriticalValue;
                            Content += "\nχВыборки: " + Item.ChiSquareSum + "\n";
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

                        MessageBox.Show("Успешно!");
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
                        // Очищение статических полей от результатов
                        AppData.OutputData.Clear();
                        AppData.COX_Datas.Clear();

                        // Закрытие окна
                        WindowsObjects.InterlabMasterWindow.Close();
                        WindowsObjects.InterlabMasterWindow = null;
                    }    
                );
            }
        }

        #endregion

        #region События

        public ICommand ChoiseResult { get; }

        public InterlabMasterWindowViewModel ()
        {
            ChoiseResult = new Command(ChoiseResultMethod);
        }

        /// <summary>
        /// Выбирает какой из результатов расчётов будет отображён на экране пользователю
        /// </summary>
        private void ChoiseResultMethod(object parameter)
        {
            try
            {
                ViewedOutputData obj = (ViewedOutputData) parameter;
                if (obj == null)
                {
                    MessageBox.Show("Апкаст к целевому объекту не удался.");
                    return;
                }

                List<ViewedOutputData> NewList = new List<ViewedOutputData>();

                foreach (var Item in ViewedOutputDataList)
                {
                    if (Item.MethodName.Equals(obj.MethodName)) 
                        NewList.Add(new ViewedOutputData { 
                            MethodName = Item.MethodName, 
                            X = Item.X,
                            U = Item.U,
                            InterLabVariance = Item.InterLabVariance,
                            IsChoised = true
                        });
                    else
                    {
                        NewList.Add(new ViewedOutputData
                        {
                            MethodName = Item.MethodName,
                            X = Item.X,
                            U = Item.U,
                            InterLabVariance = Item.InterLabVariance,
                            IsChoised = false
                        });
                    }
                }

                ViewedOutputDataList = NewList;

                var choisedobj = ViewedOutputDataList.FirstOrDefault(x => x.IsChoised);

                KCRV_Data = GraphicsSketchers.GetKCRV_Lines(choisedobj, AppData.CurrentData);
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла неустранимая ошибка:\n" + e.Message + "\n\nСообщите о проблеме разработчику.");
            }
        }

        #endregion
    }
}