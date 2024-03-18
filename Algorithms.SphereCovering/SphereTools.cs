using System;
using System.Collections.Generic;

namespace Algorithms.SphereCovering
{
	//inspired by:
	//https://habr.com/ru/articles/460643/
	//[Actual formulas took from here] https://habr.com/ru/articles/506526/

	public static class SphereTools
	{
		public static double Epsilon = 1D;

		//x = (i/phi)%1 ---> (%1 - fraction part)
		//y = (i/n)
		//phi = (1 + sqrt(5))/2
		private static IList<(double X, double Y)> GetUnitSquarePointsCoordinates(int pointsNumber)
		{
			if (pointsNumber <= 0)
				throw new ArgumentOutOfRangeException($"'{nameof(pointsNumber)}' should be greater than zero!");

			var points = new List<(double X, double Y)>(pointsNumber);

			var phi = (1 + Math.Sqrt(5)) / 2;

			for (int i = 0; i < pointsNumber; i++)
			{
				double fibonacciProportion = i / phi;
				double x = fibonacciProportion - Math.Truncate(fibonacciProportion);
				double y = (1D * i + Epsilon) / (pointsNumber - 1 + 2 * Epsilon);

				points.Add((x, y));
			}

			return points;
		}

		//theta = 2*pi*x
		//ro = sqrt(y)
		private static IList<(double Theta, double Ro)> GetFibonacciSpiral(int pointsNumber)
		{
			var gridPoints = GetUnitSquarePointsCoordinates(pointsNumber);
			var spiralPoints = new List<(double X, double Y)>(gridPoints.Count);

			foreach (var gridPoint in gridPoints)
			{
				double theta = 2 * Math.PI * gridPoint.X;
				double ro = Math.Sqrt(gridPoint.Y);

				spiralPoints.Add((theta, ro));
			}

			return spiralPoints;
		}

		//tetha = 2 * pi * x
		//phi = arccos(1 - 2 * y)
		//x' = cos(tetha) * sin(phi)
		//y' = sin(tetha) * sin(phi)
		//z' = cos(phi)
		public static IList<(double X, double Y, double Z)> GetSphereEvenCoveringPoints(int pointsNumber)
		{
			var gridPoints = GetUnitSquarePointsCoordinates(pointsNumber);
			var spherePoints = new List<(double X, double Y, double Z)>(gridPoints.Count);

			foreach (var gridPoint in gridPoints)
			{
				double tetha = 2 * Math.PI * gridPoint.X;
				double phi = Math.Acos(1 - 2 * gridPoint.Y);

				double x = Math.Cos(tetha) * Math.Sin(phi);
				double y = Math.Sin(tetha) * Math.Sin(phi);
				double z = Math.Cos(phi);

				spherePoints.Add((x, y, z));
			}

			return spherePoints;
		}

		public static IList<(double X, double Y)> GetCirclePoints(int pointsNumber, double radius, (double X, double Y)? center)
		{
			if (!center.HasValue)
				center = (0D, 0D);

			var circlePoints = new List<(double X, double Y)>(pointsNumber);

			for (var i = 0; i < pointsNumber; i++)
			{
				double x = Math.Cos(2 * Math.PI * i / pointsNumber) * radius + center.Value.X;
				double y = Math.Sin(2 * Math.PI * i / pointsNumber) * radius + center.Value.Y;

				circlePoints.Add((x, y));
			}

			return circlePoints;
		}
	}
}