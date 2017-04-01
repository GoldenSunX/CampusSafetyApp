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
using Android.Webkit;
using Xamarin.Forms.WebView;
using Xamarin.Android.Net;

namespace CampusSafetyApp
{
    public static class PublicSafetyNoticesWebpageDisplay
    {
        var browser = new WebView
        {
            Source = "http://xamarin.com"
        };
    }
}