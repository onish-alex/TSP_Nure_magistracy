using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.TSPMap
{
	public class CreateTSPMapViewModel
	{
		public ICommand CreateTSPMapCommand { get; set; }

		public CreateTSPMapViewModel()
		{
			this.CreateTSPMapCommand = new CommonCommand((x) => CreateTSPMap(x));
		}

		private void CreateTSPMap(object param)
		{
			if (param is MapDTO mapDto)
				MapManager.GetInstance().CreateMap(mapDto);
		}
	}
}
