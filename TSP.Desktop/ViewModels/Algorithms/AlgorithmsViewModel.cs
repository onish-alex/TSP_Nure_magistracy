﻿using Microsoft.Win32;
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
using TSP.Desktop.Models.Entities;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.Views.Modals;

namespace TSP.Desktop.ViewModels.Algorithms
{
    [AddINotifyPropertyChangedInterface]
    public class AlgorithmsViewModel
    {
		public ICommand ShowCreateAlgorithmWindowCommand { get; set; }
		public ICommand ShowLoadAlgorithmWindowCommand { get; set; }
		public ICommand ShowSaveAlgorithmWindowCommand { get; set; }

		public string SelectedAlgorithmName { get; set; }
		public bool AlgorithmSelected { get; private set; }
        public bool IsAlgoSaved { get; private set; }

        public FontWeight SelectedAlgoNameFontWeight { get; private set; }

        public AlgorithmsViewModel()
		{
			this.ShowCreateAlgorithmWindowCommand = new CommonCommand((x) => OpenCreateAlgorithmWindow());
			this.ShowLoadAlgorithmWindowCommand = new CommonCommand((x) => OpenLoadAlgorithmWindow());
			this.ShowSaveAlgorithmWindowCommand = new CommonCommand((x) => OpenSaveAlgorithmWindow());

            AlgorithmManager.GetInstance().PropertyChanged += (sender, args) =>
            {
                if (Enum.TryParse(args.PropertyName, out AlgorithmState state))
                {
                    switch (state)
                    {
                        case AlgorithmState.AlgorithmCreated:
                            AlgorithmSelected = AlgorithmManager.GetInstance().Algorithm != null;
                            SelectedAlgorithmName = AlgorithmSelected
                                ? $"{AlgorithmManager.GetInstance().Algorithm.Name}.{ConfigurationManager.AppSettings["AlgoExtension"]}*"
                                : string.Empty;

                            SelectedAlgoNameFontWeight = FontWeight.FromOpenTypeWeight(600);
                            IsAlgoSaved = false;
                            break;

                        case AlgorithmState.AlgorithmLoaded:
                            AlgorithmSelected = AlgorithmManager.GetInstance().Algorithm != null;

                            SelectedAlgorithmName = AlgorithmSelected
                                ? $"{AlgorithmManager.GetInstance().Algorithm.Name}.{ConfigurationManager.AppSettings["AlgoExtension"]}"
                                : string.Empty;

                            SelectedAlgoNameFontWeight = FontWeight.FromOpenTypeWeight(100);
                            break;

                        case AlgorithmState.AlgorithmSaved:
                            SelectedAlgoNameFontWeight = FontWeight.FromOpenTypeWeight(100);
                            SelectedAlgorithmName = SelectedAlgorithmName.TrimEnd('*');
                            IsAlgoSaved = true;
                            break;
                    }
                }
            };
        }

		private void OpenCreateAlgorithmWindow()
		{
			var createAlgoModal = new CreateAlgorithmModal();
            createAlgoModal.ShowDialog();
		}

		private void OpenLoadAlgorithmWindow()
		{
			var loadMapDialog = new OpenFileDialog()
			{
				AddExtension = true,
				Multiselect = false,
				Filter = $"{ConfigurationManager.AppSettings["AlgoExtensionName"]}|*.{ConfigurationManager.AppSettings["AlgoExtension"]}",
			};

			if (loadMapDialog.ShowDialog().GetValueOrDefault())
				AlgorithmManager.GetInstance().LoadAlgorithm(loadMapDialog.FileName);
		}

		private void OpenSaveAlgorithmWindow()
		{
			var saveMapDialog = new SaveFileDialog()
			{
				FileName = SelectedAlgorithmName.TrimEnd('*'),
				Filter = $"{ConfigurationManager.AppSettings["AlgoExtensionName"]}|*.{ConfigurationManager.AppSettings["AlgoExtension"]}",
			};

			if (saveMapDialog.ShowDialog().GetValueOrDefault())
				AlgorithmManager.GetInstance().SaveAlgorithm(saveMapDialog.FileName);
		}
	}
}