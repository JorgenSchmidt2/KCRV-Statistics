using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Core.Entities.GraphicsShellEntities;
using KCRV_Statistics.Model.MessageService.MessageBoxService;
using System.Collections.ObjectModel;

namespace KCRV_Statistics.Model.GraphicsShell
{
    public class GraphicsShellService
    {
        public static ObservableCollection<ViewedOutputData> GetViewedOutputData (List<OutputData> Data)
        {
            try
            {
                ObservableCollection<ViewedOutputData> Result = new ObservableCollection<ViewedOutputData>();
                int counter = 1;
                foreach (var item in Data)
                {
                    ViewedOutputData obj = new ViewedOutputData();
                    obj.MethodName = item.MethodName;
                    obj.X = item.X;
                    obj.U = item.U;
                    
                    if (counter == 1) 
                        obj.MethodIsChoised = true;
                    else 
                        obj.MethodIsChoised = false;

                    Result.Add(obj);

                    counter += 1;
                }
                return Result;
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка при составлении отображаемого списка элементов результатов расчётов: \n" + e.Message);
                return new ObservableCollection<ViewedOutputData>();
            }
        }
    }
}