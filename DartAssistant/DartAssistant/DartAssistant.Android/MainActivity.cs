using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Util;
using Android.OS;
using Android.Speech;

namespace DartAssistant.Droid
{
    [Activity(Label = "DartAssistant", Icon = "@mipmap/icon", MainLauncher = true, Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private readonly int VOICE = 10;
		public static Context Context;
		public static string speakThis = "";

		//public static Activity thisActivity { get; set; }
		internal static MainActivity Instance { get; private set; }
		internal static Android.Content.Res.AssetManager assets { get; private set; }

		protected override void OnCreate(Bundle bundle)
		{
			//thisActivity = this;
			
			base.OnCreate(bundle);

			//Pass parameters on to Application
			global::Xamarin.Forms.Forms.Init(this, bundle);
			Instance = this;
			assets = this.Assets;
			Xamarin.Forms.DependencyService.Register<IActivityHelper, ActivityHelper>();
			LoadApplication(new App());

		}

		protected override void OnActivityResult(int requestCode, Result resultVal, Intent data)
		{
			//This is where the listener activity sends the voice recording info when it detects
			//that the current dictation has ended. The actual recognition happens here
			//and returns the score to look up to the MainPAge in the Xamarin shared code
			//for converting to words and Text-to-Speech
			if (requestCode == VOICE)
			{
				if (resultVal == Result.Ok)
				{
					
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						string textInput = ""; 
						textInput = matches[0];

						// limit the output to 100 characters
						if (textInput.Length > 100)
							textInput = textInput.Substring(0, 100);
						
						MainPage pgMainPage = new MainPage();
						Int16 TotalScore = 0;
						Int16 SingleScore = 0;

						string fmtInput = textInput.Trim().ToLower();

						if (fmtInput.Contains("out"))
						{
							fmtInput = textInput.Substring(0, textInput.Length - textInput.IndexOf("out"));

							bool Result = false;
							Result = Int16.TryParse(fmtInput, out TotalScore);

							//If not an Int set an out of range value for Total Score
							if (true != Result)
							{
								TotalScore = 200;
							}

							speakThis = pgMainPage.RecommendedOut(TotalScore);
							//speakThis = "Speak Now";

						}
						else if (fmtInput.Contains("scored"))
						{
							//TODO 
							//implement this after establishing total score tracking
							fmtInput = textInput.Substring(0, textInput.Length - textInput.IndexOf("score"));

							bool Result = false;
							Result = Int16.TryParse(fmtInput, out SingleScore);

							// If not an Int set an out of range value for Total Score
 							if (true != Result)
							{
								SingleScore = -1;
							}

							speakThis = pgMainPage.RecommendedOut(SingleScore);

						}


						pgMainPage.ReturnText(textInput);
						
					}
					
				}
			}

			base.OnActivityResult(requestCode, resultVal, data);
		}
		protected override void OnDestroy()
		{
			Log.Debug(GetType().FullName, "MainActivity - On Destroy");
			base.OnDestroy();
		}

		protected override void OnPause()
		{
			Log.Debug(GetType().FullName, "MainActivity - OnPause");
			base.OnPause();
		}

		protected override void OnRestart()
		{
			Log.Debug(GetType().FullName, "MainActivity - OnRestart");
			base.OnRestart();
		}

		protected override void OnResume()
		{
			Log.Debug(GetType().FullName, "MainActivity - OnResume");
			base.OnResume();
		}

		protected override void OnStart()
		{
			Log.Debug(GetType().FullName, "MainActivity - OnStart");
			base.OnStart();
		}

		protected override void OnStop()
		{
			Log.Debug(GetType().FullName, "MainActivity - OnStop");
			base.OnStop();
		}

	}
}