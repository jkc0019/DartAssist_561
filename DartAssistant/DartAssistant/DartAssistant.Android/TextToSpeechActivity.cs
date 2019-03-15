using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Speech.Tts;
using Android.Speech;
using System.Collections.Generic;
using System.Linq;

namespace DartAssistant.Droid
{
	[Activity(Label = "TextToSpeechActivity")]
	public class TextToSpeechActivity : Activity, TextToSpeech.IOnInitListener
	{
		TextToSpeech textToSpeech;
		private readonly int NeedLang = 103;
		Java.Util.Locale lang;

		//Speech Recognition
		private bool isRecording;
		private readonly int VOICE = 10;
		private TextView textBox;
		private Button recButton;
		//

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			InitSpeaking();
			BeginListening();
			return;

	
		}

			private void InitSpeaking()
		{
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.MainTTs);

			var editWhatToSay = textBox; 
			
			// set up the TextToSpeech object
			// third parameter is the speech engine to use
			textToSpeech = new TextToSpeech(this, this, "com.google.android.tts");

			// set the top option to be default
			var langAvailable = new List<string> { "Default" };

			// our spinner only wants to contain the languages supported by the tts and ignore the rest
			var localesAvailable = Java.Util.Locale.GetAvailableLocales().ToList();
			foreach (var locale in localesAvailable)
			{
				LanguageAvailableResult res = textToSpeech.IsLanguageAvailable(locale);
				switch (res)
				{
					case LanguageAvailableResult.Available:
						langAvailable.Add(locale.DisplayLanguage);
						break;
					case LanguageAvailableResult.CountryAvailable:
						langAvailable.Add(locale.DisplayLanguage);
						break;
					case LanguageAvailableResult.CountryVarAvailable:
						langAvailable.Add(locale.DisplayLanguage);
						break;
				}

			}

			// set up the speech to use the default langauge
			// if a language is not available, then the default language is used.
			lang = Java.Util.Locale.Default;
			textToSpeech.SetLanguage(lang);

			// set the speed and pitch
			textToSpeech.SetPitch(.5f);
			textToSpeech.SetSpeechRate(.5f);
			var pitch = 150 / 255f;
			textToSpeech.SetPitch(pitch);
			textToSpeech.SetSpeechRate(pitch);
			
		}

		// Interface method required for IOnInitListener
		void TextToSpeech.IOnInitListener.OnInit(OperationResult status)
		{
			// if we get an error, default to the default language
			if (status == OperationResult.Error)
				textToSpeech.SetLanguage(Java.Util.Locale.Default);
			// if the listener is ok, set the lang
			if (status == OperationResult.Success)
				textToSpeech.SetLanguage(lang);
		}

		protected override void OnActivityResult(int req, Result res, Intent data)
		{
			if (req == NeedLang)
			{
				// we need a new language installed
				var installTTS = new Intent();
				installTTS.SetAction(TextToSpeech.Engine.ActionInstallTtsData);
				StartActivity(installTTS);
			}

			if (req == VOICE)
			{
				if (res == Result.Ok)
				{
					var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
					if (matches.Count != 0)
					{
						string textInput = textBox.Text + matches[0];

						// limit the output to 500 characters
						if (textInput.Length > 500)
							textInput = textInput.Substring(0, 500);
						textBox.Text = textInput;
					}
					else
						textBox.Text = "No speech was recognised";
					// change the text back on the button
					recButton.Text = "Start Recording";

					//Just Added
					isRecording = false;
				}
			}
		}

		private void BeginListening()
		{
			// set the isRecording flag to false (not recording)
			isRecording = false;

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			// get the resources from the layout
			recButton = FindViewById<Button>(Resource.Id.btnRecord);
			textBox = FindViewById<TextView>(Resource.Id.textYourText);

			// check to see if we can actually record - if we can, assign the event to the button
			string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
			if (rec != "android.hardware.microphone")
			{
				// no microphone, no recording. Disable the button and output an alert
				var alert = new AlertDialog.Builder(recButton.Context);
				alert.SetTitle("You don't seem to have a microphone to record with");
				alert.SetPositiveButton("OK", (sender, e) =>
				{
					textBox.Text = "No microphone present";
					recButton.Enabled = false;
					return;
				});

				alert.Show();
			}
			else
				recButton.Click += delegate
				{
					// change the text on the button
					recButton.Text = "End Recording";

					//reset Text
					textBox.Text = "";

					isRecording = !isRecording;
					if (isRecording)
					{
						// create the intent and start the activity
						var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
						voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

						// put a message on the modal dialog
						voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.messageSpeakNow));

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
						StartActivityForResult(voiceIntent, VOICE);
					}
				};

			var btnSayIt = FindViewById<Button>(Resource.Id.btnSpeakNow);

			btnSayIt.Click += delegate
			{
				System.Diagnostics.Debug.Print("Heya-" + textBox.Text);

				// if there is nothing to say, don't say it
				if (!string.IsNullOrEmpty(textBox.Text))
				{
					textToSpeech.Speak(textBox.Text, QueueMode.Flush, null);
				}
					

			};

		}
	}
}