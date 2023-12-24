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

        protected IList<TVector> dataVectors;

        protected IList<TVector> networkVectors;
        protected double?[,] networkTopologyMatrix;
        protected Func<IEnumerable<double>, TVector> generateVector; 

        private SOMBuilderBase(IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator)
        {
            this.dataVectors = dataVectors;
            this.generateVector = vectorGenerator;
        }

        protected SOMBuilderBase(IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, Topology topology)
            : this(dataVectors, vectorGenerator)
        {
            InitNetwork(topology);
        }

        protected SOMBuilderBase(IList<TVector> dataVectors, Func<IEnumerable<double>, TVector> vectorGenerator, bool[,] topology)
            : this(dataVectors, vectorGenerator)
        {
            InitNetwork(topology);
        }

        protected abstract void InitNetwork(Topology topology);

        protected abstract void InitNetwork(bool[,] topology);

        protected abstract void InitSphereNetwork(int networkSize);

        protected abstract double GetDistance(TVector first, TVector second);
    }
}