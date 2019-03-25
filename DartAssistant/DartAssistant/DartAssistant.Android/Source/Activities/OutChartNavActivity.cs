using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Out Chart")]
	public class OutChartNavActivity : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.NavOutChart);
			
			//Button button = FindViewById<Button>(Resource.Id.btn_Back);
			//button.Click += delegate {
			//	StartActivity(typeof(AndroidActivity));
			//};

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
					StartActivity(typeof(AndroidNavActivity));
					break;
				case Resource.Id.menu_audio:
					break;
				case Resource.Id.menu_video:
					StartActivity(typeof(Activity3));
					break;
				case Resource.Id.menu_scores:
					StartActivity(typeof(Activity4));
					break;
				case Resource.Id.menu_info:
					StartActivity(typeof(Activity5));
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
			List<String> allOutsList = new List<String>();
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
			lstMyList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, allOutsList);

		}
	}
}