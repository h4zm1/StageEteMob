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
using Android.Graphics;

namespace StageEteMob
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnTouchListener
    {
        public ImageView addImg, profileImg, cdImg;
        public TextView profileTv, cdTv;
        Rect rect;
        bool outside = false;
        bool profileSelected = false;
        bool cdSelected = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //set signinfrag as starting frag
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.containerView, new SignInFrag()).Commit();
            addImg = FindViewById<ImageView>(Resource.Id.imageView161);
            profileImg = FindViewById<ImageView>(Resource.Id.imageView171);
            cdImg = FindViewById<ImageView>(Resource.Id.imageView151);

            profileTv = FindViewById<TextView>(Resource.Id.textView172);
            cdTv = FindViewById<TextView>(Resource.Id.textView152);

            addImg.SetOnTouchListener(this);
            profileImg.SetOnTouchListener(this);
            cdImg.SetOnTouchListener(this);
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
                    //search
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView151"))
                    {
                        outside = false;
                        rect = new Rect(addImg.Left + (addImg.Width / 3), addImg.Top, addImg.Right - (addImg.Width / 3), addImg.Bottom);

                        if (!cdSelected)
                        {
                            cdImg.SetImageResource(Resource.Drawable.nslight);
                            cdTv.SetTextColor(Color.Rgb(98, 98, 98));
                        }
                        else
                        {
                            cdImg.SetImageResource(Resource.Drawable.yslight);
                            cdTv.SetTextColor(Color.Rgb(98, 98, 98));
                        }

                        break;
                    }
                    //add
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView161"))
                    {
                        outside = false;
                        addImg.SetImageResource(Resource.Drawable.midbl);
                        //create bounds as rectange
                        rect = new Rect(addImg.Left + (addImg.Width / 3), addImg.Top, addImg.Right - (addImg.Width / 3), addImg.Bottom);
                        Console.WriteLine(addImg.Width + " width " + rect.Width());
                        break;

                    }
                    //me
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView171"))
                    {
                        outside = false;
                        rect = new Rect(addImg.Left + (addImg.Width / 3), addImg.Top, addImg.Right - (addImg.Width / 3), addImg.Bottom);

                        if (!profileSelected)
                        {
                            profileImg.SetImageResource(Resource.Drawable.notselecteduserlight);
                            profileTv.SetTextColor(Color.Rgb(98, 98, 98));
                        }
                        else
                        {
                            profileImg.SetImageResource(Resource.Drawable.selecteduserlight);
                            profileTv.SetTextColor(Color.Rgb(98, 98, 98));
                        }

                        break;

                    }
                    break;


                case MotionEventActions.Up:
                    //add
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView161"))
                    {
                        //lift finger(or wtv) while outside the button region => bail
                        if (outside)
                            break;

                        ///now the menu is up
                        //go back to default image
                        addImg.SetImageResource(Resource.Drawable.midb);
                        //rotate the image
                        rotate_Clockwise(addImg);

                        AndroidX.Fragment.App.FragmentTransaction ftrans = SupportFragmentManager.BeginTransaction();
                        AddPopFrag popFrag = new AddPopFrag();
                        popFrag.Show(ftrans, "");
                        break;

                    }
                    //me
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView171"))
                    {
                        //lift finger(or wtv) while outside the button region => bail
                        if (outside)
                            break;

                        profileImg.SetImageResource(Resource.Drawable.selecteduser);
                        profileTv.SetTextColor(Color.Rgb(22, 22, 22));

                        //no need to keep commiting (not sure about performance)
                        if (profileSelected)
                            break;
                        SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new MeFrag()).Commit();
                        profileSelected = true;
                        //unselect the other faketab
                        cdImg.SetImageResource(Resource.Drawable.ns);
                        cdSelected = false;
                        break;

                    }
                    //search
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView151"))
                    {
                        //lift finger(or wtv) while outside the button region => bail
                        if (outside)
                            break;

                        cdImg.SetImageResource(Resource.Drawable.ys);
                        cdTv.SetTextColor(Color.Rgb(22, 22, 22));

                        //no need to keep commiting (not sure about performance)
                        if (cdSelected)
                            break;
                        SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new SearchFrag()).Commit();
                        cdSelected = true;
                        //unselect the other faketab
                        profileImg.SetImageResource(Resource.Drawable.notselecteduser);
                        profileSelected = false;
                        break;

                    }
                    break;
                case MotionEventActions.Move:
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView161"))
                    {
                        //check if touch outside the rectangle
                        if (!rect.Contains(addImg.Left + (int)e.GetX(), addImg.Top + (int)e.GetY()))
                        {
                            addImg.SetImageResource(Resource.Drawable.midb);
                            //left the button region while touching
                            outside = true;
                            break;
                        }
                    }
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView171"))
                    {
                        //check if touch outside the rectangle
                        if (!rect.Contains(addImg.Left + (int)e.GetX(), addImg.Top + (int)e.GetY()))
                        {
                            if (!profileSelected)
                            {
                                profileImg.SetImageResource(Resource.Drawable.notselecteduser);
                                profileTv.SetTextColor(Color.Rgb(22, 22, 22));
                            }
                            else
                            {
                                profileImg.SetImageResource(Resource.Drawable.selecteduser);
                                profileTv.SetTextColor(Color.Rgb(22, 22, 22));
                            }
                            //left the button region while touching
                            outside = true;
                            break;
                        }
                    }
                    if (v.Resources.GetResourceName(v.Id).Contains("imageView151"))
                    {
                        //check if touch outside the rectangle
                        if (!rect.Contains(addImg.Left + (int)e.GetX(), addImg.Top + (int)e.GetY()))
                        {
                            if (!cdSelected)
                            {
                                cdImg.SetImageResource(Resource.Drawable.ns);
                                cdTv.SetTextColor(Color.Rgb(22, 22, 22));
                            }
                            else
                            {
                                cdImg.SetImageResource(Resource.Drawable.ys);
                                cdTv.SetTextColor(Color.Rgb(22, 22, 22));
                            }
                            //left the button region while touching
                            outside = true;
                            break;
                        }
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