using PropertyChanged;
using System;
using System.Collections.Generic;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.Algorithms
{
	[AddINotifyPropertyChangedInterface]
	public class CreateAlgorithmViewModel
	{

		public IEnumerable<AlgorithmType> AlgorithmTypes { get; set; }

		public AlgorithmDTO AlgorithmDTO { get; private set; }

		public CreateAlgorithmViewModel()
		{
			this.AlgorithmTypes = Enum.GetValues<AlgorithmType>();

			this.AlgorithmDTO = new AlgorithmDTO();
		}

		public void SetAlgorithm(AlgorithmDTO algorithmDTO)
		{
			AlgorithmDTO.Name = algorithmDTO.Name;
			AlgorithmDTO.Type = algorithmDTO.Type;
		}
	}
}
