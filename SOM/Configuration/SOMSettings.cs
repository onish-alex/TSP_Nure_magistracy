using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Configuration
{
    public class SOMSettings
    {
        //distance after which network vector parameters will be set in data vector parameters values
        public double RoundPrecision { get; set; } = 1;

        //decreasing elasticity multiplier
        public double CooperationCoefficient { get; set; }
        public bool UseElasticity { get; set; } = false;

        //decreasing learning coef multiplier
        public double? LearningFadingCoefficient { get; set; } = null;
        public double LearningCoefficient { get; set; } = 0.5;

        //coefficient for increasing penalties (decreasing distance)
        public double PenaltiesIncreasingCoefficient { get; set; }
        public bool UseDistancePenalties { get; set; } = false;
    }
}
