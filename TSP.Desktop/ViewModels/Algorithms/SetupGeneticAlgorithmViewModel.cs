using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TSP.Desktop.Commands;
using TSP.Desktop.Common;
using TSP.Desktop.Models.Entities;
using TSP.Desktop.ViewModels.Entities;

namespace TSP.Desktop.ViewModels.Algorithms
{
    public class SetupGeneticAlgorithmViewModel
    {
        public ICommand CreateGeneticAlgorithmCommand { get; set; }

        public IEnumerable<CrossoverType> CrossoverTypes { get; set; }
        public IEnumerable<SelectionType> SelectionTypes { get; set; }
        public IEnumerable<MutationType> MutationTypes { get; set; }

        public GeneticAlgorithmSettings AlgorithmSettings { get; private set; }

        public SetupGeneticAlgorithmViewModel()
        {
            this.CreateGeneticAlgorithmCommand = new CommonCommand((x) => FillAlgorithmParameters(x));
            this.CrossoverTypes = Enum.GetValues<CrossoverType>();
            this.SelectionTypes = Enum.GetValues<SelectionType>();
            this.MutationTypes = Enum.GetValues<MutationType>();

            this.AlgorithmSettings = new GeneticAlgorithmSettings();
        }

        public void SetSettings(GeneticAlgorithmSettings geneticAlgorithmSettings)
        {
            this.AlgorithmSettings.SelectionType = geneticAlgorithmSettings.SelectionType;
            this.AlgorithmSettings.CrossoverType = geneticAlgorithmSettings.CrossoverType;
            this.AlgorithmSettings.MutationType = geneticAlgorithmSettings.MutationType;

            this.AlgorithmSettings.GASettings.MutationProbability = geneticAlgorithmSettings.GASettings.MutationProbability;
            this.AlgorithmSettings.GASettings.OnlyChildrenInNewGeneration = geneticAlgorithmSettings.GASettings.OnlyChildrenInNewGeneration;
            this.AlgorithmSettings.GASettings.ElitePercent = geneticAlgorithmSettings.GASettings.ElitePercent;
        }

        public void FillAlgorithmParameters(object param)
        {
            AlgorithmSettings = param as GeneticAlgorithmSettings;
        }
    }
}
