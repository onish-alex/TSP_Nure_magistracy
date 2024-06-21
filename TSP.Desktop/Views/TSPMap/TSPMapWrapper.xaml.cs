using System.Windows.Controls;
using TSP.Desktop.ViewModels.TSPMap;

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
