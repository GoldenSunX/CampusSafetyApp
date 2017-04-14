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
using Android.Webkit;

using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Locations;

namespace CampusSafetyApp
{
    public class EventAlertFragment : Fragment
    {
        public WebView browser;
        public bool localEventOccured = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.EventAlert, container, false);
        }

        public override void OnStart()
        {
            //Note that scrolling is only possible by putting your finger in the top half of the
            //  screen, and doing all of your scrolling there
            browser = Activity.FindViewById<WebView>(Resource.Id.webView1);
            browser.SetWebViewClient(new WebViewClient());
            browser.LoadUrl("https://twitter.com/osu_emfp?lang=en");

            TextView inactive = Activity.FindViewById<TextView>(Resource.Id.inactive);
            TextView active = Activity.FindViewById<TextView>(Resource.Id.active);
            inactive.TextSize = 20f;
            active.TextSize = 20f;
            if (localEventOccured)
            {
                ((ViewGroup)inactive.Parent).RemoveView(inactive);
            } else
            {
                ((ViewGroup)active.Parent).RemoveView(active);
            }
            base.OnStart();
        }

        public override void OnResume()
        {
            base.OnResume();
        }
    }
}