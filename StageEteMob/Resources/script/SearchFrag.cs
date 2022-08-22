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
    public class SearchFrag : AndroidX.Fragment.App.Fragment
    {
        TextView devisTV;
        TextView articleTv;
        TextView clientTv;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Console.WriteLine("DEVISSSSS");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_search, container, false);

            devisTV = fragmentView.FindViewById<TextView>(Resource.Id.devisText);
            articleTv = fragmentView.FindViewById<TextView>(Resource.Id.articleText);
            clientTv = fragmentView.FindViewById<TextView>(Resource.Id.clientText);
            devisTV.Click += devisEvent;
            articleTv.Click += articleEvent;
            clientTv.Click += clientEvent;
            //set _DevisFrag as starting frag
            Activity.SupportFragmentManager.BeginTransaction().Add(Resource.Id.devisContainerView, new _DevisFrag()).Commit();


            return fragmentView;
        }
        void devisEvent(object sender, EventArgs eventArgs)
        {

            devisTV.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.devisContainerView, new _DevisFrag()).Commit();

        }
        void articleEvent(object sender, EventArgs eventArgs)
        {
            devisTV.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.devisContainerView, new _ArticleFrag()).Commit();

        }
        void clientEvent(object sender, EventArgs eventArgs)
        {
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            devisTV.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.devisContainerView, new _ClientFrag()).Commit();

        }
    }
}