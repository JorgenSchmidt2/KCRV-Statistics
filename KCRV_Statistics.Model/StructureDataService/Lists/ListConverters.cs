using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;
using KCRV_Statistics.Model.MessageService.MessageBoxService;

namespace KCRV_Statistics.Model.StructureDataService.Lists
{
    public class ListConverters
    {
        public static List<RegularData> StringToRegularData (string Content)
        {
            List<RegularData> Result = new List<RegularData>();

            try
            {
                Content = Content.Replace('.', ',')
                    .Replace("\t\n", "")
                    .Replace("\r", "");

                if (Content[Content.Length - 1] == '\n')
                {
                    Content = Content.Remove(Content.Length - 1, 1);
                }

                var lines = Content.Split('\n');

                int laboratoryCounter = 0;
                foreach (var item in lines)
                {
                    laboratoryCounter += 1;
                    var columns = item.Split('\t');
                    if (Double.TryParse(columns[0], out var number_1) && Double.TryParse(columns[1], out var number_2))
                    {
                        Result.Add(new RegularData
                        {
                            LaboratoryNumber = laboratoryCounter,
                            Value = number_1,
                            Uncertanity = number_2
                        });
                    }
                }
            }
            catch (Exception e)
            {
                GetMessageBox.Show("Ошибка на стадии перевода текста из файла в соответствующий тип данных: \n" + e.Message);
                return Result;
            }

            return Result;
        }
    }
}