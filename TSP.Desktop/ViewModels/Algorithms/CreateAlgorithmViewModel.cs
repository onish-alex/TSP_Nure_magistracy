using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.TSPMap
{
	public class CreateAlgorithmViewModel
	{
		public ICommand CreateAlgorithmCommand { get; set; }

		public CreateAlgorithmViewModel()
		{
			this.CreateAlgorithmCommand = new CommonCommand((x) => CreateAlgorithm(x));
		}

		private void CreateAlgorithm(object param)
		{
			if (param is AlgorithmDTO algoDto)
				AlgorithmManager.GetInstance().CreateAlgorithm(algoDto);
		}
	}
}
