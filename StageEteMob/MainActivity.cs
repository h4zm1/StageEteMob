using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Google.Android.Material.Button;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Tabs;
using Android.Widget;


namespace StageEteMob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    //BottomNavigationView.IOnNavigationItemSelectedListener
    {
        //ViewStub stub;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //set setfrag as main frag
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.containerView, new SetFrag()).Commit();

            TabLayout tab = FindViewById<TabLayout>(Resource.Id.tabLayout1);
            tab.TabSelected += TabLayoutOnTabSelected;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        private void TabLayoutOnTabSelected(object sender, TabLayout.TabSelectedEventArgs tabSelectedEventArgs)
        {
            string selected = tabSelectedEventArgs.Tab.Text;
            if (selected == "set")
            {
                Console.WriteLine("*******************   set ");
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new SetFrag()).Commit();
            }
            else
            {
                Console.WriteLine("*******************   get ");
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new GetFrag()).Commit();
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}