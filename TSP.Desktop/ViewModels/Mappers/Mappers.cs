using AutoMapper;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.Mappers
{
	internal static class Mappers
	{
		internal static IMapper TSPMap = new MapperConfiguration(x => x.CreateMap<MapDTO, Map>()).CreateMapper();
		internal static IMapper Algorithm = new MapperConfiguration(x => x.CreateMap<AlgorithmDTO, Algorithm>()).CreateMapper();
	}
}
