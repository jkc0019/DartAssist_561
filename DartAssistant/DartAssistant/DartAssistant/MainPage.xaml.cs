using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace DartAssistant
{
	public partial class MainPage : ContentPage

	{
		//This is where the UI and user interaciton is controlled

		public MainPage()
		{
			InitializeComponent();

			//<><><><><><><><><><><><><><><><><><><><><><><><><>
			//Use this to return content of a text file as a List from
			//Files included as Assets in the OS-specific native Project. Pass Filename only with extension (i.e. DartRules.txt)
			//List<string> infoRequested = DependencyService.Get<IVersionHelper>().GetStoredInfo(infoFile:"DartRules.txt");

			//Example of file contents
			//foreach (string strOut in infoRequested)
			//{

			//	Debug.Print(strOut);

			//}
			//<><><><><><><><><><><><><><><><><><><><><><><><><>

			//TODO: work on getting the looping working. 
			//Challenge is getting it to speech when there's a 1.5 sec pause but continue listening
			//when waiting for the next speech to start.
			//May need to set a flag between speech to text and listening

			//If we still want to listen for voice recognition
			if (0 != App.numberOfTimes)
			{
				LetsBegin();
			}
		}

		private void OnStartGameButtonClicked(object sender, EventArgs args)
		{
			// Set # of times to listen (1 or -1 (for looping))
			App.numberOfTimes = 1;
			LetsBegin();
		}
			private void LetsBegin()
		{
			//<><><><><><><><><><><><><><><><><><><><><><><><><>
			//Use this to start listening on Native device using the Interface
			//Parameter determines if it should stop listening after first text is spoken
			if (App.numberOfTimes != 0)
			{
				string strText = DependencyService.Get<IActivityHelper>().Listen();

			}
			
			//<><><><><><><><><><><><><><><><><><><><><><><><><>

		}

		public string RecommendedOut(int TotalOut)

			//This gets the out from the classes and vocalizes the words 
		{
			string strOut = "";
			int dartCount = 0;

			OutCalculator clsOutCalc = new OutCalculator(InOutRule.Double);
			List<Dart> recOut = clsOutCalc.GetDartsForOut(TotalOut);

			StringBuilder sb = new StringBuilder();

			foreach (Dart mDart in recOut)
			{
				sb.Append(recOut[dartCount].ToString());
				dartCount++;
			}
			
			string strOutText = sb.ToString();

			//If it's not time to stop
			if (App.numberOfTimes != 0)
			{
				TextToSpeech.SpeakAsync(strOutText);
				App.numberOfTimes--;
			}
			else
			{
				Debug.Print("Heya");
			}

			return strOut;

		}

		public async Task ReturnText(string whatText)
		{
			//This is used to control stopping to recognition
			//TODO: work thru stopping the looping
			if ("pause" == whatText.ToLower() || "stop" == whatText.ToLower())
			{
				App.numberOfTimes = 0;
			}
		}

	}

}
