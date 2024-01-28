using System;

namespace GA.Experiments.Writer
{
	public interface IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		void Write<TNode>(GAExperimentResult<TNode> result);

		void WriteLog(Exception ex);
	}
}
