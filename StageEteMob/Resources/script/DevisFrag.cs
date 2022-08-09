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
using AndroidX.RecyclerView.Widget;

namespace StageEteMob
{
    public class DevisFrag : AndroidX.Fragment.App.Fragment
    {
        TextView devisTV;
        TextView articleTv;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_devis, container, false);

            devisTV = fragmentView.FindViewById<TextView>(Resource.Id.devisText);
            articleTv = fragmentView.FindViewById<TextView>(Resource.Id.articleText);
            devisTV.Click += devisEvent;
            articleTv.Click += articleEvent;

            //set _DevisFrag as starting frag
            Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.devisContainerView, new _DevisFrag()).Commit();


            //jsonDisplay.Click += initEvent;
            return fragmentView;
        }
        void devisEvent(object sender, EventArgs eventArgs)
        {

            devisTV.SetTextColor(Android.Graphics.Color.Rgb(22, 22, 22));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(173, 173, 173));
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.devisContainerView, new _DevisFrag()).Commit();

        }
        void articleEvent(object sender, EventArgs eventArgs)
        {
            devisTV.SetTextColor(Android.Graphics.Color.Rgb(173, 173, 173));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(22, 22, 22));
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.devisContainerView, new _ArticleFrag()).Commit();

        }
    }
}