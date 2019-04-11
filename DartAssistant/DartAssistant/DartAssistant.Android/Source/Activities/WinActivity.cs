﻿using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.Essentials;

namespace DartAssistant.Droid
{
    [Activity(Theme = "@style/MyTheme.Win", NoHistory = true)]
    public class WinActivity : AppCompatActivity
    {
		bool isMuted = false;
		int outScore = 0;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
			outScore = Intent.GetIntExtra("OutScore", 0);
			isMuted = Intent.GetBooleanExtra("IsMuted", false);
		}

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
			outScore = Intent.GetIntExtra("OutScore", 0);
			isMuted = Intent.GetBooleanExtra("IsMuted", false);

			Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup ()
        {
			if (outScore != 0)
			{
				if ((double)outScore / 2 == (int)outScore / 2)
				{
					string outText = ((int)outScore / 2).ToString();
					TextToSpeech.SpeakAsync("Winner! Double " + outText + " Out!");
				}
				else
				{
					TextToSpeech.SpeakAsync("Winner, Winner, Chicken Dinner!");
				}
				
			}
			else
			{
				TextToSpeech.SpeakAsync("Winner, Winner, Chicken Dinner!");
			}
			
			await Task.Delay(3000);
			Intent iActivity = new Intent(this, typeof(Source.Activities.AndroidActivity));

			iActivity.PutExtra("IsMuted", isMuted);

			StartActivity(iActivity);
			
		}
    }
}