using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TSP.Desktop.Views;

namespace TSP.Desktop
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly IHost _host;

		/// <summary>
		/// Interaction logic for App.xaml
		/// </summary>
		public App()
		{
			_host = Host.CreateDefaultBuilder()
			.ConfigureServices((services) =>
			{
				services.AddSingleton<App>();
				services.AddSingleton<MainWindow>();
			})
			.Build();
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			_host.Start();

			MainWindow = _host.Services.GetRequiredService<MainWindow>();
			MainWindow.Show();

			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			_host.StopAsync();
			_host.Dispose();

			base.OnExit(e);
		}
	}
}
