using Android.App;
using Android.OS;

using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Scores")]
	public class Activity4 : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.NavMain);

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

				bottomNavigation.Menu.GetItem(3).SetChecked(true);

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
				case Resource.Id.menu_chart:
					StartActivity(typeof(OutChartNavActivity));
					break;
				case Resource.Id.menu_rules:
					StartActivity(typeof(Activity3));
					break;
				case Resource.Id.menu_scores:
					
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
	}
}