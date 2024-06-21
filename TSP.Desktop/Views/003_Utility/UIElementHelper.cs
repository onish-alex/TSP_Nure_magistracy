using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TSP.Desktop.Views.Utility
{
	public static class UIElementHelper
	{
		private static Dictionary<UIElement, ToolTip> ToolTips = new Dictionary<UIElement, ToolTip>();

		public static void ShowTooltip(this UIElement element, string toolTipMessage)
		{
			if (ToolTips.ContainsKey(element))
			{
				ToolTips[element].IsOpen = true;
				ToolTips[element].PlacementTarget = element;
			}
			else
			{
				ToolTips.Add(element, new ToolTip() { Placement = PlacementMode.Bottom, PlacementTarget = element, IsOpen = true });
			}

			ToolTips[element].Content = toolTipMessage;
			new ToolTipAbleWrapper(element).ToolTip = ToolTips[element];
		}

		public static void RefreshTooltip(this UIElement element)
		{
			var wrapper = new ToolTipAbleWrapper(element);

			if (ToolTips.ContainsKey(wrapper.Element))
			{
				wrapper.ToolTip = null;
				ToolTips[wrapper.Element].IsOpen = true;
				ToolTips[wrapper.Element].PlacementTarget = wrapper.Element;
				ToolTips[wrapper.Element].Placement = PlacementMode.Bottom;
				wrapper.ToolTip = ToolTips[wrapper.Element];
			}
		}
	}
}
