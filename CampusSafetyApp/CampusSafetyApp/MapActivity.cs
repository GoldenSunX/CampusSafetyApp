using System;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Widget;
using Android.Locations;
using Newtonsoft.Json;

namespace CampusSafetyApp
{
    [Activity(Name = "com.campussafety.Activities.MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
    {
        private static readonly LatLng LatLngPoint = new LatLng(40.002286, -83.015986);
        public static LatLng eventLatLng;
        private GoogleMap _map;
        private MapFragment _mapFragment;

        public void OnMapReady(GoogleMap googleMap)
        {
            _map = googleMap;
            if (Intent.GetStringExtra("loc1") != null)
            {
                Console.WriteLine("User created event.");
                var obj = JsonConvert.DeserializeObject<LatLng>(Intent.GetStringExtra("loc1"));
                eventLatLng = new LatLng(40.002386, -83.017086);
            }
            SetupMapIfNeeded();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MapLayout);
            
            InitMapFragment();

            SetupAnimateToButton();
            SetupZoomInButton();
            SetupZoomOutButton();
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetupMapIfNeeded();
        }

        private void InitMapFragment()
        {
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
                _mapFragment.GetMapAsync(this);
            }

        }

        private void SetupAnimateToButton()
        {
            Button animateButton = FindViewById<Button>(Resource.Id.animateButton);
            animateButton.Click += (sender, e) =>
            {
                // Move camera
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(LatLngPoint);
                builder.Zoom(18);
                CameraPosition cameraPosition = builder.Build();

                // AnimateCamera provides a smooth, animation effect while moving
                // the camera to the the position.

                _map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
            };

        }

        private void SetupMapIfNeeded()
        {
            if (_map != null)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(LatLngPoint);
                markerOpt1.SetTitle("You are here!");
                markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRed));
                _map.AddMarker(markerOpt1);

                if (eventLatLng != null)
                {
                    Console.WriteLine("Event detected!");
                    MarkerOptions markerOpt2 = new MarkerOptions();
                    markerOpt2.SetPosition(eventLatLng);
                    markerOpt2.SetTitle("DANGER: Reported event occuring here!");
                    markerOpt2.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                    _map.AddMarker(markerOpt2);
                }
                
                // We create an instance of CameraUpdate, and move the map to it.
                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(LatLngPoint, 15);
                _map.MoveCamera(cameraUpdate);
                
            }
        }

        private void SetupZoomInButton()
        {
            Button zoomInButton = FindViewById<Button>(Resource.Id.zoomInButton);
            zoomInButton.Click += (sender, e) => { _map.AnimateCamera(CameraUpdateFactory.ZoomIn()); };
        }

        private void SetupZoomOutButton()
        {
            Button zoomOutButton = FindViewById<Button>(Resource.Id.zoomOutButton);
            zoomOutButton.Click += (sender, e) => { _map.AnimateCamera(CameraUpdateFactory.ZoomOut()); };
        }
    }
}
