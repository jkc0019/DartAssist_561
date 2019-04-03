using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;

namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Stop It!")]
	public class Activity5 : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;

		string UIClassSerial = "";
		string turnClassSerial = "";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.NavMain);

			turnClassSerial = Intent.GetStringExtra("turnClassSerial");;
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

				bottomNavigation.Menu.GetItem(4).SetChecked(true);

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

				bottomNavigation.Menu.GetItem(4).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}

			base.OnResume();
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
					Intent iActivity = new Intent(this, typeof(AndroidNavActivity));

					iActivity.PutExtra("turnClassSerial", turnClassSerial);
					iActivity.PutExtra("UIClassSerial", UIClassSerial);

					StartActivity(iActivity);
					break;
				case Resource.Id.menu_chart:
					Intent ichartActivity = new Intent(this, typeof(OutChartNavActivity));

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
					Intent iscoresActivity = new Intent(this, typeof(Activity4));

					iscoresActivity.PutExtra("turnClassSerial", turnClassSerial);
					iscoresActivity.PutExtra("UIClassSerial", UIClassSerial);
					StartActivity(iscoresActivity);
					break;
				case Resource.Id.menu_info:
					
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