using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace TSP.Desktop.Views.Utility
{
	public static class UIElementHelper
	{
		private static Dictionary<TextBox, ToolTip> ToolTips = new Dictionary<TextBox, ToolTip>();

		public static void ShowTooltip(this TextBox textBox, string toolTipMessage)
		{
			if (ToolTips.ContainsKey(textBox))
			{
				ToolTips[textBox].IsOpen = true;
				ToolTips[textBox].PlacementTarget = textBox;
			}
			else
			{
				ToolTips.Add(textBox, new ToolTip() { Placement = PlacementMode.Bottom, PlacementTarget = textBox, IsOpen = true });
			}

			ToolTips[textBox].Content = toolTipMessage;
			textBox.ToolTip = ToolTips[textBox];
		}

		public static void RefreshTooltip(this TextBox textBox)
		{
			if (ToolTips.ContainsKey(textBox))
			{
				textBox.ToolTip = null;
				ToolTips[textBox].IsOpen = true;
				ToolTips[textBox].PlacementTarget = textBox;
				ToolTips[textBox].Placement = PlacementMode.Bottom;
				textBox.ToolTip = ToolTips[textBox];
			}
		}
	}
}
