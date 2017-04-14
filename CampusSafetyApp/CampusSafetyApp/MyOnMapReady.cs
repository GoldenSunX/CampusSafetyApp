using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CampusSafetyApp
{
    class MyOnMapReady : Java.Lang.Object, IOnMapReadyCallback
    {
        public GoogleMap Map { get; private set; }
        public event EventHandler MapReady;

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;
            MapReady?.Invoke(this, EventArgs.Empty);
        }
    }
}
