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
using Android.Graphics;

namespace StageEteMob
{
    public class _NewDevisName : AndroidX.Fragment.App.Fragment
    {
        Boolean nextState = false;
        TextView nextTV;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._newDevis_name, container, false);

            EditText devName = fragmentView.FindViewById<EditText>(Resource.Id.editText1);
            nextTV = fragmentView.FindViewById<TextView>(Resource.Id.nexttv);

            devName.TextChanged += DevName_TextChanged;
            nextTV.Click += NextTV_Click;

            return fragmentView;
        }

        private void NextTV_Click(object sender, EventArgs e)
        {
            if (nextState)
                Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisClient()).Commit();

        }

        private void DevName_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (e.Text.Count() > 0)
            {
                if (!nextState)
                    toggleNext();
            }
            else
            {
                toggleNext();
            }
        }
        public void toggleNext()
        {
            if (nextState)
            {
                nextState = false;
                nextTV.SetTextColor(Color.Rgb(152, 152, 152));
            }
            else
            {
                nextState = true;
                nextTV.SetTextColor(Color.Rgb(22, 22, 22));
            }
        }
    }

}
