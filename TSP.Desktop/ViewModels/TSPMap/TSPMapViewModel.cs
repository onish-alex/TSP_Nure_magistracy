using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.ViewModels.Validators;
using TSP.Desktop.Views.Modals;

namespace TSP.Desktop.ViewModels.TSPMap
{
	[AddINotifyPropertyChangedInterface]
	public class TSPMapViewModel
	{
		public ICommand ShowCreateTSPMapWindowCommand { get; set; }
		public ICommand ShowLoadTSPMapWindowCommand { get; set; }
		public ICommand ShowSaveTSPMapWindowCommand { get; set; }

		public bool MapSelected { get; private set; }
		public string SelectedMapName { get; set; }
		public bool IsMapSaved { get; private set; }

		public FontWeight SelectedMapNameFontWeight { get; private set; }

		public TSPMapViewModel()
		{
			this.ShowCreateTSPMapWindowCommand = new CommonCommand((x) => OpenCreateTSPMapWindow());
			this.ShowLoadTSPMapWindowCommand = new CommonCommand((x) => OpenLoadTSPMapWindow());
			this.ShowSaveTSPMapWindowCommand = new CommonCommand((x) => OpenSaveTSPMapWindow());

			MapManager.GetInstance().PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == "Map")
				{
					MapSelected = MapManager.GetInstance().Map != null;
					SelectedMapName = MapSelected 
						? $"{MapManager.GetInstance().Map.Name}.{ConfigurationManager.AppSettings["MaxExtension"]}*"
						: string.Empty;

					SelectedMapNameFontWeight = FontWeight.FromOpenTypeWeight(600);
				}
			};
		}

		private void OpenCreateTSPMapWindow()
		{
			var createMapModal = new CreateMapModal();
			createMapModal.ShowDialog();
		}

		private void OpenLoadTSPMapWindow()
		{
			var loadMapDialog = new OpenFileDialog()
			{
				AddExtension = true,
				Multiselect = false,
				Filter = $"{ConfigurationManager.AppSettings["MaxExtensionName"]}|*.{ConfigurationManager.AppSettings["MaxExtension"]}",
			};

			loadMapDialog.ShowDialog();
			//var stream = loadMapDialog.OpenFile();
		}

		private void OpenSaveTSPMapWindow()
		{
			var saveMapDialog = new SaveFileDialog()
			{
				FileName = SelectedMapName,
				Filter = $"{ConfigurationManager.AppSettings["MaxExtensionName"]}|*.{ConfigurationManager.AppSettings["MaxExtension"]}",
			};

			saveMapDialog.ShowDialog();
		}
	}
}
