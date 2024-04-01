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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace testWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnReplacing_Click(object sender, RoutedEventArgs e)
        {
            //Create Object to Repalcing Window
            ReplacingWindow rwOBJ = new ReplacingWindow();
            //Close Current window
            this.Close();
            //Open Replacing Window
            rwOBJ.Show();

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnIdentifyingAreas_Click(object sender, RoutedEventArgs e)
        {
            IdentifyingAreas iaOBJ = new IdentifyingAreas();

            this.Close();

            iaOBJ.Show();
        }

        private void btnFindingCall_Click(object sender, RoutedEventArgs e)
        {
            FindingCallNumbers fcnOBJ = new FindingCallNumbers();

            this.Close();

            fcnOBJ.Show();
        }
    }
}
