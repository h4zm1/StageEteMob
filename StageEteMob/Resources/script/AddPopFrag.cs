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
using System.Timers;
using Google.Android.Material.Snackbar;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Animation;

namespace StageEteMob
{
    public class AddPopFrag : AndroidX.Fragment.App.DialogFragment
    {
        ImageView addImg;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout.addPopup, container, false);

            TextView addC = fragmentView.FindViewById<TextView>(Resource.Id.ClientTV);
            TextView addD = fragmentView.FindViewById<TextView>(Resource.Id.devisTV);
            addImg = Activity.FindViewById<ImageView>(Resource.Id.imageView161);
            addC.Click += AddC_Click;
            addD.Click += AddD_Click;
            roundCornerSetup();
            return fragmentView;
        }

        private void AddD_Click(object sender, EventArgs e)
        {
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisName()).Commit();

            rotate_AntiClockwise(addImg);
            this.Dismiss();
        }

        private void AddC_Click(object sender, EventArgs e)
        {
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new SetFrag()).Commit();
            rotate_AntiClockwise(addImg);
            this.Dismiss();

        }

        void roundCornerSetup()
        {
            Window window = Dialog.Window;

            // anchor it to the bottom
            window.SetGravity(GravityFlags.Bottom);

            WindowManagerLayoutParams windowParams = window.Attributes;
            windowParams.Y = 140;
            window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            window.Attributes = windowParams;
        }
        private void initEvent(object sender, EventArgs eventArgs)
        {
        }
        public override void OnDismiss(IDialogInterface dialog)
        {
            rotate_AntiClockwise(addImg);
            base.OnDismiss(dialog);
        }
        public void rotate_AntiClockwise(View view)
        {
            ObjectAnimator rotate = ObjectAnimator.OfFloat(view, "rotation", 45f, 00f);
            rotate.SetDuration(300);
            rotate.Start();
        }
    }
}