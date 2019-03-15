using System;
using Android.Media;
using Android.Speech;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using DartAssistant;
using Xamarin.Essentials;

//Class used for controlling audio
//Uses Interface IAudio
[assembly: Dependency(typeof(AudioService))]
namespace DartAssistant
{
	//Xamarin.Forms.Platform.Android.FormsAppCompatActivity, 
	public class AudioService : IAudio
	{
		private readonly int VOICE = 10;

		public AudioService()
		{
		}

		//Function for playing the selected audio files
		//File to play and whether to Loop audio are input parameters
		public string Listen(bool Ignore)
		{
			
			try
			{
				// check to see if we can actually record - if we can, assign the event to the button
				//string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
				//if (rec != "android.hardware.microphone")
				//{
				//	// no microphone, no recording. Return message to display to user
				//	 return "You don't seem to have a microphone to record with";
				//}
				//else if (!Ignore)

				//	{
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
				//}

				return "OK";
			}
			catch (Exception ex)
			{

				return "Error: " + ex.Message ;
			}
			
		}
		
		//Function used to Pause Listening
		public bool Pause(bool pause)
		{
			try
			{
				

				return true;
			}
			catch (Exception ex)
			{

				return false;
			}

		}

		//Not needed currently - handled by Xamarin.Essentials
		//Function for Text to Speech (TTS)
		//public bool SpeakText(string TextToSpeak)
		//{
		//	try
		//	{

		//		//TextToSpeech.SpeakAsync(TextToSpeak);

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{

		//		return false;
		//	}

		//}


	}
}