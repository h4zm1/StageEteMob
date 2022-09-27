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
using Com.Airbnb.Lottie;
using Android.Views.InputMethods;
using static Android.Icu.Text.CaseMap;

namespace StageEteMob
{
    /// <summary>
    /// TODO ADD LOADING INDICATOR
    /// </summary>
    public class _NewDevisArticle : AndroidX.Fragment.App.Fragment
    {
        TextView nextTV, titleTV;
        ImageButton gobackIB;
        LottieAnimationView img;
        public Boolean nextState = false;
        ImageButton searchIB;
        EditText searchET;
        Boolean isSearch = true;
        RecyclerView.LayoutManager mLayoutManager;
        List<Article> listOfArticle = new List<Article>();
        int maxNameLength = 0;
        Boolean firstRun = true;
        View fragmentView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            fragmentView = inflater.Inflate(Resource.Layout._newDevis_article, container, false);

            nextTV = fragmentView.FindViewById<TextView>(Resource.Id.nexttv);
            titleTV = fragmentView.FindViewById<TextView>(Resource.Id.titleTV);

            gobackIB = fragmentView.FindViewById<ImageButton>(Resource.Id.goback);
            img = fragmentView.FindViewById<LottieAnimationView>(Resource.Id.animation_view);

            searchIB = fragmentView.FindViewById<ImageButton>(Resource.Id.searchIB);
            searchET = fragmentView.FindViewById<EditText>(Resource.Id.searchET);

            searchET.TextChanged += SearchET_TextChanged;
            searchIB.Click += SearchIB_Click;

            nextTV.Click += NextTV_Click;
            gobackIB.Click += GobackIB_Click;
            midSync(fragmentView);

            return fragmentView;
        }
        private void SearchET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            searchMeth(e.Text.ToString());
        }

        void searchMeth(string incStr)
        {
            Console.WriteLine(incStr);
            List<Article> tempListOfArticle = new List<Article>();

            Console.WriteLine("max;; " + maxNameLength);

            foreach (Article c in listOfArticle)
            {
                if (c.Name.Length > maxNameLength && firstRun) maxNameLength = c.Name.Length;

                // bail if input length surpass a name in main list
                if (c.Name.Length < incStr.Length)
                {
                    continue;
                }
                // is input inside name
                if (c.Name.ToLower().Contains(incStr.ToLower()))
                {
                    tempListOfArticle.Add(c);
                    recyclerViewSetup(fragmentView, tempListOfArticle);
                    Console.WriteLine("if " + c.Name.Substring(0, c.Name.Length));

                }
                // don't add non equal names to display list
                else
                {
                    if (tempListOfArticle.Count > 0)
                        continue;
                    tempListOfArticle.Clear();
                    recyclerViewSetup(fragmentView, tempListOfArticle);
                    Console.WriteLine("else " + c.Name.Substring(0, c.Name.Length));

                }
            }
            firstRun = false;
            //Console.WriteLine("max;; " + maxNameLength);
        }
        private void SearchIB_Click(object sender, EventArgs e)
        {
            // ui setup
            if (isSearch)
            {
                searchIB.SetImageResource(Resource.Drawable.closeIcon);
                searchET.Visibility = ViewStates.Visible;
                titleTV.Visibility = ViewStates.Invisible;
                searchIB.LayoutParameters.Width = 50;
                searchIB.LayoutParameters.Height = 50;
                searchET.RequestFocus();


                LinearLayout tab = Activity.FindViewById<LinearLayout>(Resource.Id.fakeTabLayout);
                View shadow = Activity.FindViewById<View>(Resource.Id.shadow);
                RelativeLayout rel = Activity.FindViewById<RelativeLayout>(Resource.Id.containerView);

                tab.Visibility = ViewStates.Invisible;
                shadow.Visibility = ViewStates.Invisible;
                rel.SetPadding(0, 0, 0, 0);
                var imm = searchET.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                imm.ToggleSoftInput(ShowFlags.Implicit, 0);

            }
            else
            {
                searchET.Text = "";

                searchIB.SetImageResource(Resource.Drawable.searchIcon);
                searchET.Visibility = ViewStates.Invisible;
                titleTV.Visibility = ViewStates.Visible;
                searchIB.LayoutParameters.Width = 75;
                searchIB.LayoutParameters.Height = 75;

                LinearLayout tab = Activity.FindViewById<LinearLayout>(Resource.Id.fakeTabLayout);
                View shadow = Activity.FindViewById<View>(Resource.Id.shadow);
                RelativeLayout rel = Activity.FindViewById<RelativeLayout>(Resource.Id.containerView);

                tab.Visibility = ViewStates.Visible;
                shadow.Visibility = ViewStates.Visible;
                rel.SetPadding(0, 0, 0, 160);
                var imm = searchET.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
                imm.ToggleSoftInput(0, HideSoftInputFlags.ImplicitOnly);
            }
            isSearch = !isSearch;

        }

        private void NextTV_Click(object sender, EventArgs e)
        {
            if (nextState)
            {
                GlobVars.devisArticleDone = true;
                Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisSummary()).Commit();

            }
        }

        private void GobackIB_Click(object sender, EventArgs e)
        {
            GlobVars.devisArticleDone = true;
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisClient()).Commit();

        }

        public void recyclerViewSetup(View fragmentView, List<Article> listOfArticles)
        {
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.articlerv);
            mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listOfArticles, this);
            rv.SetAdapter(adapter);
        }
        private async void midSync(View vf)
        {
            //List<Article> listOfArticle = new List<Article>();
            await CallAPI(vf);
            //for (int i = 0; i < 100; i++)
            //{
            //    Article c = new Article();
            //    c.Name = i.ToString();
            //    listOfArticle.Add(c);
            //}
            //recyclerViewSetup(vf, listOfArticle);
        }
        private async Task CallAPI(View vf)
        {
            try
            {
                string uri = "";

                //only for testing with current emulator
                if (Build.Hardware.Contains("ranchu"))
                {
                    uri = "https://10.0.2.2:44317/api/Devis/GetArticle";
                    Console.WriteLine("********* ranchu emu *******");
                }
                else
                    //ip = pc(host) ip address, port = extension remote url port 
                    uri = "https://192.168.9.97:45461/api/Devis/GetArticle";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient httpClient = new HttpClient(clientHandler);
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);
                showList();


                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);


                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);


                    listOfArticle = JsonConvert.DeserializeObject<List<Article>>(prettyJson);

                    Console.WriteLine("************** length of list article: " + listOfArticle.Count);

                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
                //after retrieving the list of client from the server we setup the recyclerview
                recyclerViewSetup(vf, listOfArticle);
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
            }
        }
        void showList()
        {
            img.Visibility = ViewStates.Gone;
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