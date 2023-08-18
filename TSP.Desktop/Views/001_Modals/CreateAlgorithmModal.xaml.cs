using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Algorithms;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.ViewModels.TSPMap;
using TSP.Desktop.ViewModels.Validators.Algorithm;
using TSP.Desktop.Views.Utility;

namespace TSP.Desktop.Views.Modals
{
    /// <summary>
    /// Interaction logic for CreateAlgorithmModal.xaml
    /// </summary>
    public partial class CreateAlgorithmModal : Window
    {
        AlgorithmNameValidator algoNameValidator;

        public CreateAlgorithmModal()
        {
            InitializeComponent();
            DataContext = new CreateAlgorithmViewModel();
            this.algoNameValidator = new AlgorithmNameValidator();
        }

        private void bCreateAlgorithmModal_Click(object sender, RoutedEventArgs e)
        {
            ResetValidationState();
            var validated = true;

            if (!algoNameValidator.Validate(tbAlgorithmName.Text))
            {
                tbAlgorithmName.Background = Brushes.Red;
                tbAlgorithmName.ShowTooltip(FindResource("FieldCannotBeEmpty").ToString());
                validated = false;
            }

            if (!Enum.TryParse(cbAlgorithmType.SelectedValue.ToString(), out AlgorithmType algoType))
            {
                cbAlgorithmType.Background = Brushes.Red;
                cbAlgorithmType.ShowTooltip(FindResource("InvalidAlgorithmType").ToString());
                validated = false;
            }

            if (validated)
            {
                DialogResult = true;
                Close();
            }
        }

        private void ResetValidationState()
        {
            ClearToolTips();
            tbAlgorithmName.Background = Brushes.White;
        }

        private void tbAlgorithmName_KeyDown(object sender, KeyEventArgs e)
        {
            ClearToolTips();
            tbAlgorithmName.Background = Brushes.White;
        }

        private void ModalWindow_LocationChanged(object sender, EventArgs e)
        {
            ClearToolTips();
        }

        protected void ClearToolTips()
        {
            var tbAlgorithmNameToolTip = tbAlgorithmName.ToolTip as ToolTip;

            if (tbAlgorithmNameToolTip != null)
                tbAlgorithmNameToolTip.IsOpen = false;
        }
    }
}
