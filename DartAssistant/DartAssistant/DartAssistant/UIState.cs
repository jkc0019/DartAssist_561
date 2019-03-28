using System;
using System.Collections.Generic;
using System.Text;

namespace DartAssistant
{
	public class UIState
	{
		/// <summary>
		/// Gets the Last dart score
		/// </summary>
		public int LastScore { get; set; }

		/// <summary>
		/// Gets the current score text.
		/// </summary>
		public string CurrentScoreText { get; set; }

		/// <summary>
		/// Gets the double out text.
		/// </summary>
		public string DoubleOutText { get; set; }
	}
}
