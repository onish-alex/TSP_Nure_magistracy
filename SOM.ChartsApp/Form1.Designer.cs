namespace SOM.ChartsApp
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.StripLine stripLine1 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            bNext = new System.Windows.Forms.Button();
            bInit = new System.Windows.Forms.Button();
            checkBox1 = new System.Windows.Forms.CheckBox();
            lbLength = new System.Windows.Forms.Label();
            lbLearn = new System.Windows.Forms.Label();
            lbN = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.AxisX.MajorGrid.Enabled = false;
            stripLine1.ForeColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.StripLines.Add(stripLine1);
            chartArea1.AxisX2.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(0, -1);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.Name = "Data";
            series2.BorderWidth = 4;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series2.Legend = "Legend1";
            series2.Name = "Network";
            chart1.Series.Add(series1);
            chart1.Series.Add(series2);
            chart1.Size = new System.Drawing.Size(788, 413);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            // 
            // bNext
            // 
            bNext.Location = new System.Drawing.Point(694, 418);
            bNext.Name = "bNext";
            bNext.Size = new System.Drawing.Size(94, 29);
            bNext.TabIndex = 1;
            bNext.Text = "Next";
            bNext.UseVisualStyleBackColor = true;
            bNext.Click += bNext_Click;
            // 
            // bInit
            // 
            bInit.Location = new System.Drawing.Point(594, 418);
            bInit.Name = "bInit";
            bInit.Size = new System.Drawing.Size(94, 29);
            bInit.TabIndex = 2;
            bInit.Text = "Init";
            bInit.UseVisualStyleBackColor = true;
            bInit.Click += bInit_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(12, 418);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(101, 24);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // lbLength
            // 
            lbLength.AutoSize = true;
            lbLength.Location = new System.Drawing.Point(165, 419);
            lbLength.Name = "lbLength";
            lbLength.Size = new System.Drawing.Size(50, 20);
            lbLength.TabIndex = 4;
            lbLength.Text = "label1";
            // 
            // lbLearn
            // 
            lbLearn.AutoSize = true;
            lbLearn.Location = new System.Drawing.Point(519, 422);
            lbLearn.Name = "lbLearn";
            lbLearn.Size = new System.Drawing.Size(50, 20);
            lbLearn.TabIndex = 5;
            lbLearn.Text = "label1";
            // 
            // lbN
            // 
            lbN.AutoSize = true;
            lbN.Location = new System.Drawing.Point(358, 420);
            lbN.Name = "lbN";
            lbN.Size = new System.Drawing.Size(50, 20);
            lbN.TabIndex = 6;
            lbN.Text = "label1";
            // 
            // Form1
            // 
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(lbN);
            Controls.Add(lbLearn);
            Controls.Add(lbLength);
            Controls.Add(checkBox1);
            Controls.Add(bInit);
            Controls.Add(bNext);
            Controls.Add(chart1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
		private System.Windows.Forms.Button bNext;
		private System.Windows.Forms.Button bInit;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label lbLength;
		private System.Windows.Forms.Label lbLearn;
		private System.Windows.Forms.Label lbN;
	}
}