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
using AndroidX.Fragment.App;
using Android.Graphics.Drawables;

namespace StageEteMob
{
    public class SearchFrag : AndroidX.Fragment.App.Fragment, Java.IO.ISerializable
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

            if (GlobVars.comingFromSetFrag)
                clientEvent(this, null);
            else
                //set _DevisFrag as starting frag
                startAndPass(new _DevisFrag(), this);
            GlobVars.comingFromSetFrag = false;
            //ImageView img = fragmentView.FindViewById<ImageView>(Resource.Id.bin);
            //AnimatedVectorDrawable drawable = (AnimatedVectorDrawable)Resource.Drawable.ripple2;
            //img.SetImageDrawable(drawable);
            //drawable.Start();
            return fragmentView;
        }
        /// <summary>
        /// setup for loading
        /// </summary>
        public void startAndPass(AndroidX.Fragment.App.Fragment frag, Java.IO.ISerializable msg)
        {
            if (msg == null)
            {
                var fragTx = Activity.SupportFragmentManager.BeginTransaction();
                fragTx.Show(frag);
                //fragTx.Replace(Resource.Id.devisContainerView, frag);
                fragTx.Commit();
            }
            else
            {
                Bundle args = new Bundle();
                args.PutSerializable("parent", msg);
                frag.Arguments = args;
                var fragTx = Activity.SupportFragmentManager.BeginTransaction();
                fragTx.Hide(frag);
                fragTx.Replace(Resource.Id.devisContainerView, frag);
                fragTx.Commit();
            }
        }
        void devisEvent(object sender, EventArgs eventArgs)
        {

            devisTV.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            startAndPass(new _DevisFrag(), this);
        }
        void articleEvent(object sender, EventArgs eventArgs)
        {
            devisTV.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            startAndPass(new _ArticleFrag(), this);
        }
         void clientEvent(object sender, EventArgs eventArgs)
        {
            clientTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
            devisTV.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            articleTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
            startAndPass(new _ClientFrag(), this);
        }
        public void displayTest()
        {
            Console.WriteLine("FROM EARTH TO MARS");
        }
    }
}