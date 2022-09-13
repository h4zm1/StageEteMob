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
using AndroidX.Core.OS;
using AndroidX.Lifecycle;
using Android.Nfc;
using System.Timers;

namespace StageEteMob
{
    public class _DevisFrag : AndroidX.Fragment.App.Fragment
    {
        SearchFrag parent;
        int days = 30;
        TextView thirtyTv;
        TextView sixtyTv;
        TextView ninetyTv;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._content_devis, container, false);

            //part of the loading animation setup
            if (Arguments != null)
            {
                //saving a reference of SearchFrag
                parent = (SearchFrag)Arguments.GetSerializable("parent");
            }

            thirtyTv = fragmentView.FindViewById<TextView>(Resource.Id.thirtyDays);
            sixtyTv = fragmentView.FindViewById<TextView>(Resource.Id.sixtydays);
            ninetyTv = fragmentView.FindViewById<TextView>(Resource.Id.nintydays);
            thirtyTv.Click += (o, i) =>
            {
                days = 30;
                thirtyTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
                sixtyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                ninetyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                midSync(fragmentView);

            };
            sixtyTv.Click += (o, i) =>
            {
                days = 60;
                thirtyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                sixtyTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
                ninetyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                midSync(fragmentView);

            };
            ninetyTv.Click += (o, i) =>
            {
                days = 90;
                thirtyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                sixtyTv.SetTextColor(Android.Graphics.Color.Rgb(152, 152, 152));
                ninetyTv.SetTextColor(Android.Graphics.Color.Rgb(34, 34, 34));
                midSync(fragmentView);

            };
            midSync(fragmentView);
            return fragmentView;
        }
        public void recyclerViewSetup(View fragmentView, List<Devis> listOfDevis)
        {
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.devisRecyclerView);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listOfDevis);
            rv.SetAdapter(adapter);
        }
        private async void midSync(View vf)
        {
            await CallAPI(vf);
        }
        private async Task CallAPI(View vf)
        {
            try
            {
                string uri = "";

                //only for testing with current emulator
                if (Build.Hardware.Contains("ranchu"))
                {
                    uri = "https://10.0.2.2:44317/api/Devis/Post";
                    Console.WriteLine("********* ranchu emu *******");
                }
                else
                    //ip = pc(host) ip address, port = extension remote url port 
                    uri = "https://192.168.1.2:45461/api/Devis/GetTimedDevis";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                var client = new HttpClient(clientHandler);
                var httpContent = new StringContent("");

                var httpResponse = await client.PostAsync(uri + "?days=" + days, httpContent);

                //this how to retrieve the returned string from the POST call
                var fromPost = await httpResponse.Content.ReadAsStringAsync();
                //show this fragment
                parent.startAndPass(this, null);
                List<Devis> listOfDevis = null;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);


                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);


                    listOfDevis = JsonConvert.DeserializeObject<List<Devis>>(prettyJson);

                    Console.WriteLine("************** length of list deviis: " + listOfDevis.Count);

                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }


                //after retrieving the list of client from the server we setup the recyclerview
                recyclerViewSetup(vf, listOfDevis);
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
            }
        }


    }
}