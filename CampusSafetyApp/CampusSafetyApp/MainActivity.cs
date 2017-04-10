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

namespace CampusSafetyApp
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        DrawerLayout drawerLayout;
        ActionBarDrawerToggle drawerToggle;
        NavigationView navigatorView;

        static List<string> eventNumbers = new List<string>();

        //Creates UI
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create UI
            SetContentView(Resource.Layout.MainPageNavigation);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Widget.Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            // Attach item selected handler to navigation view
            navigatorView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigatorView.NavigationItemSelected += NavigationView_NavigationItemSelected;
            navigatorView.SetCheckedItem(Resource.Id.nav_home);

            // Create ActionBarDrawerToggle button and add it to the toolbar
            drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.AddDrawerListener(drawerToggle);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            drawerToggle.SyncState();

            //Setup floating action button
            FloatingActionButton actionButton = FindViewById<FloatingActionButton>(Resource.Id.create_event);
            actionButton.Click += createEvent;

            //Add home fragment to view
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            HomeFragment home = new HomeFragment();
            transaction.Add(Resource.Id.fragment_container, home).Commit();

            Button eventHistoryButton = FindViewById<Button>(Resource.Id.nav_history);
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
                    MapFragment map = new MapFragment();
                    transaction.Replace(Resource.Id.fragment_container, map).Commit();
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

        //Callback for floating action button.
        void createEvent(object sender, EventArgs e)
        {
            Console.WriteLine("[Navigation]: Moving to Event Creator.");
            FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
            CreateEventFragment eventCreator = new CreateEventFragment();
            transaction.Replace(Resource.Id.fragment_container, eventCreator).Commit();

            //Uncheck any navigation items
            navigatorView.SetCheckedItem(Resource.Id.nav_none);
        }

        //Enable ActionBar button for opening navigation.
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            drawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }


    }
}

