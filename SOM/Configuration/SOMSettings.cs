namespace SOM.Configuration
{
	public class SOMSettings
	{
		//distance after which network vector parameters will be set in data vector parameters values
		public double RoundPrecision { get; set; } = 1;

		public bool UseElasticity { get; set; } = false;

		public double CooperationCoefficient { get; set; } = 0;
		public double CooperationFading { get; set; } = 1;
		public double CooperationThreshold { get; set; }

		//decreasing learning coef multiplier
		public double? LearningFadingCoefficient { get; set; } = null;
		public double LearningCoefficient { get; set; } = 0.5;

		//coefficient for increasing penalties (decreasing distance)
		public double PenaltiesIncreasingCoefficient { get; set; }
		public bool UseDistancePenalties { get; set; } = false;

		public double NetworkRadiusPercent { get; set; } = 1D;

        //Increases the amount of neurons in <c>NetworkSizeMultiplier</c>
		//times to avoid long offshoots due to not even points distribution
        public double NetworkSizeMultiplier { get; set; }
	}
}
