using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;
using System.Collections.Generic;
using System.Text;
namespace DartAssistant.Droid.Source
{
    public class fragRules3 : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static fragRules3 NewInstance()
        {
            var frag3 = new fragRules3 { Arguments = new Bundle() };
            return frag3;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			//var ignored = base.OnCreateView(inflater, container, savedInstanceState);
			//return inflater.Inflate(Resource.Layout.fragRules3, null);
			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			View v = inflater.Inflate(Resource.Layout.fragRules1, container, false);

			Android.Webkit.WebView webView = v.FindViewById<WebView>(Resource.Id.wvw_Rules);

			//webView.LoadUrl("https://google.com");

			//webView.Settings.JavaScriptEnabled = true;
			//webView.SetWebViewClient(new WebViewClient());

			//<><><><><><><><><><><><><><><><><><><><><><><><><>
			//Use this to return content of a text file as a List from
			//Files included as Assets in the OS-specific native Project. Pass Filename only with extension (i.e. DartRules.txt)
			Activities.RulesActivity activity = new Activities.RulesActivity();
			List<string> infoRequested = activity.GetStoredInfo(infoFile: "DartRules.txt");

			StringBuilder sb = new StringBuilder();

			foreach (string strOut in infoRequested)
			{
				System.Diagnostics.Debug.Print(strOut);

				if (0 == strOut.Trim().Length)
				{
					sb.Append("<br/>");
				}
				else
				{
					sb.Append(strOut);
				}

			}

			webView.LoadDataWithBaseURL(null, @"<html><body><h1>Strike Out</h1><p>" + sb.ToString() + "</p></body></html>", "text/html", "utf-8", null);


			return v;
		}
    }
}