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
using TSP.Desktop.ViewModels.Algorithms;

namespace TSP.Desktop.Views.Modals
{
    /// <summary>
    /// Interaction logic for SetupGeneticAlgorithm.xaml
    /// </summary>
    public partial class SetupGeneticAlgorithmModal : Window
    {
        public SetupGeneticAlgorithmModal()
        {
            InitializeComponent();
            DataContext = new SetupGeneticAlgorithmViewModel();
        }

        private void ModalWindow_LocationChanged(object sender, EventArgs e)
        {
            ClearToolTips();
        }

        protected void ClearToolTips()
        {
            //TODO
        }

        private void bSetupGeneticAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
