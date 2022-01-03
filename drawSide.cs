using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WinFlash
{
	internal static class drawSide
	{
		private const Single leftPadding = 10;
		// TODO: Make these two configurable by user
		private static Font font = SystemFonts.DefaultFont;
		private static Brush defaultBrush = Brushes.Black;
		public static void draw(Graphics g, string[] content)
		{
			Single rowPointer = 10;
			float rowSize = font.GetHeight();
			foreach (string text in content)
			{
				if (text.EndsWith(".jpg") || text.EndsWith(".png"))
				{

				}
				else
				{
					g.DrawString(text + "\n", font, Brushes.Black, 10, rowPointer);
					rowPointer += rowSize;
				}
			}
		}
	}
}
