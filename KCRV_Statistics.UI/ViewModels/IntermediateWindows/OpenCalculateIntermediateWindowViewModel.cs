using KCRV_Statistics.UI.AppService;

namespace KCRV_Statistics.UI.ViewModels.IntermediateWindows
{
    public class OpenCalculateIntermediateWindowViewModel : NotifyPropertyChanged
    {
        public int coordinateDataBegin_X;

        public int CoordinateDataBegin_X
        {
            get
            {
                return coordinateDataBegin_X;
            }
            set
            {
                coordinateDataBegin_X = value;
                CheckChanges();
            }
        }

        public int coordinateDataBegin_Y;

        public int CoordinateDataBegin_Y
        {
            get
            {
                return coordinateDataBegin_Y;
            }
            set
            {
                coordinateDataBegin_Y = value;
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

        public Command Close
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
    }
}