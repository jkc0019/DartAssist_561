﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Speech;
//using Android.Util;
using Android.Widget;
using Android.Media;
using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Threading.Tasks;

namespace DartAssistant.Droid.Source.Activities
{
	//, MainLauncher = true
	[Activity(Label = "@string/app_name",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class AndroidActivity : AppCompatActivity
	{
        RecognitionListener recListener;
        SpeechRecognizer Recognizer { get; set; }
		Intent SpeechIntent { get; set; }

		bool isListeningPaused = false;
		int seconds = 0;
		int pauseTime = -8;
		bool singleUse = false;
		bool singleOut = true;

		private AudioManager mAudioManager;
		private int mStreamVolume = 0;
		private bool isMuted = false;

		//Variables for Maintaining State
		int startingScore = 0;
		int currentScore = 0;
		string turnClassSerial = "";
		string UIClassSerial = "";

		//Collection for debugging messages
		System.Collections.Generic.List<string> mList = new System.Collections.Generic.List<string>();

		//Nav menu
		BottomNavigationView bottomNavigation;

		//Turn Class instantiation
		Turn clsTurn = new Turn(InOutRule.Double);
		//Class to store some UI info
		UIState clsUIState = new UIState();
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			
			SetContentView(Resource.Layout.Game);

			//Logging
			//Log.Debug(GetType().FullName, "Activity A - OnCreate");

			turnClassSerial = Intent.GetStringExtra("turnClassSerial");
			UIClassSerial = Intent.GetStringExtra("UIClassSerial");

			var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
			var txtStartScore = FindViewById<Android.Widget.EditText>(Resource.Id.StartScore);
			var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
			var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);

			ImageView imgRecord;
			imgRecord = FindViewById<Android.Widget.ImageView>(Resource.Id.imgRecord);
			imgRecord.Visibility = Android.Views.ViewStates.Invisible;

			Android.Widget.Switch swcMuteSwitch = FindViewById<Android.Widget.Switch>(Resource.Id.MuteSwitch);
			swcMuteSwitch.CheckedChange += swcMuteSwitch_Click;

			//Saved Instance Takes precedence
			if (savedInstanceState != null)
			{
				string savedTurn = savedInstanceState.GetString("jsonSerialTurn", "");
				clsTurn = JsonConvert.DeserializeObject<Turn>(savedTurn);

				System.Diagnostics.Debug.Print("-" + clsTurn.CurrentScore.ToString());

				string savedUIState = savedInstanceState.GetString("jsonSerialUIState", "");
				clsUIState = JsonConvert.DeserializeObject<UIState>(savedUIState);

				System.Diagnostics.Debug.Print("-" + clsUIState.LastScore.ToString());

				if (TurnState.InProgress == clsTurn.State)
				{
					txtDartScore.Text = clsUIState.LastScore.ToString();
					txtStartScore.Text = clsTurn.StartingScore.ToString();
					txtNewOut.Text = clsUIState.CurrentScoreText.ToString();
					txtOutTurn.Text = clsUIState.DoubleOutText.ToString();

					currentScore = clsTurn.CurrentScore;
				}
				else
				{
					txtDartScore.Text = "";
					txtStartScore.Text = "";
					txtNewOut.Text = "";
					txtOutTurn.Text = "";

					currentScore = 0;
				}

				isMuted = savedInstanceState.GetBoolean("IsMuted", false);

				if (true == isMuted)
				{
					swcMuteSwitch.Checked = true;
				}

				//Log.Debug(GetType().FullName, "AndroidNavActivity - Recovered instance state");
			}
			else if (null != turnClassSerial && 0 < turnClassSerial.Trim().Length)
			{
				clsTurn = JsonConvert.DeserializeObject<Turn>(turnClassSerial);

				System.Diagnostics.Debug.Print("-" + clsTurn.CurrentScore.ToString());

				clsUIState = JsonConvert.DeserializeObject<UIState>(UIClassSerial);

				System.Diagnostics.Debug.Print("-" + clsUIState.LastScore.ToString());

				if (TurnState.InProgress == clsTurn.State)
				{
					txtDartScore.Text = clsUIState.LastScore.ToString();
					txtStartScore.Text = clsTurn.StartingScore.ToString();
					txtNewOut.Text = clsUIState.CurrentScoreText.ToString();
					txtOutTurn.Text = clsUIState.DoubleOutText.ToString();

					currentScore = clsTurn.CurrentScore;
				}
				else
				{
					txtDartScore.Text = "";
					txtStartScore.Text = "";
					txtNewOut.Text = "";
					txtOutTurn.Text = "";

					currentScore = 0;
				}

				isMuted = clsUIState.IsMuted;

				if (true == isMuted)
				{
					swcMuteSwitch.Checked = true;
				}

				//Log.Debug(GetType().FullName, "AndroidNavActivity - Loaded Serial");
			}
			else
			{
				isMuted = Intent.GetBooleanExtra("IsMuted", false);

				if (true == isMuted)
				{
					swcMuteSwitch.Checked = true;
				}
			}

			Android.Widget.Button BtnStartSpeech = FindViewById<Android.Widget.Button>(Resource.Id.btn_start_game);
			BtnStartSpeech.Click += BtnStartSpeech_Click;
			BtnStartSpeech.Visibility = Android.Views.ViewStates.Gone;

			recListener = new RecognitionListener();
			recListener.BeginSpeech += RecListener_BeginSpeech;
			recListener.EndSpeech += RecListener_EndSpeech;
            recListener.Error += RecListener_Error;
            recListener.Ready += RecListener_Ready;
            recListener.Recognized += RecListener_Recognized;
            
            CreateSpeechRecognizer();

			var BtnStartTurn = FindViewById<Android.Widget.Button>(Resource.Id.btn_StartTurn);
             BtnStartTurn.Click += BtnStartTurn_Click;
            
            var BtnDartScored = FindViewById<Android.Widget.Button>(Resource.Id.btn_DartScored);
			BtnDartScored.Click += BtnDartScored_Click;

			var BtnGetOut = FindViewById<Android.Widget.Button>(Resource.Id.btn_GetOut);
			BtnGetOut.Click += BtnGetOut_Click;

			var BtnClearOut = FindViewById<Android.Widget.Button>(Resource.Id.btn_Clear);
			BtnClearOut.Click += BtnClearOut_Click;
			BtnClearOut.Visibility = Android.Views.ViewStates.Gone;

			var txtYourScore = FindViewById<Android.Widget.EditText>(Resource.Id.YourScore);
			txtYourScore.Click += txtYourScore_Click;

			//Log.Debug(nameof(AndroidActivity), nameof(OnCreate));
			mList.Add("Start");
			Forms.Init(this, savedInstanceState);

			mAudioManager = (AudioManager)GetSystemService(Context.AudioService);

			if (false == isMuted)
			{
				mStreamVolume = mAudioManager.GetStreamVolume(Stream.Music); // getting system volume into var for later un-muting 

				if (mStreamVolume == 0)
				{
					mStreamVolume = 8;

					mAudioManager.SetStreamVolume(Stream.Music, mStreamVolume, 0); // setting system volume in case it was left at 0
				}

			}

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

		}

        private void CreateSpeechRecognizer()
        {
            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this);
            Recognizer.SetRecognitionListener(recListener);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, PackageName);
        }

        private void RecListener_Error(object sender, SpeechRecognizerError e)
        {
            if (SpeechRecognizerError.SpeechTimeout == e)
            {
                Recognizer.Destroy();

                CreateSpeechRecognizer();

                Recognizer.StartListening(SpeechIntent);
            }
        }

        private void OnContinue(Task t)
        {
			mAudioManager.SetStreamVolume(Stream.Music, 0, 0); // setting system volume to zero, muting

			Recognizer.StartListening(SpeechIntent);
        }

        protected override void OnSaveInstanceState(Bundle outsInstanceState)
		{

			string savedTurn = JsonConvert.SerializeObject(clsTurn);
			outsInstanceState.PutString("jsonSerialTurn", savedTurn);

			string savedUIState = JsonConvert.SerializeObject(clsUIState);
			outsInstanceState.PutString("jsonSerialUIState", savedUIState);

			outsInstanceState.PutBoolean("IsMuted", isMuted);

			//Log.Debug(GetType().FullName, "AndroidNavActivity- Saving instance state");

			// always call the base implementation!
			base.OnSaveInstanceState(outsInstanceState);
		}

		protected override void OnResume()
		{
			//Log.Debug(GetType().FullName, "Activity A - OnResume");

			try
			{

				bottomNavigation.Menu.GetItem(0).SetChecked(true);

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
			string turnClassSerial = JsonConvert.SerializeObject(clsTurn);
			string UIClassSerial = JsonConvert.SerializeObject(clsUIState);

			Android.Support.V4.App.Fragment fragment = null;

			switch (id)
			{
				case Resource.Id.menu_home:
					break;
				case Resource.Id.menu_chart:
					Intent ichartActivity = new Intent(this, typeof(OutChartActivity));

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

		private void BtnStartSpeech_Click(object sender, System.EventArgs e)
		{
			singleUse = false;
			singleOut = true;

			HideKeyboard();

			Recognizer.StartListening(SpeechIntent);

			mStreamVolume = mAudioManager.GetStreamVolume(Stream.Music); // getting system volume into var for later un-muting 

			if (mStreamVolume == 0)
			{
				mStreamVolume = 9;
			}

		}

		private void txtYourScore_Click(object sender, System.EventArgs e)
		{
			EditText eText = (EditText)sender;
			eText.Text = "";
		}

		private void swcMuteSwitch_Click(object sender, CompoundButton.CheckedChangeEventArgs e)
		{
			if (e.IsChecked == true)
			{
				Toast.MakeText(this, "Muted", ToastLength.Long).Show();
				isMuted = true;
				pauseTime = -9;
			}
			else
			{
				Toast.MakeText(this, "Sound On", ToastLength.Long).Show();
				isMuted = false;
				pauseTime = -8;
			}

			clsUIState.IsMuted = isMuted;
		}

		private void BtnStartTurn_Click(object sender, System.EventArgs e)
		{
			singleUse = false;
			singleOut = false;

			string strRecommendedOut = "";

			HideKeyboard();
			
			var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
			txtDartScore.Text = "";

			//Clear values for the Get Out objects
			Android.Widget.TextView txtOutLabel = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutLabel);
			txtOutLabel.Text = "";
			var txtYourScore = FindViewById<Android.Widget.EditText>(Resource.Id.YourScore);
			txtYourScore.Text = "";

			var txtStartScore = FindViewById<Android.Widget.EditText>(Resource.Id.StartScore);

			if (txtStartScore.Text.Trim() != "")
			{
				
				bool Result = false;
				Result = int.TryParse(txtStartScore.Text, out startingScore);

				if (true != Result)
				{
					startingScore = -1;
					strRecommendedOut = "Unknown Score";
				}
				else
				{
					clsTurn.DartsRemaining = 3;
					strRecommendedOut = RecommendedOut(startingScore);
				}

				bool mbOK = clsTurn.SetStartingScore(startingScore);

				var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
				txtNewOut.Text = txtStartScore.Text + " Left, Darts Remaining: 3";

				var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
				txtOutTurn.Text = "(" + txtStartScore.Text + ") " + strRecommendedOut;
				
				currentScore = startingScore;
				clsUIState.CurrentScoreText = txtNewOut.Text;
				clsUIState.DoubleOutText = txtOutTurn.Text;
				
				AsyncSpeak(txtStartScore.Text + " Starting Score", strRecommendedOut, true);
			}
			else
			{
				Recognizer.StopListening();
				
				isListeningPaused = false;

				Recognizer.StartListening(SpeechIntent);

				mStreamVolume = mAudioManager.GetStreamVolume(Stream.Music); // getting system volume into var for later un-muting 

				if (mStreamVolume == 0)
				{
					mStreamVolume = 9;
				}

				mAudioManager.SetStreamVolume(Stream.Music, 0, 0); // setting system volume to zero, muting

				var BtnStartTurn = FindViewById<Android.Widget.Button>(Resource.Id.btn_StartTurn);
				BtnStartTurn.Enabled = false;
				var BtnGetOut = FindViewById<Android.Widget.Button>(Resource.Id.btn_GetOut);
				BtnGetOut.Enabled = false;

			}
			
		}

		private void BtnDartScored_Click(object sender, System.EventArgs e)
		{
			singleUse = false;
			string strNewOut = "";

			HideKeyboard();

			var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);

			int dartScore = 0;

			if (txtDartScore.Text.Trim() != "")
			{
				
				bool Result = false;
				Result = int.TryParse(txtDartScore.Text, out dartScore);

				if (true != Result)
				{
					dartScore = -1;
				}
				
				clsUIState.LastScore = dartScore;

				strNewOut = GetNewScore(txtDartScore.Text);
				
				var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
				txtNewOut.Text = strNewOut;
				
				if (1 == (int)clsTurn.State)
				{
					clsUIState.CurrentScoreText = strNewOut;
				}
			}

			if (strNewOut == "Win")
			{
				Intent iActivity = new Intent(this, typeof(WinActivity));
				
				iActivity.PutExtra("OutScore", dartScore);
				iActivity.PutExtra("IsMuted", isMuted);

				StartActivity(iActivity);
				
			}
			else if (strNewOut == "Bust")
			{
				Intent iActivity = new Intent(this, typeof(BustActivity));
				
				iActivity.PutExtra("IsMuted", isMuted);

				StartActivity(iActivity);

			}
			else if (strNewOut == "TurnOver")
			{
				Intent iActivity = new Intent(this, typeof(TurnOverActivity));

				iActivity.PutExtra("OutScore", clsTurn.CurrentScore);
				iActivity.PutExtra("IsMuted", isMuted);

				StartActivity(iActivity);

			}
			else if (clsTurn.State == TurnState.InProgress)
			{
				
				string strRecommendedOut = "";
				strRecommendedOut = RecommendedOut(clsTurn.CurrentScore);
				var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
				txtOutTurn.Text = "(" + clsTurn.CurrentScore.ToString() + ") " + strRecommendedOut;

				AsyncSpeak(strNewOut, strRecommendedOut, true);
			}
			
		}

		private void BtnGetOut_Click(object sender, System.EventArgs e)
		{
			Android.Widget.TextView txtOutLabel = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutLabel);
			var txtYourScore = FindViewById<Android.Widget.EditText>(Resource.Id.YourScore);
			
			string scoreStr = txtYourScore.Text;
			int score = 0;
			string text = "";

			HideKeyboard();

			//Clear values for the Turn objects
			var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
			txtNewOut.Text = "";
			var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
			txtOutTurn.Text = "";
			var txtStartScore = FindViewById<Android.Widget.EditText>(Resource.Id.StartScore);
			txtStartScore.Text = "";
			var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
			txtDartScore.Text = "";

			if (scoreStr.Trim().Length > 0)
			{
				bool Result = false;
				Result = int.TryParse(scoreStr, out score);

				//If not an Int
				if (true != Result)
				{
					text = "Invalid Out Number";
				}
				else
				{
					if (170 < score | 2 > score)
					{
						text = "Invalid Out Number";
					}
					else
					{

						text = GetAbbrevOut(score);
					}

				}
				if ("" == text)
				{
					text = "Unknown Out";
				}

				txtOutLabel.Text = "(" + txtYourScore.Text + ") " + text;
				txtYourScore.Text = "";

				string strRecommendedOut = RecommendedOut(score,true);
				AsyncSpeak(strRecommendedOut,"", true);
			}
			else
			{
				singleUse = false;
				singleOut = true;

				Recognizer.StartListening(SpeechIntent);

				mStreamVolume = mAudioManager.GetStreamVolume(Stream.Music); // getting system volume into var for later un-muting 

				if (mStreamVolume == 0)
				{
					mStreamVolume = 9;
				}
				mAudioManager.SetStreamVolume(Stream.Music, 0, 0); // setting system volume to zero, muting

				var BtnGetOut = FindViewById<Android.Widget.Button>(Resource.Id.btn_GetOut);
				BtnGetOut.Enabled = false;
				var BtnStartTurn = FindViewById<Android.Widget.Button>(Resource.Id.btn_StartTurn);
				BtnStartTurn.Enabled = false;

			}
			
		}
		private void BtnClearOut_Click(object sender, System.EventArgs e)
		{
			HideKeyboard();

			var txtOutLabel = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutLabel);
			var txtYourScore = FindViewById<Android.Widget.EditText>(Resource.Id.YourScore);

			txtOutLabel.Text = "";
			txtYourScore.Text = "";

		}
		private void RecListener_Ready(object sender, Bundle e)
		{
			//Log.Debug(nameof(AndroidActivity), nameof(RecListener_Ready));
			seconds = 1;
			mList.Add("RecListener_Ready");

			ImageView imgRecord;
			imgRecord = FindViewById<Android.Widget.ImageView>(Resource.Id.imgRecord);
			imgRecord.Visibility = Android.Views.ViewStates.Visible;

		}

		private void RecListener_BeginSpeech()
		{
			//Log.Debug(nameof(AndroidActivity), nameof(RecListener_BeginSpeech));

			seconds = 2;
			mList.Add("RecListener_BeginSpeech");
		}
		private void RecListener_EndSpeech()
		{

			//Log.Debug(nameof(AndroidActivity), nameof(RecListener_EndSpeech));
			mAudioManager.SetStreamVolume(Stream.Music, 0, 0);

			ImageView imgRecord;
			imgRecord = FindViewById<Android.Widget.ImageView>(Resource.Id.imgRecord);
			imgRecord.Visibility = Android.Views.ViewStates.Invisible;
			
			mList.Add("RecListener_EndSpeech");
		}

		private void RecListener_Recognized(object sender, string recognized)
		{
			string fmtInput = recognized.Trim().ToLower();
			ProcessText(fmtInput);
		}

		private void ProcessText(string recognized)
		{
			//Log.Debug(nameof(AndroidActivity), nameof(RecListener_Recognized));

			var txtYourScore = FindViewById<Android.Widget.EditText>(Resource.Id.YourScore);

			// this method called when Speech Recognition is ready
			// also this is the right time to un-mute system volume because the annoying sound played already
			mAudioManager = (AudioManager)GetSystemService(Context.AudioService);
			mAudioManager.SetStreamVolume(Stream.Music, mStreamVolume, 0); // again setting the system volume back to the original, un-muting

			int TotalScore = 0;
			int SingleScore = 0;
			string strRecommendedOut = "";

			string fmtInput = recognized.Trim().ToLower();

			try
			{
				
				if ("pause" == fmtInput.ToLower() || "stop" == fmtInput.ToLower())
				{
					seconds = 1;
					isListeningPaused = true;

					var txtOutLabel = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutLabel);
					var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
					var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);

					if (true == singleOut)
					{
						txtOutLabel.Text = "stopped";
						txtOutTurn.Text = "";
						txtNewOut.Text = "";
					}
					else
					{
						txtOutTurn.Text = "";
						txtNewOut.Text = "stopped";
						txtOutLabel.Text = "";
					}

					Toast.MakeText(this, "stopped", ToastLength.Long).Show();

					ResetGame();

					return;
				}

				Toast.MakeText(this, recognized, ToastLength.Long).Show();

				if (fmtInput.Contains("out") && recognized.ToLower().IndexOf("out") > 1)
				{
					txtYourScore.Text = recognized;

					if (!fmtInput.Contains(" "))
					{
						fmtInput = recognized.ToLower().Substring(0, (recognized.ToLower().IndexOf("out")));
					}
					else
					{
						fmtInput = recognized.ToLower().Substring(0, (recognized.ToLower().IndexOf("out") - 1));
					}

					bool Result = false;
					Result = int.TryParse(fmtInput, out TotalScore);

					//If not an Int set an out of range value for Total Score
					if (true != Result)
					{
						TotalScore = 200;
						strRecommendedOut = "Unknown Out";
					}
					else
					{
						strRecommendedOut = RecommendedOut(TotalScore, true);
					}

					var txtOutLabel = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutLabel);
					txtOutLabel.Text = strRecommendedOut;
					txtYourScore.Text = "";

					AsyncSpeak(strRecommendedOut);

				}
				//else if ((fmtInput.Contains("triple") && recognized.ToLower().IndexOf("triple") == 0))
				//{
				//	fmtInput = recognized.ToLower().Substring((recognized.ToLower().IndexOf("triple") + 6));

				//	int DoubleScore = clsTurn.CurrentScore;

				//	bool Result = false;
				//	Result = int.TryParse(fmtInput, out SingleScore);

				//	if (true != Result)
				//	{
				//		SingleScore = -1;
				//		strRecommendedOut = "Unknown Score";
				//	}
				//	else if (20 < SingleScore || 0 == SingleScore)
				//	{
				//		SingleScore = -1;
				//		strRecommendedOut = "Unknown Score";
				//	}
				//	else
				//	{
				//		SingleScore = SingleScore * 3;

				//		var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
				//		txtDartScore.Text = SingleScore.ToString();
				//		clsUIState.LastScore = SingleScore;

				//		strRecommendedOut = GetNewScore(SingleScore.ToString());

				//		var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
				//		txtNewOut.Text = strRecommendedOut;
				//		clsUIState.CurrentScoreText = txtNewOut.Text;

				//	}

				//	if (strRecommendedOut == "Win")
				//	{

				//		Intent iActivity = new Intent(this, typeof(BustActivity));

				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (strRecommendedOut == "Bust")
				//	{
				//		Intent iActivity = new Intent(this, typeof(BustActivity));

				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (strRecommendedOut == "TurnOver")
				//	{
				//		Intent iActivity = new Intent(this, typeof(TurnOverActivity));

				//		iActivity.PutExtra("OutScore", clsTurn.CurrentScore);
				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (clsTurn.State == TurnState.InProgress)
				//	{

				//		string strRecommendedOut2 = RecommendedOut(clsTurn.CurrentScore);
				//		var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
				//		txtOutTurn.Text = "(" + clsTurn.CurrentScore.ToString() + ") " + strRecommendedOut2;

				//		AsyncSpeak(strRecommendedOut, strRecommendedOut2);

				//	}

				//}
				//else if ((fmtInput.Contains("double") && recognized.ToLower().IndexOf("double") == 0))
				//{
				//	fmtInput = recognized.ToLower().Substring((recognized.ToLower().IndexOf("double") + 6));

				//	int DoubleScore = clsTurn.CurrentScore;

				//	bool Result = false;
				//	Result = int.TryParse(fmtInput, out SingleScore);

				//	if (true != Result)
				//	{
				//		SingleScore = -1;
				//		strRecommendedOut = "Unknown Score";
				//	}
				//	else if ((25 < SingleScore) || (20 < SingleScore && 25 > SingleScore) || 0 == SingleScore)
				//	{
				//		SingleScore = -1;
				//		strRecommendedOut = "Unknown Score";
				//	}
				//	else
				//	{
				//		SingleScore = SingleScore * 2;

				//		var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
				//		txtDartScore.Text = SingleScore.ToString();
				//		clsUIState.LastScore = SingleScore;

				//		strRecommendedOut = GetNewScore(SingleScore.ToString());

				//		var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
				//		txtNewOut.Text = strRecommendedOut;
				//		clsUIState.CurrentScoreText = txtNewOut.Text;

				//	}

				//	if (strRecommendedOut == "Win")
				//	{

				//		Intent iActivity = new Intent(this, typeof(WinActivity));

				//		iActivity.PutExtra("OutScore", SingleScore);
				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (strRecommendedOut == "Bust")
				//	{
				//		Intent iActivity = new Intent(this, typeof(BustActivity));

				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (strRecommendedOut == "TurnOver")
				//	{
				//		Intent iActivity = new Intent(this, typeof(TurnOverActivity));

				//		iActivity.PutExtra("OutScore", clsTurn.CurrentScore);
				//		iActivity.PutExtra("IsMuted", isMuted);

				//		StartActivity(iActivity);

				//	}
				//	else if (clsTurn.State == TurnState.InProgress)
				//	{

				//		string strRecommendedOut2 = RecommendedOut(clsTurn.CurrentScore);
				//		var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
				//		txtOutTurn.Text = "(" + clsTurn.CurrentScore.ToString() + ") " + strRecommendedOut2;

				//		AsyncSpeak(strRecommendedOut, strRecommendedOut2);

				//	}

				//}
				else if (((fmtInput.Contains("score") && recognized.ToLower().IndexOf("score") > 1) ||
						(fmtInput.Contains("hit") && recognized.ToLower().IndexOf("hit") > 1)) && clsTurn.State == TurnState.InProgress)
				{

					if (recognized.ToLower().IndexOf("score") > 1)
					{
						fmtInput = recognized.ToLower().Substring(0, (recognized.ToLower().IndexOf("score") - 1));
					}
					else
					{
						fmtInput = recognized.ToLower().Substring(0, (recognized.ToLower().IndexOf("hit") - 1));
					}

					bool Result = false;
					Result = int.TryParse(fmtInput, out SingleScore);

					// If not an Int set an out of range value for Total Score
					if (true != Result)
					{
						SingleScore = -1;
						strRecommendedOut = "Unknown Score";
					}
					else
					{
						var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
						txtDartScore.Text = SingleScore.ToString();
						clsUIState.LastScore = SingleScore;

						strRecommendedOut = GetNewScore(SingleScore.ToString());

						var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
						txtNewOut.Text = strRecommendedOut;
						clsUIState.CurrentScoreText = txtNewOut.Text;
					}

					if (strRecommendedOut == "Win")
					{
						Intent iActivity = new Intent(this, typeof(WinActivity));

						iActivity.PutExtra("OutScore", SingleScore);
						iActivity.PutExtra("IsMuted", isMuted);

						StartActivity(iActivity);

					}
					else if (strRecommendedOut == "Bust")
					{
						Intent iActivity = new Intent(this, typeof(BustActivity));

						iActivity.PutExtra("IsMuted", isMuted);

						StartActivity(iActivity);

					}
					else if (strRecommendedOut == "TurnOver")
					{
						Intent iActivity = new Intent(this, typeof(TurnOverActivity));

						iActivity.PutExtra("OutScore", clsTurn.CurrentScore);
						iActivity.PutExtra("IsMuted", isMuted);

						StartActivity(iActivity);

					}
					else if (clsTurn.State == TurnState.InProgress)
					{

						string strRecommendedOut2 = RecommendedOut(clsTurn.CurrentScore);
						var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
						txtOutTurn.Text = "(" + clsTurn.CurrentScore.ToString() + ") " + strRecommendedOut2;

						AsyncSpeak(strRecommendedOut, strRecommendedOut2);

					}

				}
				else if (fmtInput.Contains("start") && recognized.ToLower().IndexOf("start") > 1)
				{
					int startingScore = 0;

					fmtInput = recognized.ToLower().Substring(0, (recognized.ToLower().IndexOf("start") - 1));

					bool Result = false;
					Result = int.TryParse(fmtInput, out startingScore);

					if (true != Result)
					{
						startingScore = -1;
						strRecommendedOut = "Unknown Score";
					}
					else
					{
						clsTurn.DartsRemaining = 3;
						strRecommendedOut = RecommendedOut(startingScore);
					}

					bool mbOK = clsTurn.SetStartingScore(startingScore);

					if (mbOK)
					{
						var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
						txtDartScore.Text = "";
						clsUIState.LastScore = 0;

						var txtStartScore = FindViewById<Android.Widget.EditText>(Resource.Id.StartScore);
						txtStartScore.Text = startingScore.ToString();

						var txtNewOut = FindViewById<Android.Widget.TextView>(Resource.Id.txtNewScore);
						txtNewOut.Text = txtStartScore.Text + " Left, Darts Remaining: 3";
						currentScore = startingScore;
						clsUIState.CurrentScoreText = txtNewOut.Text;

						var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
						txtOutTurn.Text = "(" + txtStartScore.Text + ") " + strRecommendedOut;

						clsUIState.DoubleOutText = txtOutTurn.Text;

						AsyncSpeak(txtStartScore.Text + " Starting Score", strRecommendedOut);

					}

				}
				else
				{
					AsyncSpeak("Unknown Keyword","");
				}

				mList.Add("RecListener_Recognized");

				if (singleUse)
				{
					seconds = 1;
					isListeningPaused = true;
				}
				if (!isListeningPaused)
				{
					seconds = pauseTime;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print("Error in ProcessText(): " + ex.Message);
				throw ex;
			}
		}

		private void AsyncSpeak(string SpeakThis, string AndSpeakThis = "", bool TextOnly = false)
		{
			if (true == isMuted)
			{
				SpeakThis = "";
				AndSpeakThis = "";
			}

			// this method called when Speech Recognition is ready
			// also this is the right time to un-mute system volume because the annoying sound played already
			mAudioManager = (AudioManager)GetSystemService(Context.AudioService);

			if (0 < AndSpeakThis.Trim().Length)
			{
				if (false == TextOnly)
				{
					mAudioManager.SetStreamVolume(Stream.Music, mStreamVolume, 0); // again setting the system volume back to the original, un-muting

					// With multiple SpeakAsync calls
					Task.WhenAll(
						TextToSpeech.SpeakAsync(SpeakThis),
						TextToSpeech.SpeakAsync(AndSpeakThis)
						.ContinueWith(OnContinue, TaskScheduler.FromCurrentSynchronizationContext()));

				}
				else
				{
					TextToSpeech.SpeakAsync(SpeakThis);
					TextToSpeech.SpeakAsync(AndSpeakThis);
				}
				
			}
			else
			{
				if (false == TextOnly)
				{
					mAudioManager.SetStreamVolume(Stream.Music, mStreamVolume, 0); // again setting the system volume back to the original, un-muting

					TextToSpeech.SpeakAsync(SpeakThis).ContinueWith(OnContinue, TaskScheduler.FromCurrentSynchronizationContext());

				}
				else
				{
					TextToSpeech.SpeakAsync(SpeakThis);
				}
				
			}

		}
		private void ResetGame()
		{
			seconds = 1;
			isListeningPaused = true;

			Recognizer.StopListening();

			var txtDartScore = FindViewById<Android.Widget.EditText>(Resource.Id.DartScore);
			txtDartScore.Text = "";
			clsUIState.LastScore = 0;

			var txtStartScore = FindViewById<Android.Widget.EditText>(Resource.Id.StartScore);
			txtStartScore.Text = "";
			startingScore = 0;

			currentScore = startingScore;
			clsUIState.CurrentScoreText = "";

			var txtOutTurn = FindViewById<Android.Widget.TextView>(Resource.Id.txtOutTurn);
			txtOutTurn.Text = "";
			clsUIState.DoubleOutText = txtOutTurn.Text;

			int? totalScore = 0;
			totalScore = clsTurn.FirstDartPoints;
			
			if (clsTurn.SecondDartPoints != null)
			{
				totalScore += clsTurn.SecondDartPoints;
			}
			if (clsTurn.ThirdDartPoints != null)
			{
				totalScore += clsTurn.ThirdDartPoints;
			}

			PreferenceStore clsPrefStore = new PreferenceStore();
			bool scoreStored = clsPrefStore.CheckScoresTopList(totalScore.ToString());

			var BtnStartTurn = FindViewById<Android.Widget.Button>(Resource.Id.btn_StartTurn);
			BtnStartTurn.Enabled = true;
			var BtnGetOut = FindViewById<Android.Widget.Button>(Resource.Id.btn_GetOut);
			BtnGetOut.Enabled = true;
		}

		private string GetNewScore(string ThrownScore)
		{
			int dartScore = 0;
			string strReturnValue = "";

			bool Result = false;
			Result = int.TryParse(ThrownScore, out dartScore);

			// If not an Int set an out of range value for Total Score
			if (true != Result)
			{
				dartScore = -1;
				strReturnValue = "Unknown Score";
			}
			else
			{
				bool mbOK = clsTurn.RecordPointsScored(dartScore);

				if (mbOK)
				{
					int intNewScore = clsTurn.CurrentScore;
					
					if (TurnState.Done == clsTurn.State)
					{
						strReturnValue = strReturnValue = "TurnOver";
						ResetGame();
					}
					else if (TurnState.Bust == clsTurn.State)
					{
						strReturnValue = "Bust";
						ResetGame();
					}
					else if (TurnState.Win == clsTurn.State)
					{
						//strReturnValue = "Winner Winner Chicken Dinner!";
						strReturnValue = "Win";
						ResetGame();
					}
					else
					{
						strReturnValue = intNewScore.ToString() + " Left, Darts Remaining: " + clsTurn.DartsRemaining.ToString();
					}

				}
				else
				{
					if (TurnState.Done == clsTurn.State)
					{
						strReturnValue = "Turn Over";
						ResetGame();
					}
					else
					{
						strReturnValue = "Unknown Score";
					}
					
				}
			}


			return strReturnValue;
		}

		//This gets the out from the classes for vocalizing the words 
		private string RecommendedOut(int TotalOut, bool SingleOut = false)
		{

			int dartCount = 0;

			OutCalculator clsOutCalc = new OutCalculator(InOutRule.Double);
			List<Dart> recOut;

			if (false == SingleOut)
			{

				recOut = clsOutCalc.GetDartsForOut(TotalOut, clsTurn.DartsRemaining);

			}
			else
			{
				recOut = clsOutCalc.GetDartsForOut(TotalOut);

			}

			StringBuilder sb = new StringBuilder();

			foreach (Dart mDart in recOut)
			{
				sb.Append(recOut[dartCount].ToString() + ", ");
				dartCount++;
			}

			string strOutText = "";

			if (0 == sb.Length)
			{
				strOutText = "Out Not Found";
			}
			else
			{
				sb = sb.Remove(sb.Length - 2, 2);
				strOutText = sb.ToString();
			}

			return strOutText;

		}

		private string GetAbbrevOut(int TotalOut)
		{

			int score = TotalOut;
			OutCalculator outCalculator = new OutCalculator(InOutRule.Double);
			List<Dart> outs = outCalculator.GetDartsForOut(score);

			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < outs.Count; i++)
			{
				stringBuilder.Append(outs[i].Abbreviation);
				if (i != outs.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
		
			string text = stringBuilder.ToString();

			return text;
		}
		private bool UpdateDateTime()
		{
			//Log.Debug(nameof(AndroidActivity), "UpdateDateTime(): secs=" + seconds.ToString());

			if (seconds < pauseTime || seconds == 0)
			{
				Recognizer.StartListening(SpeechIntent);
				return true;
			}

			if (!isListeningPaused)
			{
				seconds += 1;
			}

			return true;
		}

		private void HideKeyboard()
		{
			try
			{
				Android.Views.InputMethods.InputMethodManager inputMethodManager = Application.GetSystemService(Context.InputMethodService) as Android.Views.InputMethods.InputMethodManager;
				inputMethodManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, Android.Views.InputMethods.HideSoftInputFlags.None);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print("Error- " + ex.Message);

			}
		}

		}
}