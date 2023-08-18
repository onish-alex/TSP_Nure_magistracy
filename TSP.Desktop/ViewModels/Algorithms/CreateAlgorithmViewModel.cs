using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
