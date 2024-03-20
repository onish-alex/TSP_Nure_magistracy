using System;

namespace SOM.Experiments.Writer
{
	public interface IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch>
	{
		void Write(SOMExperimentResult result);

		void WriteLog(Exception ex);
	}
}
