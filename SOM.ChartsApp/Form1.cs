using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using TSP.Core;
using TSP.Examples;

namespace SOM.ChartsApp
{
	public partial class Form1 : Form
	{
		private BaseSOM<TSPNode> som;

		public Form1()
		{
			InitializeComponent();
		}


		private void bNext_Click(object sender, System.EventArgs e)
		{
			som.ProcessIteration();

			chart1.Series["Network"].Points.Clear();

			var result = (som.FinishCondition ? som.BuildMap() : som.Network).ToArray();

			foreach (var node in result)
				chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);

			double sum = 0D;
			for (int i = 1; i < result.Count(); i++)
				sum += som.GetDistance(result[i], result[i - 1]);

			sum += som.GetDistance(result.First(), result.Last());

			lbLength.Text = sum.ToString();
			lbLearn.Text = som.settings.LearningCoefficient.ToString();
			lbN.Text = som.settings.CooperationCoefficient.ToString();
		}

		//private void bNext_Click(object sender, System.EventArgs e)
		//{
		//	var result = som.BuildMap().ToArray();

		//	chart1.Series["Network"].Points.Clear();

		//	foreach (var node in result)
		//	{
		//		chart1.Series["Network"].Points.AddXY(node["X"], node["Y"]);
		//	}

		//	double sum = 0D;
		//	for (int i = 1; i < result.Count(); i++)
		//		sum += som.GetDistance(result[i], result[i - 1]);

		//	sum += som.GetDistance(result.First(), result.Last());

		//	lbLength.Text = sum.ToString();
		//	lbLearn.Text = som.settings.LearningCoefficient.ToString();
		//	lbN.Text = som.settings.CooperationCoefficient.ToString();
		//}

		private void bInit_Click(object sender, System.EventArgs e)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
			CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

			var model = PreparedModelLoader.GetModel(PreparedModelsEnum.ch150);

			som = new TwoDimensionalSOM<TSPNode>(
				new Configuration.SOMSettings()
				{
                    LearningCoefficient = 0.35D,                                            //+-
                    UseDistancePenalties = true,                                            //-
                    PenaltiesIncreasingCoefficient = 18D,                                  //+
                    RoundPrecision = 1D,                                                    //-
                    LearningFadingCoefficient = 0.0001D,                                    //+-
                    UseElasticity = true,                                                   //-
                    CooperationCoefficient = model.Nodes.Count * model.Nodes.Count,         //+-
                    CooperationFading = 0.1D,                                               //+
                    CooperationThreshold = 1D,                                              //-
                    NetworkRadiusPercent = 30D,                                             //-
                    NetworkSizeMultiplier = 3
                },
				model.Nodes,
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