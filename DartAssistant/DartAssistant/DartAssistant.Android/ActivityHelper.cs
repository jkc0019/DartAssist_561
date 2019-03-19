using Android.App;
using Android.Content;
using Android.Speech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;


namespace DartAssistant.Droid
{
    public class ActivityHelper : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IActivityHelper
	{
		private readonly int VOICE = 10;
		public string SpokeText = "";

		SpeechRecognizer Recognizer { get; set; }

		public string GetVersionNumber()
        {
            var versionNumber = string.Empty;
            if (MainApplication.CurrentContext != null)
            {
                versionNumber = MainApplication.CurrentContext.PackageManager.GetPackageInfo(MainApplication.CurrentContext.PackageName, 0).VersionName;
            }
            return versionNumber;
        }
		public string Listen()
		{
			//var recListener = new RecognitionListener();
			//https://stackoverflow.com/questions/48934239/c-sharp-xamarin-speech-recognition-in-the-background-android

			// create the intent and start the activity
			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

			// put a message on the modal dialog
			voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Listening");

			// if there is more then 1.5s of silence, consider the speech over
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
			
			// you can specify other languages recognised here, for example
			// voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
			// if you wish it to recognise the default Locale language and German
			// if you do use another locale, regional dialects may not be recognised very well

			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
			MainActivity.Instance.StartActivityForResult(voiceIntent, VOICE);

			return SpokeText;
		}

		public List<string> GetStoredInfo(string infoFile)
		{
			//Used to get the contents from the Android Assests and retun a List

			List<string> infoFromFile = new List<string>();
			string content;

			try
			{
				MainActivity activity = new MainActivity();
				Android.Content.Res.AssetManager assets = MainActivity.assets;

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
				Debug.Print("Error- " + ex.Message);
				throw (ex);
			}

		}
		
	}

}
