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
        public double RoundPrecision { get; set; }

        //decreasing elasticity multiplier
        public double CooperationCoefficient { get; set; }

        //decreasing learning coef multiplier
        public double LearningFadingCoefficient { get; set; }

        public bool UseElasticity { get; set; }
    }
}
