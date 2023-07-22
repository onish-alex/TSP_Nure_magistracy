using System.Windows.Input;
using TSP.Desktop.Commands;
using TSP.Desktop.Models.Managers;
using TSP.Desktop.ViewModels.Entities;
using TSP.Desktop.Views.Modals;

namespace TSP.Desktop.ViewModels.TSPMap
{
	public class CreateTSPMapViewModel
	{
		public ICommand CreateTSPMapCommand { get; set; }

		public CreateTSPMapViewModel()
		{
			this.CreateTSPMapCommand = new CommonCommand((x) => CreateTSPMapWindow(x));
		}

		private void CreateTSPMapWindow(object param)
		{
			if (param is MapDTO mapDto)
				MapManager.GetInstance().CreateMap(mapDto);
		}
	}
}
