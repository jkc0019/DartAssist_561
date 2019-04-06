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

namespace DartAssistant.Droid
{
    class OutChartAdapter : BaseAdapter, ISectionIndexer
    {
        List<string> items;
        Activity context;

        public OutChartAdapter(Activity context, List<string> items): base()
        {
            this.context = context;
            this.items = items;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return items[position];
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position].ToString();
            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public Java.Lang.Object[] GetSections()
        {
            // full list of section index titles
            // put in all numbers between highest and lowest
            int highestNumber = this.GetScoreFromItem(items.ElementAt(0));
            int highestSection = highestNumber / 10;

            // need a section for all numbers starting from zero
            Java.Lang.Object[] sections = new Java.Lang.Object[highestSection + 1];
            for (int i = 0; i < sections.Length; i++)
            {
                sections[i] = (sections.Length - i - 1) * 10;
            }
            return sections;
        }

        public int GetPositionForSection(int sectionIndex)
        {
            Java.Lang.Object sectionItem = this.GetSections().ElementAt(sectionIndex);
            // we need to get the value of the section to compare the items against that
            int sectionValue = (int)sectionItem;

            int position = 0;
            // return first row that meets that criteria.
            for (int i = 0; i < items.Count; i++)
            {
                // show the first item that is in the section.
                if ((this.GetScoreFromItem(items[i]) / 10) * 10 == sectionValue)
                {
                    position = i;
                    break;
                }                
            }

            return position;
        }

        public int GetSectionForPosition(int position)
        {
            int score = this.GetScoreFromItem(items[position]);
            int numberOfSections = this.GetSections().Count();
            // get the inverse of the score / 10... let's say we're at row with value of 45.
            // 45 / 10 = 4, which is the section VALUE... but we need the section INDEX.
            // since the sections are in descending order, the index is mirrored 
            // (if value is 4 and total num of sections is 17, index is 13)
            int sectionIndex = numberOfSections - (score / 10);
            return sectionIndex;
        }

        private int GetScoreFromItem(string item)
        {
            // the score is the part of the string before the colon
            string[] pieces = item.Split(':');
            return int.Parse(pieces[0]);
        }

    }

    class Adapter1ViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}