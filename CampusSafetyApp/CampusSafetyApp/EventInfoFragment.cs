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
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Showing the text of the FAQ

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.EventFAQ, container, false);
            TextView content = (TextView)view.FindViewById(Resource.String.armed_text);
            content.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            return view;
        }
    }
}