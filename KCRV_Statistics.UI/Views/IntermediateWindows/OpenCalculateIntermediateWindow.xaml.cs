using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace KCRV_Statistics.UI.Views.IntermediateWindows
{
    /// <summary>
    /// Логика взаимодействия для OpenCalculateIntermediateWindow.xaml
    /// </summary>
    public partial class OpenCalculateIntermediateWindow : Window
    {
        public OpenCalculateIntermediateWindow()
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
