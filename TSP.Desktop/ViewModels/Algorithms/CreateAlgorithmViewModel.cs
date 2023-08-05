using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.Algorithms
{
    [AddINotifyPropertyChangedInterface]
    public class CreateAlgorithmViewModel
	{
		public ICommand CreateAlgorithmCommand { get; set; }
		
		//public IEnumerable<(int Value, string Name)> AlgorithmTypes { get; set; }
		public IEnumerable<AlgorithmType> AlgorithmTypes { get; set; }
		public AlgorithmType SelectedType { get; set; }


        public CreateAlgorithmViewModel()
		{
			this.CreateAlgorithmCommand = new CommonCommand((x) => CreateAlgorithm(x));
			this.AlgorithmTypes = Enum.GetValues<AlgorithmType>();
			//.Select(x => ((int)x,  x.ToString()));

			this.SelectedType = this.AlgorithmTypes.First();
        }

		private void CreateAlgorithm(object param)
		{
			if (param is AlgorithmDTO algoDto)
				AlgorithmManager.GetInstance().CreateAlgorithm(algoDto);
		}
	}
}
