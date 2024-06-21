using System.Windows.Controls;
using TSP.Desktop.ViewModels.Algorithms;

namespace TSP.Desktop.Views.Algorithms
{
	/// <summary>
	/// Interaction logic for AlgorithmListItem.xaml
	/// </summary>
	public partial class AlgorithmListItem : UserControl
	{
		public AlgorithmListItem()
		{
			InitializeComponent();
			DataContext = new AlgorithmListItemViewModel();
		}
	}
}
