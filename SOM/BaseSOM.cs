using Algorithms.Utility.StructuresLinking;
using SOM.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SOM
{
	public abstract class BaseSOM<TVector> where TVector : IVector<double>
	{
		protected const double networkDistancePenaltiesDefaultValue = 1D;

		protected IList<TVector> dataVectors;

		protected int networkSize;
		protected IList<TVector> networkVectors;
		protected Dictionary<TVector, Dictionary<TVector, double>> networkTopologyDistances;
		public SOMSettings settings;
		protected Dictionary<TVector, double> networkDistancePenalties;
		protected Dictionary<TVector, bool> networkReadiness;
		protected Dictionary<TVector, bool> dataReadiness;

		public IList<TVector> Network => networkVectors.ToList();

		public bool FinishCondition => !this.dataReadiness.Any(x => x.Value == false);

		public int ProcessedVectors => this.dataReadiness.Count(x => x.Value);

		private BaseSOM(SOMSettings settings, IList<TVector> dataVectors)
		{
			this.dataVectors = dataVectors;
			this.dataReadiness = new Dictionary<TVector, bool>(dataVectors.Count);

			foreach (var dataVector in this.dataVectors)
				dataReadiness.Add(dataVector, false);

			this.settings = settings;
		}

		protected BaseSOM(SOMSettings settings, IList<TVector> dataVectors, Topology topology)
			: this(settings, dataVectors)
		{
			InitNetwork(topology);
		}

		protected BaseSOM(SOMSettings settings, IList<TVector> dataVectors, bool[,] topology)
			: this(settings, dataVectors)
		{
			InitNetwork(topology);
		}

		protected abstract void InitNetwork(Topology topology);

		protected abstract void InitNetwork(bool[,] topology);

		protected virtual void InitSphereNetwork(int networkSize)
		{
			this.networkSize = networkSize;
		}

		public abstract double GetDistance(TVector first, TVector second);

		protected abstract double GetNeighbourFunction(TVector chosenVector, TVector otherVector);

		public abstract void ProcessIteration();
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