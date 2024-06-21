using System;
using System.Windows;
using TSP.Desktop.ViewModels.Algorithms;

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
