using SOM.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SOM
{
    public abstract class SOMBuilderBase<TVector> where TVector : IVector<double>
    {
        protected const double radiusPercent = 1D;
        protected const double networkDistancePenaltiesDefaultValue = 1D;

        protected IList<TVector> dataVectors;

        protected IList<TVector> networkVectors;
        protected double?[,] networkTopologyMatrix;
        protected Func<IEnumerable<double>, TVector> generateVector;
        protected SOMSettings settings;
        protected Dictionary<TVector, double> networkDistancePenalties;
        protected Dictionary<TVector, bool> networkReadiness;
        protected Dictionary<TVector, bool> dataReadiness;

        private SOMBuilderBase(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator)
        {
            this.dataVectors = dataVectors;
            this.dataReadiness = new Dictionary<TVector, bool>(dataVectors.Count);

            foreach (var dataVector in this.dataVectors)
                dataReadiness.Add(dataVector, false);

            this.generateVector = vectorGenerator;
            this.settings = settings;
        }

        protected SOMBuilderBase(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, Topology topology)
            : this(settings, dataVectors, vectorGenerator)
        {
            InitNetwork(topology);
        }

        protected SOMBuilderBase(SOMSettings settings, IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, bool[,] topology)
            : this(settings, dataVectors, vectorGenerator)
        {
            InitNetwork(topology);
        }

        protected abstract void InitNetwork(Topology topology);

        protected abstract void InitNetwork(bool[,] topology);

        protected abstract void InitSphereNetwork(int networkSize);

        protected abstract double GetDistance(TVector first, TVector second);
        
        protected abstract double GetElasticityCoefficient(TVector chosenVector, TVector otherVector);

        protected abstract void ProcessEpoch();

        public abstract IEnumerable<TVector> BuildMap();
    }
}