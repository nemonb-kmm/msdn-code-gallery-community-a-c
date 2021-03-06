using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace System.Web.UI.DataVisualization.Charting.Samples
{
	/// <summary>
    /// Summary description for RealTimeChart.
	/// </summary>
	public partial class RealTimeChart : System.Web.UI.Page
	{
        /// <summary>
        /// Page Load event handler.
        /// </summary>
        protected void Page_Load(object sender, System.EventArgs e)
        {
            
        }


        /// <summary>
        /// Handles the Tick event of the Timer1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Random rand = new Random();

            // Add several random point into each series
            foreach (Series series in this.Chart1.Series)
            {
                double lastYValue = series.Points[series.Points.Count - 1].YValues[0];
                double lastXValue = series.Points[series.Points.Count - 1].XValue + 1;
                for (int pointIndex = 0; pointIndex < 5; pointIndex++)
                {
                    lastYValue += rand.Next(-3, 4);
                    if (lastYValue >= 100.0)
                    {
                        lastYValue -= 25.0;
                    }
                    else if (lastYValue <= 10.0)
                    {
                        lastYValue += 25.0;
                    }
                    series.Points.AddXY(lastXValue++, lastYValue);
                }
            }

            // Remove points from the left chart side if number of points exceeds 100.
            while (this.Chart1.Series[0].Points.Count > 100)
            {
                // Remove series points
                foreach (Series series in this.Chart1.Series)
                {
                    series.Points.RemoveAt(0);
                }

            }

            // Adjust categorical scale
            double axisMinimum = this.Chart1.Series[0].Points[0].XValue;
            this.Chart1.ChartAreas[0].AxisX.Minimum = axisMinimum;
            this.Chart1.ChartAreas[0].AxisX.Maximum = axisMinimum + 100;
        }
    }
}
