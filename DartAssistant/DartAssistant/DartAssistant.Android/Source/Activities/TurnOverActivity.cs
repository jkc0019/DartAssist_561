using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Xamarin.Essentials;

namespace DartAssistant.Droid
{
    [Activity(Theme = "@style/MyTheme.TurnOver", NoHistory = true)]
    public class TurnOverActivity : AppCompatActivity
    {
		bool isMuted = false;
		int outScore = 0;
		bool InitialStartupOver = true;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
			outScore = Intent.GetIntExtra("OutScore", 0);
			isMuted = Intent.GetBooleanExtra("IsMuted", false);
			InitialStartupOver = Intent.GetBooleanExtra("InitialStartOver", true);
		}

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
			outScore = Intent.GetIntExtra("OutScore", 0);
			isMuted = Intent.GetBooleanExtra("IsMuted", false);
			InitialStartupOver = Intent.GetBooleanExtra("InitialStartOver", true);

			try
			{
				Toast.MakeText(this, "Last Score " + outScore.ToString(), ToastLength.Long).Show();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.Message);
			}

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
				try
				{
					if (!isMuted)
					{
						TextToSpeech.SpeakAsync("Last Score " + outScore.ToString() + ". Turn Over!");
					}
					
				}
				catch (System.Exception ex)
				{
					System.Diagnostics.Debug.Print(ex.Message);
				}
			}
			else
			{
				if (!isMuted)
				{
					TextToSpeech.SpeakAsync("Turn Over!");
				}
				
			}

			await Task.Delay(3000);
			Intent iActivity = new Intent(this, typeof(Source.Activities.AndroidActivity));

			iActivity.PutExtra("IsMuted", isMuted);
			iActivity.PutExtra("InitialStartOver", InitialStartupOver);

			StartActivity(iActivity);
			
		}
    }
}