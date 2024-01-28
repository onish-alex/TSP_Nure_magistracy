using SOM.TSPCompatibility;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TSP.Examples;

namespace SOM.ChartsApp
{
	public partial class Form1 : Form
	{
		private BaseSOM<IVector<double>> som;

		public Form1()
		{
			InitializeComponent();
		}

		//private void bNext_Click(object sender, System.EventArgs e)
		//{
		//	som.ProcessEpochIteration().GetEnumerator().MoveNext();

		//	chart1.Series["Network"].Points.Clear();

		//	foreach (var node in som.Network)
		//	{
		//		chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);
		//	}

		//	lbLength.Text = som.GetFullLength().ToString();
		//	lbLearn.Text = som.settings.LearningCoefficient.ToString();
		//	lbN.Text = som.n.ToString();
		//}

		private void bNext_Click(object sender, System.EventArgs e)
		{
			som.ProcessEpoch();

			chart1.Series["Network"].Points.Clear();

			foreach (var node in som.Network)
			{
				chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);
			}

			lbLength.Text = som.GetFullLength().ToString();
			lbLearn.Text = som.settings.LearningCoefficient.ToString();
			lbN.Text = som.n.ToString();
		}

		//private void bNext_Click(object sender, System.EventArgs e)
		//{
		//	var result = som.BuildMap();

		//	chart1.Series["Network"].Points.Clear();

		//	foreach (var node in result)
		//	{
		//		chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);
		//	}

		//	lbLength.Text = som.GetFullLength().ToString();
		//	lbLearn.Text = som.settings.LearningCoefficient.ToString();
		//	lbN.Text = som.n.ToString();
		//}

		private void bInit_Click(object sender, System.EventArgs e)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.eil51);

			som = new TwoDimensionalSOM<IVector<double>>(
				new Configuration.SOMSettings()
				{
					LearningCoefficient = 0.1D,
					UseDistancePenalties = true,
					PenaltiesIncreasingCoefficient = 0.75D,
					//RoundPrecision = model.DistancesMap.Values.SelectMany(x => x.Values).Min(),
					RoundPrecision = 5D,
					LearningFadingCoefficient = 0.0003D,
					UseElasticity = true,
					CooperationDistance = 0.15D,
					CooperationThreshold = 0.01D,
					NetworkRadiusPercent = 5D,
				},
				model.Nodes.Select(x => SOMMapper.Map(x)).ToList(),
				(x) => new Vector(x.ToList()),
				Configuration.Topology.Sphere);

			chart1.ChartAreas[0].RecalculateAxesScale();

			foreach (var node in model.Nodes)
			{
				chart1.Series["Data"].Points.AddXY(node.X, node.Y);
			}

			foreach (var node in som.Network)
			{
				chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);
			}
		}

		private void checkBox1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBox1.Checked)
				chart1.Series["Network"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
			else
				chart1.Series["Network"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
		}
	}
}