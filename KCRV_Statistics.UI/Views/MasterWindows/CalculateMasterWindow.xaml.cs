using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
