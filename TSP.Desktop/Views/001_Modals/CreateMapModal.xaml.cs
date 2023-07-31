using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.ViewModels.TSPMap;
using TSP.Desktop.ViewModels.Validators.Map;
using TSP.Desktop.Views.Utility;

namespace TSP.Desktop.Views.Modals
{
    /// <summary>
    /// Interaction logic for CreateMapModal.xaml
    /// </summary>
    public partial class CreateMapModal : Window
    {
        public static string MapName { get; private set; }
        public static int MapNodeAmount { get; private set; }

        private MapNameValidator mapNameValidator;
        private MapNodeCountValidator mapNodeCountValidator;

        public CreateMapModal()
        {
            InitializeComponent();
			DataContext = new CreateTSPMapViewModel();
            mapNameValidator = new MapNameValidator();
            mapNodeCountValidator = new MapNodeCountValidator();
		}

        private void bCreateMapModal_Click(object sender, RoutedEventArgs e)
        {
            ResetValidationState();
            var validated = true;

            if (!mapNameValidator.Validate(tbMapName.Text))
            {
				tbMapName.Background = Brushes.Red; 
                tbMapName.ShowTooltip(FindResource("FieldCannotBeEmpty").ToString());
                validated = false;
            }

            if (!mapNodeCountValidator.Validate(tbMapNodeAmount.Text))
            {
				tbMapNodeAmount.Background = Brushes.Red;
                tbMapNodeAmount.ShowTooltip(
                    string.Format(FindResource("FieldMustBeInteger").ToString(), 
                    mapNodeCountValidator.MinValue,
                    mapNodeCountValidator.MaxValue));
				validated = false;
			}

            if (validated)
            {
                (DataContext as CreateTSPMapViewModel).CreateTSPMapCommand.Execute(new MapDTO()
                {
                    Name = tbMapName.Text,
                    NodeCount = int.Parse(tbMapNodeAmount.Text)
                });
                Close();
            }
		}

        private void ResetValidationState()
        {
			ClearToolTips();
			tbMapName.Background = Brushes.White;
			tbMapNodeAmount.Background = Brushes.White;
		}

		private void tbMapName_KeyDown(object sender, KeyEventArgs e)
		{
			ClearToolTips();
			tbMapName.Background = Brushes.White;
		}

		private void tbMapNodeAmount_KeyDown(object sender, KeyEventArgs e)
		{
            ClearToolTips();
            tbMapNodeAmount.Background = Brushes.White;
        }

		private void ModalWindow_LocationChanged(object sender, EventArgs e)
		{
            ClearToolTips();
		}

		protected void ClearToolTips()
		{
            var tbMapNameToolTip = tbMapName.ToolTip as ToolTip;
            var tbMapNodeAmountToolTip = tbMapNodeAmount.ToolTip as ToolTip;

            if (tbMapNameToolTip != null)
                tbMapNameToolTip.IsOpen = false;

			if (tbMapNodeAmountToolTip != null)
				tbMapNodeAmountToolTip.IsOpen = false;
		}
	}
}
