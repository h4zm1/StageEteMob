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
    public class _NewDevisClient : AndroidX.Fragment.App.Fragment
    {
        TextView nextTV;
        ImageButton gobackIB;
        Boolean nextState = false;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var fragmentView = inflater.Inflate(Resource.Layout._newDevis_client, container, false);

            nextTV = fragmentView.FindViewById<TextView>(Resource.Id.nexttv);
            gobackIB = fragmentView.FindViewById<ImageButton>(Resource.Id.goback);

            gobackIB.Click += GobackIB_Click;
            midSync(fragmentView);

            return fragmentView;
        }

        private void GobackIB_Click(object sender, EventArgs e)
        {
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new _NewDevisName()).Commit();

        }

        public void recyclerViewSetup(View fragmentView, List<Client> listOfClients)
        {
            List<Client> listClients = listOfClients;
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.clientrv);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            RVAdapter adapter = new RVAdapter(listClients, this);
            rv.SetAdapter(adapter);
        }
        private async void midSync(View vf)
        {
            List<Client> listOfClient = new List<Client>();
            //await CallAPI(vf);
            for(int i =0; i<100; i++)
            {
                Client c = new Client();
                c.Nom = i.ToString();
                listOfClient.Add(c);
            }
            recyclerViewSetup(vf, listOfClient);

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
                //after retrieving the list of client from the server we setup the recyclerview
                recyclerViewSetup(vf, listOfClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
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