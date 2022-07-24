using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StageEteMob.Resources.script;
using Google.Android.Material.Tabs;

namespace StageEteMob
{
    public class MeFrag : AndroidX.Fragment.App.Fragment
    {
        struct incUser
        {
            public string login;
            public string password;
            public string code;
        };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_me, container, false);

            TextView jsonDisplay = fragmentView.FindViewById<TextView>(Resource.Id.tvJson);
            jsonDisplay.Text = GlobVars.userJson;

            MaterialButton btn = fragmentView.FindViewById<MaterialButton>(Resource.Id.signoutBtn);
            btn.Click += initEvent;

            return fragmentView;
        }
        private void initEvent(object sender, EventArgs eventArgs)
        {
          
            //go back to sign in fragment
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new SignInFrag()).Commit();
        }
    }
}