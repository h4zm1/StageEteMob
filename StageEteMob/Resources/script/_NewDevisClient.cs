﻿using Android.App;
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

namespace StageEteMob
{
    public class _NewDevisClient : AndroidX.Fragment.App.Fragment
    {
        TextView nextTV;
        ImageButton gobackIB;
        Boolean nextState = false;
        LottieAnimationView img;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._newDevis_client, container, false);

            nextTV = fragmentView.FindViewById<TextView>(Resource.Id.nexttv);
            gobackIB = fragmentView.FindViewById<ImageButton>(Resource.Id.goback);
            img = fragmentView.FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            gobackIB.Click += GobackIB_Click;
            midSync(fragmentView);
            nextTV.Click += NextTV_Click;
            return fragmentView;
        }

        private void NextTV_Click(object sender, EventArgs e)
        {
            if (nextState)
            {
                GlobVars.devisClientDone = true;
                Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisArticle()).Commit();
            }
        }

        private void GobackIB_Click(object sender, EventArgs e)
        {
            GlobVars.devisClientDone = true;
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisName()).Commit();
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
            //    c.Nom = "a"+i.ToString();
            //    listOfClient.Add(c);
            //}
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
                List<Client> listOfClient = null;

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