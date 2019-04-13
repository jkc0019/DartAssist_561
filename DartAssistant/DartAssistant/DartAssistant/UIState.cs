using System;
using System.Collections.Generic;

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

		/// <summary>
		/// Gets the Muted Toggle Value.
		/// </summary>
		public bool IsMuted { get; set; }

		/// <summary>
		/// Gets the IsListening Toggle Value.
		/// </summary>
		public bool IsListening { get; set; }

		/// <summary>
		/// Gets the InitialStartupOver Toggle Value.
		/// </summary>
		public bool InitialStartupOver { get; set; }

	}
}
