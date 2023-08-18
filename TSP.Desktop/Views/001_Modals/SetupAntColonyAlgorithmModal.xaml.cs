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
using TSP.Desktop.ViewModels.TSPMap;

namespace TSP.Desktop.Views.Modals
{
    /// <summary>
    /// Interaction logic for SetupGeneticAlgorithm.xaml
    /// </summary>
    public partial class SetupAntColonyAlgorithmModal : Window
    {
        public SetupAntColonyAlgorithmModal()
        {
            InitializeComponent();
            DataContext = new SetupAntColonyAlgorithmViewModel();
        }

        private void ModalWindow_LocationChanged(object sender, EventArgs e)
        {
            ClearToolTips();
        }

        protected void ClearToolTips()
        {
            //TODO
        }
    }
}
