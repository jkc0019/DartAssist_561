using System.Collections.Generic;

namespace DartAssistant
{
    public interface IActivityHelper
    {
        string GetVersionNumber();

		string Listen();

		List<string> GetStoredInfo(string infoFile);

	}
}
