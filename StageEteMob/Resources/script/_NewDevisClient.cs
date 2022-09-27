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

namespace StageEteMob
{
    public class _NewDevisClient : AndroidX.Fragment.App.Fragment
    {
        TextView nextTV, titleTV;
        ImageButton searchIB;
        EditText searchET;
        public Boolean nextState = false;
        Boolean isSearch = true;
        LottieAnimationView img;
        List<Client> listOfClient = new List<Client>();
        View fragmentView;

        int maxNameLength = 0;
        Boolean firstRun = true;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            fragmentView = inflater.Inflate(Resource.Layout._newDevis_client, container, false);

            nextTV = fragmentView.FindViewById<TextView>(Resource.Id.nexttv);
            titleTV = fragmentView.FindViewById<TextView>(Resource.Id.titleTV);

            searchIB = fragmentView.FindViewById<ImageButton>(Resource.Id.searchIB);
            searchET = fragmentView.FindViewById<EditText>(Resource.Id.searchET);

            img = fragmentView.FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            searchET.TextChanged += SearchET_TextChanged;
            searchIB.Click += SearchIB_Click;
            midSync(fragmentView);
            nextTV.Click += NextTV_Click;
            return fragmentView;
        }

        private void SearchET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            searchMeth(e.Text.ToString());
        }

        void searchMeth(string incStr)
        {
            Console.WriteLine(incStr);
            List<Client> tempListOfClient = new List<Client>();

            Console.WriteLine("max;; " + maxNameLength);

            foreach (Client c in listOfClient)
            {
                if (c.Nom.Length > maxNameLength && firstRun) maxNameLength = c.Nom.Length;

                // bail if input length surpass a name in main list
                if (c.Nom.Length < incStr.Length)
                {
                    Console.WriteLine(" <<<<<<<<<<<<< ");
                    continue;
                }
                // is input inside name
                if (c.Nom.ToLower().Contains(incStr.ToLower()))
                {
                    tempListOfClient.Add(c);
                    recyclerViewSetup(fragmentView, tempListOfClient);
                    Console.WriteLine("if " + c.Nom.Substring(0, c.Nom.Length));

                }
                // don't add non equal names to display list
                else
                {
                    if (tempListOfClient.Count > 0)
                        continue;
                    tempListOfClient.Clear();
                    recyclerViewSetup(fragmentView, tempListOfClient);
                    Console.WriteLine("else " + c.Nom.Substring(0, c.Nom.Length));

                }
            }
            firstRun = false;
            //Console.WriteLine("max;; " + maxNameLength);
        }
        private void NextTV_Click(object sender, EventArgs e)
        {
            if (nextState)
            {
                GlobVars.devisClientDone = true;
                Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisArticle()).Commit();
            }
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

        public void recyclerViewSetup(View fragmentView, List<Client> listOfClients)
        {
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.clientrv);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);

            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listOfClients, this);
            rv.SetAdapter(adapter);
        }
        private async void midSync(View vf)
        {
            await CallAPI(vf);

            //List<Client> listOfClient = new List<Client>();
            //for (int i = 0; i < 100; i++)
            //{
            //    Client c = new Client();
            //    c.Nom = "a" + i.ToString();
            //    c.Code = i+5;
            //    listOfClient.Add(c);
            //}
            //Client c0 = new Client();
            //c0.Nom = "aa2";
            //listOfClient.Add(c0);
            //Client c2 = new Client();
            //c2.Nom = "aa2";
            //listOfClient.Add(c2);
            //Client c3 = new Client();
            //c3.Nom = "ca";
            //listOfClient.Add(c3);
            //Client c4 = new Client();
            //c4.Nom = "caa";
            //listOfClient.Add(c4);
            //showList();
            //recyclerViewSetup(vf, listOfClient);

        }
        private async Task CallAPI(View vf)
        {
            try
            {
                string uri = "";

                //only for testing with current emulator
                if (Build.Hardware.Contains("ranchu"))
                {
                    uri = "https://10.0.2.2:44317/api/Client/Get";
                    Console.WriteLine("********* ranchu emu *******");
                }
                else
                    //ip = pc(host) ip address, port = extension remote url port 
                    uri = "https://192.168.9.97:45461/api/Client/Get";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient httpClient = new HttpClient(clientHandler);
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);
                showList();
                //List<Client> listOfClient = null;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);

                    //cleaning up the string (not a pure json)
                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);

                    //yes, it works 
                    listOfClient = JsonConvert.DeserializeObject<List<Client>>(prettyJson);

                    Console.WriteLine("************** length of list client: " + listOfClient.Count);

                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
                ///TODO
                ///Cache the list
                //after retrieving the list of client from the server we setup the recyclerview
                recyclerViewSetup(vf, listOfClient);
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