using System;
using System.Collections.Generic;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Webkit;
using System.Text;


namespace DartAssistant.Droid.Source.Activities
{
	[Activity(Label = "Rules")]
	public class RulesActivity : AppCompatActivity
	{
		BottomNavigationView bottomNavigation;
		internal static Android.Content.Res.AssetManager assets { get; private set; }

		string UIClassSerial = "";
		string turnClassSerial = "";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.Rules);

            turnClassSerial = Intent.GetStringExtra("turnClassSerial");
            System.Diagnostics.Debug.Print("-" + turnClassSerial);

            UIClassSerial = Intent.GetStringExtra("UIClassSerial");
            System.Diagnostics.Debug.Print("-" + UIClassSerial);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			if (toolbar != null)
			{
				SetSupportActionBar(toolbar);
				SupportActionBar.SetDisplayHomeAsUpEnabled(false);
				SupportActionBar.SetHomeButtonEnabled(false);

			}

            InitializeRules();

			bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);


			bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

			try
			{

				bottomNavigation.Menu.GetItem(2).SetChecked(true);

			}
			catch (System.Exception ex)
			{

				System.Diagnostics.Debug.Print("Heya:" + ex.Message);
			}


			assets = this.Assets;
		}

		protected override void OnResume()
		{

			try
			{

				bottomNavigation.Menu.GetItem(2).SetChecked(true);

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
            Bundle mBundle = new Bundle();

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

        public List<string> GetStoredInfo(string infoFile)
		{
			//Used to get the contents from the Android Assests and return a List

			List<string> infoFromFile = new List<string>();
			string content;

			try
			{
				//AndroidNavActivity activity = new AndroidNavActivity();

				using (StreamReader sr = new StreamReader(Android.App.Application.Context.Assets.Open(infoFile)))

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

        private void InitializeRules()
        {
            Android.Webkit.WebView webView = FindViewById<WebView>(Resource.Id.wvw_Rules);

            //webView.LoadUrl("https://google.com");

            //webView.Settings.JavaScriptEnabled = true;
            //webView.SetWebViewClient(new WebViewClient());

            //<><><><><><><><><><><><><><><><><><><><><><><><><>
            //Use this to return content of a text file as a List from
            //Files included as Assets in the OS-specific native Project. Pass Filename only with extension (i.e. DartRules.txt)
            List<string> infoRequested = GetStoredInfo("DartRules.txt");

            StringBuilder sb = new StringBuilder();

            foreach (string strOut in infoRequested)
            {
                System.Diagnostics.Debug.Print(strOut);

                if (strOut.Trim().Length > 0)
                {
                    sb.Append("<p>" + strOut + "</p>");
                }                

            }

            webView.LoadDataWithBaseURL(null, @"<html><head><style>body{padding:0 15px 50px;background-color:#FEE4BE;}p{font-size:16px;line-height:1.5;}</style></head><body><h1>301 / 501</h1>" + sb.ToString() + "</body></html>", "text/html", "utf-8", null);
        }
	}
}