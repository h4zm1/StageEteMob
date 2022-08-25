using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StageEteMob.Resources.script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StageEteMob
{
    public class _ClientFrag : AndroidX.Fragment.App.Fragment
    {
        SearchFrag parent;
        TextView clientCountTV;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._content_client, container, false);
            clientCountTV = fragmentView.FindViewById<TextView>(Resource.Id.clientCounterTV);
            if (Arguments != null)
            {
                //saving a reference of SearchFrag
                parent = (SearchFrag)Arguments.GetSerializable("parent");
            }
            midSync(fragmentView);
            return fragmentView;
        }


        public void recyclerViewSetup(View fragmentView, List<Client> listOfClients)
        {
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.clientRecyclerView);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listOfClients);
            rv.SetAdapter(adapter);
        }
        private async void midSync(View vf)
        {
            await CallAPI(vf);

            //List<Client> listOfClient = new List<Client>();
            //for (int i = 0; i < 100; i++)
            //{
            //    Client c = new Client();
            //    c.Nom = "C" + i.ToString();
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
                //show this fragment when the list is ready (got downloaded)
                parent.startAndPass(this, null);
                List<Client> listOfClient = null;

                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);


                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);


                    listOfClient = JsonConvert.DeserializeObject<List<Client>>(prettyJson);

                    Console.WriteLine("************** length of list article: " + listOfClient.Count);

                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
                ///TODO
                ///Cache the list
                clientCountTV.Text = "Number of Clients: "+listOfClient.Count.ToString();
                //after retrieving the list of client from the server we setup the recyclerview
                recyclerViewSetup(vf, listOfClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
            }
        }
    }
}