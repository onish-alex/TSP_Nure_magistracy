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
using TSP.Desktop.ViewModels.TSPMap;
using TSP.Desktop.ViewModels.Validators;
using TSP.Desktop.Views.Modals;

namespace TSP.Desktop.Views.TSPMap
{
	/// <summary>
	/// Interaction logic for TSPMap.xaml
	/// </summary>
	public partial class TSPMapWrapper : UserControl
	{	
		public TSPMapWrapper()
		{
			InitializeComponent();
			DataContext = new TSPMapViewModel();	
		}
	}
}
