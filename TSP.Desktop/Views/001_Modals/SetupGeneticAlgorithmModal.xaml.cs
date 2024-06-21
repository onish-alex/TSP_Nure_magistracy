using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TSP.Desktop.Common;
using TSP.Desktop.ViewModels.Algorithms;
using TSP.Desktop.ViewModels.Validators;
using TSP.Desktop.ViewModels.Validators.Algorithm;
using TSP.Desktop.Views.Utility;

namespace TSP.Desktop.Views.Modals
{
	/// <summary>
	/// Interaction logic for SetupGeneticAlgorithm.xaml
	/// </summary>
	public partial class SetupGeneticAlgorithmModal : Window
	{
		private GeneticAlgorithmPopulationSizeValidator populationSizeValidator;
		private PercentValueValidator percentValidator;

		public SetupGeneticAlgorithmModal()
		{
			InitializeComponent();
			DataContext = new SetupGeneticAlgorithmViewModel();

			populationSizeValidator = new GeneticAlgorithmPopulationSizeValidator();
			percentValidator = new PercentValueValidator();
		}

		private void ResetValidationState()
		{
			ClearToolTips();
			tbPopulationSize.Background = Brushes.White;
			tbElitePercent.Background = Brushes.White;
			tbMutationProbability.Background = Brushes.White;
		}

		private void tbPopulationSize_KeyDown(object sender, KeyEventArgs e)
		{
			ClearToolTips();
			tbPopulationSize.Background = Brushes.White;
		}

		private void tbMutationProbability_KeyDown(object sender, KeyEventArgs e)
		{
			ClearToolTips();
			tbMutationProbability.Background = Brushes.White;
		}

		private void tbElitePercent_KeyDown(object sender, KeyEventArgs e)
		{
			ClearToolTips();
			tbElitePercent.Background = Brushes.White;
		}

		private void ModalWindow_LocationChanged(object sender, EventArgs e)
		{
			ClearToolTips();
		}

		protected void ClearToolTips()
		{
			var tbPopulationSizeToolTip = tbPopulationSize.ToolTip as ToolTip;
			var tbMutationProbabilityToolTip = tbMutationProbability.ToolTip as ToolTip;
			var tbElitePercentToolTip = tbElitePercent.ToolTip as ToolTip;

			if (tbPopulationSizeToolTip != null)
				tbPopulationSizeToolTip.IsOpen = false;

			if (tbMutationProbabilityToolTip != null)
				tbMutationProbabilityToolTip.IsOpen = false;

			if (tbElitePercentToolTip != null)
				tbElitePercentToolTip.IsOpen = false;
		}

		private void bSetupGeneticAlgorithm_Click(object sender, RoutedEventArgs e)
		{
			ResetValidationState();
			var validated = true;

			if (!populationSizeValidator.Validate(tbPopulationSize.Text))
			{
				tbPopulationSize.Background = Brushes.Red;
				tbPopulationSize.ShowTooltip(string.Format(FindResource("FieldMustBeInteger").ToString(),
					populationSizeValidator.MinValue,
					populationSizeValidator.MaxValue));

				validated = false;
			}

			if (!percentValidator.Validate(tbMutationProbability.Text))
			{
				tbMutationProbability.Background = Brushes.Red;
				tbMutationProbability.ShowTooltip(FindResource("FieldMustBePercent").ToString());
				validated = false;
			}

			if (!percentValidator.Validate(tbElitePercent.Text) && chbUseElite.IsChecked.GetValueOrDefault())
			{
				tbElitePercent.Background = Brushes.Red;
				tbElitePercent.ShowTooltip(FindResource("FieldMustBePercent").ToString());
				validated = false;
			}

			if (!Enum.TryParse(cbMutationType.SelectedValue.ToString(), out MutationType mType))
			{
				cbMutationType.Background = Brushes.Red;
				cbMutationType.ShowTooltip(FindResource("InvalidMutationType").ToString());
				validated = false;
			}

			if (!Enum.TryParse(cbCrossoverType.SelectedValue.ToString(), out CrossoverType cType))
			{
				cbCrossoverType.Background = Brushes.Red;
				cbCrossoverType.ShowTooltip(FindResource("InvalidCrossoverType").ToString());
				validated = false;
			}

			if (!Enum.TryParse(cbSelectionType.SelectedValue.ToString(), out SelectionType sType))
			{
				cbSelectionType.Background = Brushes.Red;
				cbSelectionType.ShowTooltip(FindResource("InvalidSelectionType").ToString());
				validated = false;
			}

			if (validated)
			{
				DialogResult = true;
				Close();
			}
		}
	}
}
