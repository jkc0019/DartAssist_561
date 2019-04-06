using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;

namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Top Scores")]
	public class TopScoresActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		BottomNavigationView bottomNavigation;

		string UIClassSerial = "";
		string turnClassSerial = "";
		
		//Define Instance or Preference Interface for storing data
		ISharedPreferences pref = Android.App.Application.Context.GetSharedPreferences("TopScore", FileCreationMode.Private);

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.TopScores);

			turnClassSerial = Intent.GetStringExtra("turnClassSerial"); ;
			UIClassSerial = Intent.GetStringExtra("UIClassSerial");

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			if (toolbar != null)
			{
				SetSupportActionBar(toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled(false);
				SupportActionBar.SetHomeButtonEnabled(false);

			}

			bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);
			
			bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

			try
			{
				//TopScores();
				bottomNavigation.Menu.GetItem(3).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}

		}
		protected override void OnResume()
		{

			try
			{
				TopScores();
				bottomNavigation.Menu.GetItem(3).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}

			base.OnResume();
		}

		public void TopScores()
		{
			
			try
			{

				string scores = "";
				string[] savedHighScores = new string[10];

				//Check if there was a previous High Score and Display 
				scores = pref.GetString("TopScores", "");
				
				if (scores.Contains(","))
				{
					//split comma-delimited scores list into string array
					savedHighScores = scores.Split(',');

				}
				else if (scores.Trim().Length > 0)
				{
					savedHighScores = new string[1];
					savedHighScores[0] = scores;
				}
				
				string extractedScore = "";
				string formattedDate = "";
				//int counter = 0;
				string formattedText = "";
				List<String> allOutsList = new List<String>();

				if (savedHighScores.Length > 0)
				{
					allOutsList.Add(" ");
					allOutsList.Add("Score" + "Date".ToString().PadLeft(15, ' '));

					foreach (string score in savedHighScores)
					{
						extractedScore = score.Substring(0, score.Length - 8).PadLeft(5, ' ') + ": ";

						formattedDate = "   " + score.Substring(score.Length - 4, 2) + "/" +
									score.ToString().Substring(score.Length - 2, 2) + "/" +
									score.ToString().Substring(score.Length - 8, 4);

						formattedText = extractedScore + " " + formattedDate;

						allOutsList.Add(formattedText);

					}
					
				}
				else
				{
					allOutsList.Add(" ");
					allOutsList.Add("No High Scores Recorded");
				}

				Android.Widget.ListView lstMyList = (Android.Widget.ListView)FindViewById<Android.Widget.ListView>(Resource.Id.MyList);
				lstMyList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, allOutsList);

				//TextView lstItem = (TextView)lstMyList.GetItemAtPosition(1);
				//lstItem.SetTextColor(Android.Graphics.Color.Yellow);
				//////////////
			}
			catch (Exception ex)
			{
				//Used for Debugging
				System.Diagnostics.Debug.Print("Error- " + ex.Message);
				throw (ex);
			}

		}

		private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
		{
			LoadFragment(e.Item.ItemId);
		}

		void LoadFragment(int id)
		{
			Android.Support.V4.App.Fragment fragment = null;

			switch (id)
			{

				case Resource.Id.menu_home:
					Intent iActivity = new Intent(this, typeof(AndroidActivity));

					iActivity.PutExtra("turnClassSerial", turnClassSerial);
					iActivity.PutExtra("UIClassSerial", UIClassSerial);

					StartActivity(iActivity);
					break;
				case Resource.Id.menu_chart:
					Intent ichartActivity = new Intent(this, typeof(OutChartActivity));

					ichartActivity.PutExtra("turnClassSerial", turnClassSerial);
					ichartActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(ichartActivity);

					break;
				case Resource.Id.menu_rules:
					Intent irulesActivity = new Intent(this, typeof(RulesActivity));

					irulesActivity.PutExtra("turnClassSerial", turnClassSerial);
					irulesActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(irulesActivity);
					break;
				case Resource.Id.menu_scores:
					
					break;
				case Resource.Id.menu_info:
					Intent iinfoActivity = new Intent(this, typeof(Activity5));

					iinfoActivity.PutExtra("turnClassSerial", turnClassSerial);
					iinfoActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(iinfoActivity);
					break;
			}
			if (fragment == null)
				return;

			SupportFragmentManager.BeginTransaction()
			   .Replace(Resource.Id.content_frame, fragment)
			   .Commit();
		}
	}
}