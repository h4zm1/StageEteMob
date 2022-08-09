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
using Android.Animation;

namespace StageEteMob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnTouchListener
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //set signinfrag as starting frag
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.containerView, new SignInFrag()).Commit();

            LinearLayout leftFakeTab = FindViewById<LinearLayout>(Resource.Id.linearLayout15);
            LinearLayout midFakeTab = FindViewById<LinearLayout>(Resource.Id.linearLayout16);
            LinearLayout rightFakeTab = FindViewById<LinearLayout>(Resource.Id.linearLayout17);
            leftFakeTab.SetOnTouchListener(this);
            midFakeTab.SetOnTouchListener(this);
            rightFakeTab.SetOnTouchListener(this);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    if (v.Resources.GetResourceName(v.Id).Contains("15"))
                    {
                        SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new GetFrag()).Commit();
                        break;

                    }
                    if (v.Resources.GetResourceName(v.Id).Contains("16"))
                    {
                        var addImg = FindViewById<ImageView>(Resource.Id.imageView161);
                        rotate_Clockwise(addImg);

                        AndroidX.Fragment.App.FragmentTransaction ftrans = SupportFragmentManager.BeginTransaction();
                        AddPopFrag popFrag = new AddPopFrag();
                        popFrag.Show(ftrans, "");
                        break;

                    }
                    if (v.Resources.GetResourceName(v.Id).Contains("17"))
                    {
                        SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new MeFrag()).Commit();
                        break;

                    }
                    break;


                case MotionEventActions.Up:
                    if (v.Resources.GetResourceName(v.Id).Contains("16"))
                    {

                        break;

                    }
                    break;
            }
            return true;
        }
        public void rotate_Clockwise(View view)
        {
            ObjectAnimator rotate = ObjectAnimator.OfFloat(view, "rotation", 0f, 45f);
            rotate.SetDuration(80);
            rotate.Start();
        }

    }
}