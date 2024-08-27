using KCRV_Statistics.Core.AppConfiguration;
using KCRV_Statistics.UI.AppService;
using System.Linq;
using System.Windows;

namespace KCRV_Statistics.UI.ViewModels.MasterWindows
{
    public class CalculateMasterWindowViewModel : NotifyPropertyChanged
    {
        #region Определение размера графика (внешняя часть)

        public int ExternalFrameHeight
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        public int ExternalCanvasHeight
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
            }
        }

        public int ExternalFrameWidth
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2 + 60;
            }
        }

        public int ExternalCanvasWidth
        {
            get
            {
                return GraphicsShellConfiguration.ExternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 30;
            }
        }

        #endregion

        #region Определение размера графика (внутренняя часть)

        public int InternalFrameHeight
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        public int InternalCanvasHeight
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasHeight + GraphicsShellConfiguration.PointRadius;
            }
        }

        public int InternalFrameWidth
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius + 2;
            }
        }

        public int InternalCanvasWidth
        {
            get
            {
                return GraphicsShellConfiguration.InternalCanvasWidth + GraphicsShellConfiguration.PointRadius;
            }
        }

        #endregion

        #region Данные расчётов

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