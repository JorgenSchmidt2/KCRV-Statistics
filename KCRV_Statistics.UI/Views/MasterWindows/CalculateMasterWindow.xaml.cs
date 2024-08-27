using System.Windows;
using System.Windows.Input;

namespace KCRV_Statistics.UI.Views.MasterWindows
{
    /// <summary>
    /// Логика взаимодействия для CalculateMasterWindow.xaml
    /// </summary>
    public partial class CalculateMasterWindow : Window
    {
        public CalculateMasterWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Даёт возможность перемещать окно курсором
        /// </summary>
        private void Grid_MoveForLeftButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
