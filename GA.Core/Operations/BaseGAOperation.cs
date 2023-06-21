using GA.Core.Utility;
using System;

namespace GA.Core
{
	public abstract class BaseGAOperation
	{
		protected GAOperationSettings operationSettings;

		protected BaseGAOperation(GAOperationSettings operationSettings)
		{
			this.operationSettings = operationSettings ?? throw new ArgumentNullException(nameof(operationSettings));

			if (operationSettings.InitType == GAOperationInitType.OneTime)
				InitSettings();
		}

		protected abstract void InitSettings();
	}
}
