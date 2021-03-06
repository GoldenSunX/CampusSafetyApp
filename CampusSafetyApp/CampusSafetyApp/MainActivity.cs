﻿#define DEBUG
using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.Collections.Generic;
using Android.Content;
using Android.Locations;
using Android.Runtime;
using System.Linq;
using Newtonsoft.Json;
using Android.Gms.Maps.Model;

namespace CampusSafetyApp
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity, ILocationListener, View.IOnClickListener
    {
        DrawerLayout drawerLayout;
        ActionBarDrawerToggle drawerToggle;
        NavigationView navigatorView;
        Clans.Fab.FloatingActionMenu create_menu;
        Clans.Fab.FloatingActionButton call_911;
        Clans.Fab.FloatingActionButton call_campus;
        Clans.Fab.FloatingActionButton register_event;
        private MapActivity _mapActivity;
        Intent mapIntent;

        static List<string> eventNumbers = new List<string>();

        string campus_number = "";

        //Creates UI
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            mapIntent = new Intent(this, typeof(MapActivity));

            // Create UI
            SetContentView(Resource.Layout.MainPageNavigation);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // Attach item selected handler to navigation view
            navigatorView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigatorView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            navigatorView.SetCheckedItem(Resource.Id.nav_home);

            // Create ActionBarDrawerToggle button and add it to the toolbar
            drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.AddDrawerListener(drawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            drawerToggle.SyncState();

            //Setup Fab menu
            create_menu = FindViewById<Clans.Fab.FloatingActionMenu>(Resource.Id.create_menu);
            create_menu.LongClickable = false;
            create_menu.LongClick += create911Event;
            create_menu.SetOnMenuButtonClickListener(this);
            

            //Setup call 911 floating action button
            call_911 = FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.call_911);
            call_911.Click += create911Event;
            call_911.Visibility = ViewStates.Gone;

            //Setup call campus floating action button
            call_campus = FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.call_campus);
            call_campus.Click += createCampusEvent;
            call_campus.Visibility = ViewStates.Gone;

            //Setup register a local event floating action button
            register_event = FindViewById<Clans.Fab.FloatingActionButton>(Resource.Id.register_event);
            register_event.Click += createLocalEvent;
            register_event.Visibility = ViewStates.Gone;

            //Add home fragment to view
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            HomeFragment home = new HomeFragment();
            transaction.Add(Resource.Id.fragment_container, home).Commit();

            Button eventHistoryButton = FindViewById<Button>(Resource.Id.nav_history);
        }
        protected override void OnResume()
        {
            
            base.OnResume();
            Console.WriteLine("[Location]: Checking for " + _locationProvider);

            try
            {
                InitializeLocationManager();
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            }
            catch
            {
                Console.WriteLine("[Location]: No Location");
                createLocationAlert();
            }

            //Make sure home gets reselected as the navigation item.
            navigatorView.SetCheckedItem(Resource.Id.nav_home);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        //Callback for changing fragments when navigation options are selected.
        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();

            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    Console.WriteLine("[Navigation]: Moving to Application Home.");
                    HomeFragment home = new HomeFragment();
                    transaction.Replace(Resource.Id.fragment_container, home).Commit();
                    break;
                case (Resource.Id.nav_map):
                    Console.WriteLine("[Navigation]: Moving to Event Map.");
                    _mapActivity = new MapActivity();
                    if (_currentLocation != null)
                    {
                        mapIntent.PutExtra("cur_loc_lat", _currentLocation.Latitude);
                        mapIntent.PutExtra("cur_loc_long", _currentLocation.Longitude);
                    }
                    
                    StartActivity(mapIntent);
                    break;
                case (Resource.Id.nav_alerts):
                    Console.WriteLine("[Navigation]: Moving to Event Alerts.");
                    EventAlertFragment alerts = new EventAlertFragment();
                    transaction.Replace(Resource.Id.fragment_container, alerts).Commit();
                    break;
                case (Resource.Id.nav_history):
                    Console.WriteLine("[Navigation]: Moving to Event History.");
                    EventHistoryFragment eventHistory = new EventHistoryFragment();
                    transaction.Replace(Resource.Id.fragment_container, eventHistory).Commit();
                    break;
                case (Resource.Id.nav_info):
                    Console.WriteLine("[Navigation]: Moving to Event Info.");
                    EventInfoFragment eventInfo = new EventInfoFragment();
                    transaction.Replace(Resource.Id.fragment_container, eventInfo).Commit();
                    break;
            }

            // Close drawer
            drawerLayout.CloseDrawers();
        }

        void create911Event(object sender, EventArgs e)
        {
            string str = "tel:911";
#if DEBUG
            Console.WriteLine("Calling 911");
            str = "tel:3335552222";
#endif
            var uri = Android.Net.Uri.Parse(str);
            var intent = new Intent(Intent.ActionCall, uri);
            StartActivity(intent);

            //Uncheck any navigation items
            navigatorView.SetCheckedItem(Resource.Id.nav_none);
            OnClick(create_menu);
        }

        void createCampusEvent(object sender, EventArgs e)
        {
            SetCampusNumber();
            if (campus_number != string.Empty)
            {
                var uri = Android.Net.Uri.Parse(campus_number);
                var intent = new Intent(Intent.ActionCall, uri);
                StartActivity(intent);

                //Uncheck any navigation items
                navigatorView.SetCheckedItem(Resource.Id.nav_none);
            }
            OnClick(create_menu);
        }

        void createLocalEvent(object sender, EventArgs e)
        {
            EventAlertFragment.localEventOccured = true;
            String title = "WARNING";
            String subject = "Campus Safety App Notification";
            String body = "An Emergency Has Occured In Your Area";
            NotificationManager notif = (NotificationManager)GetSystemService(Context.NotificationService);
            Notification.Builder notify = new Notification.Builder(this).SetSmallIcon(Resource.Drawable.icon).SetContentTitle(subject).SetContentTitle(title).SetContentText(body);
            notif.Notify(0, notify.Build());

            TextView inactive = FindViewById<TextView>(Resource.Id.inactive);
            TextView active = FindViewById<TextView>(Resource.Id.active);
            if (inactive != null && active != null)
            {
                inactive.Visibility = ViewStates.Invisible;
                active.Visibility = ViewStates.Visible;
            }

            //Report location to Map page
            if (_currentLocation != null)
            {
                mapIntent.PutExtra("ev_loc_lat", _currentLocation.Latitude);
                mapIntent.PutExtra("ev_loc_long", _currentLocation.Longitude);
            }
            else
            {
                //User has no location to report.
                mapIntent.PutExtra("ev_loc_lat", 40.002293);
                mapIntent.PutExtra("ev_loc_long", -83.016978);
            }

            OnClick(create_menu);
        }

        //Enable ActionBar button for opening navigation.
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        private void createLocationAlert()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Location not active");
            builder.SetMessage("Your GPS seems to be disabled, do you want to enable it?").SetCancelable(false);
            builder.SetPositiveButton("Yes", (senderAlert, AssemblyLoadEventArgs) =>
            {
                StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            });
            builder.SetNegativeButton("No", (senderAlert, AssemblyLoadEventArgs) =>
            {

                Toast t = Toast.MakeText(this, "Unable to retrieve location.", ToastLength.Short);
                t.SetGravity(GravityFlags.Bottom, 0, 16);
                t.Show();
            });

            Android.App.AlertDialog alert = builder.Create();
            alert.Show();
        }

        private void SetCampusNumber()
        {
            Address addr = ReverseGeocodeCurrentLocation();
            if (_currentLocation == null)
            {
                campus_number = "";
                Toast t = Toast.MakeText(this, "Unable to retrieve location.", ToastLength.Short);
                t.SetGravity(GravityFlags.Bottom, 0, 16);
                t.Show();
            }
            else if (_currentLocation.DistanceTo(campus_center) < campus_distance)
            {
                campus_number = "tel:6142922121";
#if DEBUG
                Console.WriteLine("Calling Campus Police");
                campus_number = "tel:3335552442";
#endif
            }
            else if (addr != null && addr.Locality == "Columbus" && addr.AdminArea == "OH" && addr.CountryCode == "US")
            {
                campus_number = "tel:6146454545";
#if DEBUG
                Console.WriteLine("Calling Columbus Police");
                campus_number = "tel:3335554224";
#endif
            }
            else if (_currentLocation.DistanceTo(city_center) < city_distance)
            {
                campus_number = "tel:6146454545";
#if DEBUG
                Console.WriteLine("Calling Columbus Police");
                campus_number = "tel:3335554224";
#endif
            }
            else
            {
                campus_number = "";
                Toast t = Toast.MakeText(this, "Outside Columbus", ToastLength.Short);
                t.SetGravity(GravityFlags.Bottom, 0, 16);
                t.Show();
            }
        }

        /// <summary>
        /// This is the ILocationListener Implementation and dependency functions/methods
        /// </summary>
        public Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;

        static Location campus_center;
        double campus_distance;
        static Location city_center;
        double city_distance;

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        void InitializeLocationManager()
        {
            campus_center = new Location("");
            campus_center.Latitude = 40;
            campus_center.Longitude = -83.0145;
            campus_distance = 1508;

            city_center = new Location("");
            city_center.Latitude = 39.961176;
            city_center.Longitude = -82.998794;
            city_distance = 16000;

            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            try
            {
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            }
            catch
            {
                _currentLocation = null;
            }
        }

        Address ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            Address addr;
            try
            {
                IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 1);
                addr = addressList.FirstOrDefault();
            }
            catch
            {
                addr = null;
            }
            return addr;
        }

        public void OnClick(View v)
        {
            Console.WriteLine("Test");
            
            if (create_menu.IsOpened)
            {
                Console.WriteLine("Closing");
                call_911.Visibility = ViewStates.Gone;
                call_campus.Visibility = ViewStates.Gone;
                register_event.Visibility = ViewStates.Gone;
                create_menu.Close(true);
            }
            else
            {
                Console.WriteLine("Opening");
                call_911.Visibility = ViewStates.Visible;
                call_campus.Visibility = ViewStates.Visible;
                register_event.Visibility = ViewStates.Visible;
                create_menu.Open(true);
            }
        }
    }
}
