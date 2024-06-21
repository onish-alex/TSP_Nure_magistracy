using System;
using System.Windows.Input;

namespace TSP.Desktop.Commands
{
	public class CommonCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private Action<object> execute;

		public CommonCommand(Action<object> execute)
		{
			this.execute = execute;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			execute(parameter);
		}
	}
}
