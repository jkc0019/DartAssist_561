using System;
using System.Collections.Generic;
using Android.Content;


namespace DartAssistant.Droid
{
	public class PreferenceStore
	{

		//Define Instance or Preference Interface for storing data
		ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences("TopScores", FileCreationMode.Private);

		public bool CheckScoresTopList(string totalScore)
		{
			string scores = "";

			//Check if there was a previous High Score and
			//determine if current score needs added
			scores = pref.GetString("TopScores", "");

			try
			{

				//Define a new list to use for sorting scores
				List<long> sortableList = new List<long>();

				if (scores.Contains(","))
				{
					//split comma-delimited scores list into string array
					string[] savedHighScores = scores.Split(',');

					//Add items from string array to new <List>
					if (savedHighScores.Length > 0)
					{
						foreach (string score in savedHighScores)
						{
							sortableList.Add(Convert.ToInt64(score));
						}
					}

				}
				else if (scores.Trim().Length > 0)
				{
					sortableList.Add(Convert.ToInt64(scores));
				}

				//Format the new score as long with date appended to end in yyyymmdd format
				string formattedDate = String.Format("{0:yyyyMMdd}", DateTime.Now);

				long newScore = Convert.ToInt64(totalScore.ToString() + formattedDate);

				sortableList.Add(newScore);
				sortableList.Sort();
				sortableList.Reverse();

				//Variables to determine is a new list needs to be stored
				bool didScoreMakeList = true;
				string newList = "";
				bool fixList = false;
				//int listRank = 0;

				//If there are more than 11 then somehow there were more than 10
				//stored previously so correct regardless of current score
				if (sortableList.Count > 11)
				{
					fixList = true;
				}

				while (sortableList.Count > 10)
				{
					if (newScore == sortableList[10])
					{
						didScoreMakeList = false;
					}

					sortableList.RemoveAt(10);
				}

				//Need to store a new list so create comma-delimited list from sorted <List>
				if (fixList || didScoreMakeList)
				{
					foreach (long sortedScore in sortableList)
					{
						
						newList += sortedScore.ToString() + ",";

					}
					//Remove trailing ","
					if (newList.EndsWith(","))
					{
						newList = newList.TrimEnd(',');
					}
				}

				SaveToScoresTopList(TopScores: newList);
				return true;
			}
			catch (Exception ex)
			{
				//Used for Debugging
				System.Diagnostics.Debug.Print("Error- " + ex.Message);
				throw (ex);

			}

		}

		public void SaveToScoresTopList(string TopScores)
		{

			try
			{
				ISharedPreferencesEditor edit = pref.Edit();

				edit.PutString("TopScores", TopScores);
				edit.Apply();

				return;
			}
			catch (Exception ex)
			{
				//Used for Debugging
				System.Diagnostics.Debug.Print("Error- " + ex.Message);
				throw (ex);
			}

		}
		
	}
}