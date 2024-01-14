using Algorithms.SphereCovering;
using System;
using System.Globalization;
using System.Linq;

namespace Algorithms.ConsoleApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			SphereTools.Epsilon = 0D;
			//var points = SphereTools.GetSphereEvenCoveringPoints(250);

			//using (var file = System.IO.File.CreateText("points.csv"))
			//{
			//    foreach (var point in points)
			//        file.WriteLine($"{point.X};{point.Y};{point.Z};");

			//    file.Close();
			//}

			//var culture = CultureInfo.CreateSpecificCulture("en-US");
			//culture.NumberFormat = new NumberFormatInfo() { CurrencyDecimalSeparator = "." };
			//CultureInfo.CurrentCulture = culture;

			//Console.WriteLine(string.Join(',', points.Select(x => x.ToString().Contains("E") ? "0" : x.ToString())));
			//Console.WriteLine("========================");

			//foreach (var point in points)
			//{
			//    Console.WriteLine(point);
			//}

			var points = SphereTools.GetCirclePoints(250, 20, (0, 0));

			using (var file = System.IO.File.CreateText("points.csv"))
			{
				foreach (var point in points)
					file.WriteLine($"{point.X};{point.Y};");

				file.Close();
			}

			var culture = CultureInfo.CreateSpecificCulture("en-US");
			culture.NumberFormat = new NumberFormatInfo() { CurrencyDecimalSeparator = "." };
			CultureInfo.CurrentCulture = culture;

			Console.WriteLine(string.Join(',', points.Select(x => x.ToString().Contains("E") ? "0" : x.ToString())));
			Console.WriteLine("========================");

			foreach (var point in points)
			{
				Console.WriteLine(point);
			}

		}
	}
}