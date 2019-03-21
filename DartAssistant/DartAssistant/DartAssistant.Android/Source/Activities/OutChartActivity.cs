using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DartAssistant.Droid.Source.Activities
{
    [Activity(Label = "Out Chart")]
    public class OutChartActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OutChart);
            Button button = FindViewById<Button>(Resource.Id.btn_Back);
            button.Click += delegate {
                StartActivity(typeof(AndroidActivity));
            };

            // Create your application here
            InitializeOutChart();
        }

        private void InitializeOutChart()
        {
            OutCalculator outCalculator = new OutCalculator(InOutRule.Double);
            List<String> allOutsList = new List<String>();
            Dictionary<int, List<Dart>> allOuts = outCalculator.GetAllOuts();
            // iterate through the dictionary to get all the outs
            foreach (KeyValuePair<int, List<Dart>> entry in allOuts)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(entry.Key.ToString());
                stringBuilder.Append(": ");

                for (int j = 0; j < entry.Value.Count; j++)
                {
                    stringBuilder.Append(entry.Value[j].Abbreviation);
                    if (j != entry.Value.Count - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }

                string outItemText = stringBuilder.ToString();
                allOutsList.Add(outItemText);
            }

            Android.Widget.ListView lstMyList = (Android.Widget.ListView)FindViewById<Android.Widget.ListView>(Resource.Id.MyList);
            lstMyList.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, allOutsList);

        }
    }
}