namespace GA.Core.Utility
{
	public enum GAOperationInitType
	{
		Manual,				//Setting all parameters when creating operation
		OneTime,			//Random setting in constructor
		EveryGeneration,	//Random setting in each operation run
		EveryIndividual		//Random setting for each individual/pair/etc
	}
}
