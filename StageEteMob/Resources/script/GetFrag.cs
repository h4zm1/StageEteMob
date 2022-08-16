using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StageEteMob
{
    public class GetFrag : AndroidX.Fragment.App.Fragment
    {
        TextView nbrClient_TV;
        ListView listV;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var fragmentView = inflater.Inflate(Resource.Layout.content_get, container, false);

            listV = fragmentView.FindViewById<ListView>(Resource.Id.listView1);
            nbrClient_TV = fragmentView.FindViewById<TextView>(Resource.Id.textView1);
            midSync();

            return fragmentView;
        }

        private async void midSync()
        {
            //await CallAPI();

            await CallAPI();
        }
        struct client
        {
            public string Code;
            public string Nom;
            public string Mf;
            public string Tel;
            public string Mail;
            public string Adresse;
        };
        private async Task CallAPI()
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
                httpClient.MaxResponseContentBufferSize = 9999999;
                HttpResponseMessage httpResponse = await httpClient.GetAsync(uri);



                if (httpResponse.IsSuccessStatusCode)
                {
                    string result = await httpResponse.Content.ReadAsStringAsync();
                    var JsonString = JsonConvert.DeserializeObject<string>(result);


                    string prettyJson = JToken.Parse(JsonString).ToString(Formatting.Indented);
                    //editText.Text = prettyJson;

                    //convert the json we getting from server to list of client (struct)
                    client[] deptList = JsonConvert.DeserializeObject<client[]>(prettyJson);

                    IList<String> listOfName = new List<string>();

                    //filling a list with clients names
                    int c = 1;
                    foreach (var v in deptList)
                    {
                        listOfName.Add(c+ " : "+v.Nom.ToString());
                        Console.WriteLine(c+ " names:: " + v.Nom.ToString());
                        c++;
                    }

                    //displaying the number of clients
                    nbrClient_TV.Text = "number of clients: "+deptList.Count().ToString();


                    //linking the list of clients names to the list view
                    listV.Adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleListItem1, listOfName);



                    Console.WriteLine("************** length of json string: " + deptList.Count());
                    //Console.WriteLine("************** Result: " + prettyJson);

                }
                else
                {
                    Console.WriteLine("************** failed " + httpResponse.StatusCode + " " + httpResponse.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("************** Error Message: " + ex.Message);
                Console.WriteLine("************** Stack Trace: " + ex.StackTrace);
            }
        }
    }
}