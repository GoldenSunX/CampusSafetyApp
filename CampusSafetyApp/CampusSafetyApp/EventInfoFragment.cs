using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;


namespace CampusSafetyApp
{
    public class EventInfoFragment : Fragment
    {
        TextView armed_text;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            //View root = this.LayoutInflator.Inflate(Resource.Layout.EventFAQ, FragmentContainer, false);
            //SetContentView(Resource.Layout.EventFAQ);

            //armed_text = (TextView) root.FindViewById(Resource.Id.armed_text);
            //armed_text.Set
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.EventFAQ, container, false);
        }
        
        //public void toggle_contents(View v)
        //{
            //armed_text.setVisibility(armed_text.isShown() ? View.GONE: View.VISIBLE);
        //}
    }
}