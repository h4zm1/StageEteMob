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
using Android.Views.Animations;
using Java.Lang;
using Android.Animation;
using Google.Android.Material.TextField;

namespace StageEteMob
{
    public class MeFrag : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_me, container, false);

            TextView letterTV = fragmentView.FindViewById<TextView>(Resource.Id.letterTV);
            TextView nameTV = fragmentView.FindViewById<TextView>(Resource.Id.nameTV);
            TextView idTV = fragmentView.FindViewById<TextView>(Resource.Id.idTV);
            TextView iduserTV = fragmentView.FindViewById<TextView>(Resource.Id.iduserTV);

            letterTV.Text = GlobVars.user.login[0].ToString().ToUpper();
            nameTV.Text = "Name: "+GlobVars.user.login;
            idTV.Text = "Id: "+GlobVars.user.id;
            iduserTV.Text = "UId: " + GlobVars.user.IdUtilisateur;
            //passET.Text = Glo

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