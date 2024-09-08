using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.GraphicsShell;
using KCRV_Statistics.UI.AppService;
using System.Collections.Generic;
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
                AppData.CurrentData.Min(x => x.Value),
                AppData.CurrentData.Max(x => x.Value),
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
        /// Возвращает список объектов типа Point для отображения в интерфейсе приложения
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
                        AppData.CurrentData.Min(x => x.Value),
                        AppData.CurrentData.Max(x => x.Value)
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

        #endregion

        #region Данные расчётов (ожидается смена способа отображения)

        public string Mean_X
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.Mean)).X.ToString();
            }
        }

        public string Mean_U
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.Mean)).U.ToString();
            }
        }

        public string WeightedMean_X
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.WeightedMean)).X.ToString();
            }
        }

        public string WeightedMean_U
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.WeightedMean)).U.ToString();
            }
        }

        public string Median_X
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.Median)).X.ToString();
            }
        }

        public string Median_U
        {
            get
            {
                return AppData.OutputData.FirstOrDefault(x => x.MethodName.Equals(KCRV_MethodsNames.Median)).U.ToString();
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