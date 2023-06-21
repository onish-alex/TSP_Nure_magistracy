namespace GA.Core.Utility
{
	public class GAOperationSettings
	{
		public GAOperationInitType InitType { get; set; } = GAOperationInitType.Manual;

		public int NodesCount { get; set; }

		//Number of threads, for parallel operations only		
		public int ThreadsCount { get; set; }
	}
}
