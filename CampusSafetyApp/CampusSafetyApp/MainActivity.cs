using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using System.Collections.Generic;

namespace CampusSafetyApp
{
    [Activity(Label = "Campus Safety", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;

        static List<string> eventNumbers = new List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create UI
            SetContentView(Resource.Layout.MainPageNavigation);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            // Init toolbar
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            // Attach item selected handler to navigation view
            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += NavigationView_NavigationItemSelected;

            // Create ActionBarDrawerToggle button and add it to the toolbar
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, Resource.String.open_drawer, Resource.String.close_drawer);
            drawerLayout.SetDrawerListener(drawerToggle);
            drawerToggle.SyncState();

            Button eventHistoryButton = FindViewById<Button>(Resource.Id.nav_history);
        }

        void NavigationView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.MenuItem.ItemId)
            {
                case (Resource.Id.nav_home):
                    // React on 'Home' selection
                    break;
                case (Resource.Id.nav_map):
                    // React on 'Messages' selection
                    break;
                case (Resource.Id.nav_alerts):
                    // React on 'Friends' selection
                    break;
                case (Resource.Id.nav_history):
                    var intent = new Intent(this, typeof(EventHistoryActivity));
                    intent.PutStringArrayListExtra("event_numbers", eventNumbers);
                    StartActivity(intent);
                    break;
                case (Resource.Id.nav_info):
                    // React on 'Discussion' selection
                    break;
            }

            // Close drawer
            drawerLayout.CloseDrawers();
        }
    }
}

