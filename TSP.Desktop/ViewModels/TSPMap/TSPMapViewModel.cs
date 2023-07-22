using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Configuration;

using System.Windows;
using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.Models.Managers;
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
				if (Enum.TryParse(args.PropertyName, out MapState state))
				{
					switch (state)
					{
						case MapState.MapCreated:
							MapSelected = MapManager.GetInstance().Map != null;
							SelectedMapName = MapSelected
								? $"{MapManager.GetInstance().Map.Name}.{ConfigurationManager.AppSettings["MaxExtension"]}*"
								: string.Empty;

							SelectedMapNameFontWeight = FontWeight.FromOpenTypeWeight(600);
							IsMapSaved = false;
							break;

						case MapState.MapLoaded:
							MapSelected = MapManager.GetInstance().Map != null;

							SelectedMapName = MapSelected
								? $"{MapManager.GetInstance().Map.Name}.{ConfigurationManager.AppSettings["MaxExtension"]}"
								: string.Empty;

							SelectedMapNameFontWeight = FontWeight.FromOpenTypeWeight(100);
							break;

						case MapState.MapSaved:
							SelectedMapNameFontWeight = FontWeight.FromOpenTypeWeight(100);
							SelectedMapName = SelectedMapName.TrimEnd('*');
							IsMapSaved = true;
							break;
					}
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

			if (loadMapDialog.ShowDialog().GetValueOrDefault())
				MapManager.GetInstance().LoadMap(loadMapDialog.FileName);
		}

		private void OpenSaveTSPMapWindow()
		{
			var saveMapDialog = new SaveFileDialog()
			{
				FileName = SelectedMapName.TrimEnd('*'),
				Filter = $"{ConfigurationManager.AppSettings["MaxExtensionName"]}|*.{ConfigurationManager.AppSettings["MaxExtension"]}",
			};

			if (saveMapDialog.ShowDialog().GetValueOrDefault())
				MapManager.GetInstance().SaveMap(saveMapDialog.FileName);
		}
	}
}
