﻿using System.Windows.Controls;
using TSP.Desktop.ViewModels.Algorithms;

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
