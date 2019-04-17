using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;

namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Out Chart")]
	public class OutChartActivity : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;

		string UIClassSerial = "";
		string turnClassSerial = "";
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.OutChart);

			Log.Debug(GetType().FullName, "Activity B - OnCreate");
			turnClassSerial = Intent.GetStringExtra("turnClassSerial");
			System.Diagnostics.Debug.Print("-" + turnClassSerial);

			UIClassSerial = Intent.GetStringExtra("UIClassSerial");
			System.Diagnostics.Debug.Print("-" + UIClassSerial);

			// Create your application here
			InitializeOutChart();

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

				bottomNavigation.Menu.GetItem(1).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}
		}

		protected override void OnRestart()
		{
			Log.Debug(GetType().FullName, "Activity B - OnRestart");
			base.OnRestart();
		}

		protected override void OnStart()
		{
			Log.Debug(GetType().FullName, "Activity B - OnStart");
			base.OnStart();
		}

		protected override void OnResume()
		{
			Log.Debug(GetType().FullName, "Activity B - OnResume");

			try
			{

				bottomNavigation.Menu.GetItem(1).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}

			base.OnResume();
		}

		protected override void OnPause()
		{
			Log.Debug(GetType().FullName, "Activity B - OnPause");
			base.OnPause();
		}

		protected override void OnStop()
		{
			Log.Debug(GetType().FullName, "Activity B - OnStop");
			base.OnStop();
		}

		protected override void OnDestroy()
		{
			Log.Debug(GetType().FullName, "Activity B - OnDestroy");
			base.OnDestroy();
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
					break;
				case Resource.Id.menu_rules:
					Intent irulesActivity = new Intent(this, typeof(RulesActivity));

					irulesActivity.PutExtra("turnClassSerial", turnClassSerial);
					irulesActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(irulesActivity);
					break;
				case Resource.Id.menu_scores:
					Intent iscoresActivity = new Intent(this, typeof(TopScoresActivity));

					iscoresActivity.PutExtra("turnClassSerial", turnClassSerial);
					iscoresActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(iscoresActivity);
					break;

			}
			if (fragment == null)
				return;

			SupportFragmentManager.BeginTransaction()
			   .Replace(Resource.Id.content_frame, fragment)
			   .Commit();
		}

		private void InitializeOutChart()
		{
			OutCalculator outCalculator = new OutCalculator(InOutRule.Double);
			List<string> allOutsList = new List<string>();
			Dictionary<int, List<Dart>> allOuts = outCalculator.GetAllOuts();
			// iterate through the dictionary to get all the outs
			foreach (KeyValuePair<int, List<Dart>> entry in allOuts)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(entry.Key.ToString());
				stringBuilder.Append(": ");

				for (int j = 0; j < entry.Value.Count; j++)
				{
					stringBuilder.Append(entry.Value[j].Abbreviation);
					if (j != entry.Value.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}

				string outItemText = stringBuilder.ToString();
				allOutsList.Add(outItemText);
			}

			Android.Widget.ListView lstMyList = (Android.Widget.ListView)FindViewById<Android.Widget.ListView>(Resource.Id.MyList);
			lstMyList.Adapter = new OutChartAdapter(this, allOutsList);

		}
	}
}