using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Xamarin.Essentials;

namespace DartAssistant.Droid
{
    [Activity(Theme = "@style/MyTheme.Bust", NoHistory = true)]
    public class BustActivity : AppCompatActivity
    {
		bool isMuted = false;
		bool InitialStartupOver = true;

		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
			isMuted = Intent.GetBooleanExtra("IsMuted", false);
			InitialStartupOver = Intent.GetBooleanExtra("InitialStartOver", true);
		}

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
			isMuted = Intent.GetBooleanExtra("IsMuted", false);
			InitialStartupOver = Intent.GetBooleanExtra("InitialStartOver", true);

			Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup ()
        {
			if (!isMuted)
			{
				TextToSpeech.SpeakAsync("Hands Up! You're Busted!");
			}
			
			await Task.Delay(3000);
			Intent iActivity = new Intent(this, typeof(Source.Activities.AndroidActivity));

			iActivity.PutExtra("IsMuted", isMuted);
			iActivity.PutExtra("InitialStartOver", InitialStartupOver);

			StartActivity(iActivity);
			
		}
    }
}