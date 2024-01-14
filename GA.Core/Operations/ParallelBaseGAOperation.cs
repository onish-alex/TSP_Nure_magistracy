using GA.Core.Utility;
using System.Threading.Tasks;

namespace GA.Core.Operations
{
	public abstract class ParallelBaseGAOperation : BaseGAOperation
	{
		protected ParallelOptions parallelOptions;

		protected ParallelBaseGAOperation(GAOperationSettings operationSettings) : base(operationSettings)
		{
			parallelOptions = new ParallelOptions();

			if (operationSettings.ThreadsCount > 0)
				parallelOptions.MaxDegreeOfParallelism = operationSettings.ThreadsCount;
		}
	}
}
