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
    public class DevisFrag : AndroidX.Fragment.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_devis, container, false);

            TextView jsonDisplay = fragmentView.FindViewById<TextView>(Resource.Id.tvJson);

            MaterialButton btn = fragmentView.FindViewById<MaterialButton>(Resource.Id.signoutBtn);
            btn.Click += initEvent;

            midSync(fragmentView);

            return fragmentView;
        }

        public void recyclerViewSetup(View fragmentView, List<Article> listOfArticle)
        {
            List<Article> listArticle = listOfArticle;
            RecyclerView rv = fragmentView.FindViewById<RecyclerView>(Resource.Id.recyclerView1);
            RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(this.Context);
            rv.SetLayoutManager(mLayoutManager);

            ArtAdapter adapter = new ArtAdapter(listArticle);
            rv.SetAdapter(adapter);
        }
        private void initEvent(object sender, EventArgs eventArgs)
        {
            //go back to sign in fragment
            Activity.SupportFragmentManager.BeginTransaction().Replace(Resource.Id.containerView, new SignInFrag()).Commit();
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
                    uri = "https://10.0.2.2:44317/api/Devis";
                    Console.WriteLine("********* ranchu emu *******");
                }
                else
                    //ip = pc(host) ip address, port = extension remote url port 
                    uri = "https://192.168.9.97:45461/api/Devis";

                //bypassing SSLHandshakeException
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                };

                HttpClient httpClient = new HttpClient(clientHandler);
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);

                List<Article> listOfArticle = null;

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
    }
}