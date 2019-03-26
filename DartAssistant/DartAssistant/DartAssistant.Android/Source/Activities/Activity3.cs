using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;


namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Rules")]
	public class Activity3 : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;
		internal static Android.Content.Res.AssetManager assets { get; private set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.NavRules);

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

				bottomNavigation.Menu.GetItem(0).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}


			assets = this.Assets;
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
				
				case Resource.Id.menu_back:
					StartActivity(typeof(AndroidNavActivity));
					break;
				case Resource.Id.menu_OutGames:
					fragment = fragRules1.NewInstance();
					break;
				case Resource.Id.menu_cricket:
					fragment = fragRules2.NewInstance();
					break;
			}
			if (fragment == null)
				return;

			SupportFragmentManager.BeginTransaction()
			   .Replace(Resource.Id.content_frame, fragment)
			   .Commit();
		}

		public List<string> GetStoredInfo(string infoFile)
		{
			//Used to get the contents from the Android Assests and return a List

			List<string> infoFromFile = new List<string>();
			string content;

			try
			{
				//AndroidNavActivity activity = new AndroidNavActivity();
				//Android.Content.Res.AssetManager assets = AndroidNavActivity.assets;

				using (StreamReader sr = new StreamReader(assets.Open(infoFile)))

				{

					while ((content = sr.ReadLine()) != null)
					{

						infoFromFile.Add(content.ToString());

					}
				}

				return infoFromFile;
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