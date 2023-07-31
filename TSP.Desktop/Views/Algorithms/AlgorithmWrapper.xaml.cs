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
using TSP.Desktop.ViewModels.Algorithms;
using TSP.Desktop.ViewModels.TSPMap;

namespace TSP.Desktop.Views.Algorithms
{
    /// <summary>
    /// Interaction logic for AlgorithmWrapper.xaml
    /// </summary>
    public partial class AlgorithmWrapper : UserControl
    {
        public AlgorithmWrapper()
        {
            InitializeComponent();
			DataContext = new AlgorithmsViewModel();
		}
	}
}
