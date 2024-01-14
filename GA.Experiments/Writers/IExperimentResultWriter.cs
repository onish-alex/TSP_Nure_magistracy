using System;

namespace GA.Experiments.Writer
{
    public interface IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch> 
    {
        void Write<TNode>(ExperimentResult<TNode> result);
    }
}
