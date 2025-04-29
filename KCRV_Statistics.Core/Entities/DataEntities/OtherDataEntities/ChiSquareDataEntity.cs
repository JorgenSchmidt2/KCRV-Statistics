using KCRV_Statistics.Core.Entities.DataEntities.RegularDataUnits;

namespace KCRV_Statistics.Core.Entities.DataEntities.OtherDataEntities
{
    public class ChiSquareDataEntity
    {
        private double ChiSquareValue { get; set; }
        private List<ChiData> ChiDataList { get; set; }

        private class ChiData
        {
            internal double DataValue { get; set; }
            internal double Uncertanity { get; set; }
            internal double ChiSquareValue { get; set; }
        }

        public ChiSquareDataEntity(List<RegularData> InputData)
        {
            ChiDataList = new List<ChiData>();
            foreach (var Item in InputData)
                ChiDataList.Add(
                    new ChiData { DataValue = Item.Value, Uncertanity = Item.Uncertanity });
        }

        public void CalculateAll(double EstimateValue)
        {
            double ChiSquare = 0;
            foreach (var Item in ChiDataList)
            {
                var value = Math.Pow(Item.DataValue - EstimateValue, 2) / Math.Pow(Item.Uncertanity, 2);
                Item.ChiSquareValue = value;
                ChiSquare += value;
            }
            ChiSquareValue = ChiSquare;
        }

        public void CheckAndDo(double CriticalValue, out bool IsEqualsCriticalAndChiSquare)
        {
            IsEqualsCriticalAndChiSquare = ChiSquareValue <= CriticalValue;
            if (!IsEqualsCriticalAndChiSquare)
            {
                var element = ChiDataList.MaxBy(x => x.ChiSquareValue);
                if (element != null) ChiDataList.Remove(element);
            }
        }

        public int GetDataListCount() { return ChiDataList.Count; }
        public double GetChiSquare() { return ChiSquareValue; }
        public List<RegularData> GetData() 
        {  
            var Result = new List<RegularData>();

            foreach (var Item in ChiDataList)
                Result.Add(new RegularData { Value = Item.DataValue, Uncertanity = Item.Uncertanity });

            return Result;
        }
    }
}