using System;
//Interface used to handle voice/speech recognition by AudioService Class
namespace DartAssistant
{
	public interface IAudio
	{
		string Listen(bool Ignore);

		bool Pause(bool Pause);

		//Not needed currently - handled by Xamarin.Essentials
		//bool SpeakText(string TextToSpeak);
	}

}