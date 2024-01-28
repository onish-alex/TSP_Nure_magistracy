using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace SOM
{
	public abstract class BaseSOM<TVector> where TVector : IVector<double>
	{
		protected const double networkDistancePenaltiesDefaultValue = 1D;

		protected IList<TVector> dataVectors;

		protected int networkSize;
		protected IList<TVector> networkVectors;
		protected double?[,] networkTopologyMatrix;
		protected Func<IEnumerable<double>, TVector> generateVector;
		public SOMSettings settings;
		protected Dictionary<TVector, double> networkDistancePenalties;
		protected Dictionary<TVector, bool> networkReadiness;
		protected Dictionary<TVector, bool> dataReadiness;

		//protected IList<TVector> finalNetworkVectors;
		public double n;

		public IList<TVector> Network => networkVectors.ToList();

		public bool FinishCondition => !this.networkReadiness.Any(x => x.Value == false);

		private BaseSOM(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator)
		{
			this.dataVectors = dataVectors;
			this.dataReadiness = new Dictionary<TVector, bool>(dataVectors.Count);

			foreach (var dataVector in this.dataVectors)
				dataReadiness.Add(dataVector, false);

			this.generateVector = vectorGenerator;
			this.settings = settings;
		}

		protected BaseSOM(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, Topology topology)
			: this(settings, dataVectors, vectorGenerator)
		{
			InitNetwork(topology);
		}

		protected BaseSOM(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, bool[,] topology)
			: this(settings, dataVectors, vectorGenerator)
		{
			InitNetwork(topology);
		}

		protected abstract void InitNetwork(Topology topology);

		protected abstract void InitNetwork(bool[,] topology);

		protected virtual void InitSphereNetwork(int networkSize)
		{
			this.networkSize = networkSize;
		}

		protected abstract double GetDistance(TVector first, TVector second);

		protected abstract double GetElasticityCoefficient(TVector chosenVector, TVector otherVector);

		public abstract void ProcessEpoch();
		public abstract IEnumerable<int> ProcessEpochIteration();

		public abstract IEnumerable<TVector> BuildMap();

		public double GetFullLength()
		{
			double sum = 0D;
			for (int i = 1; i < this.networkSize; i++)
				sum += GetDistance(this.networkVectors[i], this.networkVectors[i - 1]);

			sum += GetDistance(this.networkVectors.First(), this.networkVectors.Last());

			return sum;
		}
	}
}