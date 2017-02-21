/*******************************************************************************
This file is part of the VIENNAAddIn project

Licensed under GNU General Public License V3 http://gplv3.fsf.org/

For further information on the VIENNAAddIn project please visit 
http://vienna-add-in.googlecode.com
*******************************************************************************/
using System;
using System.Windows.Forms;

namespace VIENNAAddIn.Utils
{
	/// <sUMM2ary>
	/// SUMM2ary description for FormStyles.
	/// </sUMM2ary>
	public class FormStyles
	{

		//Sizes for the buttons
		private const short X_SMALL = 80;
		private const short Y_SMALL = 20;
		private const short X_MEDIUM = 100;
		private const short Y_MEDIUM = 25;
		private const short X_LARGE = 120;
		private const short Y_LARGE = 30;


		internal FormStyles()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/// <sUMM2ary>
		/// returns a button with a specified text and a specified size
		/// 
		/// </sUMM2ary>
		/// <param name="text">is the label of the button</param>
		/// <param name="size">use "small" for a small button
		///						use "medium" for a medium button
		///						use "large" for a large button
		///	</param>
		/// <returns></returns>
		internal Button getButton(String name, String text, String size, int xPos, int yPos)
		{
			short width;
			short height;
			
			if (size.Equals("small"))
			{
				width=X_SMALL;
				height=Y_SMALL;
			}
			else if (size.Equals("medium"))
			{
				width=X_MEDIUM;
				height=Y_MEDIUM;
			}
			else 
			{
				width=X_LARGE;
				height=Y_LARGE;
			}

			Button button = new Button(); 
			button.Name = name;
			button.Text = text;
			button.Location = new System.Drawing.Point(xPos, yPos);
			button.Size = new System.Drawing.Size(width, height);
			return button;
			

		}
	}
}
