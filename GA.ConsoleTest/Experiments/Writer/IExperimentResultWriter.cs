using GA.Core.Utility;
using System;

namespace GA.ConsoleApp.Experiments.Writer
{
    internal interface IExperimentResultWriter<TResearch> where TResearch : struct, IComparable<TResearch> 
    {
        void Write<TNode>(ExperimentResult<TNode> result);
    }
}
