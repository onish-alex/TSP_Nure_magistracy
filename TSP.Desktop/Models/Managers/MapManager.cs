using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Desktop.Models.Entities;

namespace TSP.Desktop.Models.Managers
{
    public class MapManager : INotifyPropertyChanged
    {
        private static MapManager Instance { get; set; }

        private MapManager()
        {
        }

		public Map Map { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public static MapManager GetInstance()
        {
            if (Instance == null)
                Instance = new MapManager();

            return Instance;
        }

		public void SetMap(Map map)
        {
            Instance.Map = map;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Map)));
        }
    }
}
