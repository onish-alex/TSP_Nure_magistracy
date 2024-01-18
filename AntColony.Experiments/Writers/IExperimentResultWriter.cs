using System;

namespace AntColony.Experiments.Writer
{
	public interface IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		void Write<TNode>(ACExperimentResult<TNode> result);
	}
}
