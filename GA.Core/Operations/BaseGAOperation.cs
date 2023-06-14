using GA.Core.Utility;
using System;

namespace GA.Core
{
	public abstract class BaseGAOperation
	{
		protected GAOperationSettings operationSettings;

		protected BaseGAOperation(GAOperationSettings operationSettings)
		{
			if (operationSettings == null) 
				throw new ArgumentNullException(nameof(operationSettings));
			
			this.operationSettings = operationSettings;

			if (operationSettings.InitType == GAOperationInitType.OneTime)
				InitSettings();
		}

		protected abstract void InitSettings();
	}
}
