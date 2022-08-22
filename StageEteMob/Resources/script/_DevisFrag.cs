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
    public class _DevisFrag : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._content_devis, container, false);

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

            //await CallAPI(vf);
            List<Devis> listOfDevis = new List<Devis>();
            for (int i = 0; i < 100; i++)
            {
                Devis c = new Devis();
                c.code = "D"+i.ToString();
                listOfDevis.Add(c);
            }
            recyclerViewSetup(vf, listOfDevis);
        }
        private async Task CallAPI(View vf)
        {
            try
            {
                string uri = "";

                //only for testing with current emulator
                if (Build.Hardware.Contains("ranchu"))
                {
                    uri = "https://10.0.2.2:44317/api/Devis/GetTimedDevis";
                    Console.WriteLine("********* ranchu emu *******");
                }
                else
                    //ip = pc(host) ip address, port = extension remote url port 
                    uri = "https://192.168.9.97:45461/api/Devis/GetTimedDevis";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient httpClient = new HttpClient(clientHandler);
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);

                List<Devis> listOfDevis = null;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);


                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);


                    listOfDevis = JsonConvert.DeserializeObject<List<Devis>>(prettyJson);

                    Console.WriteLine("************** length of list article: " + listOfDevis.Count);

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