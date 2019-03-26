using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Webkit;
using System.Collections.Generic;
using System.Text;

namespace DartAssistant.Droid.Source
{
    public class fragRules1 : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static fragRules1 NewInstance()
        {
            var frag1 = new fragRules1 { Arguments = new Bundle() };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

			var ignored = base.OnCreateView(inflater, container, savedInstanceState);

			View v = inflater.Inflate(Resource.Layout.fragRules1, container, false);

			Android.Webkit.WebView webView = v.FindViewById<WebView>(Resource.Id.wvw_Rules);

			//webView.LoadUrl("https://google.com");

			//webView.Settings.JavaScriptEnabled = true;
			//webView.SetWebViewClient(new WebViewClient());

			//<><><><><><><><><><><><><><><><><><><><><><><><><>
			//Use this to return content of a text file as a List from
			//Files included as Assets in the OS-specific native Project. Pass Filename only with extension (i.e. DartRules.txt)
			Activities.Activity3 activity = new Activities.Activity3();
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

			webView.LoadDataWithBaseURL(null, @"<html><body><h1>301 / 501</h1><p>" + sb.ToString() + "</p></body></html>", "text/html", "utf-8", null);

			
			return v;

		}

	}
}